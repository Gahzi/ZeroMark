using KBConstants;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(CharacterController))]
public class PlayerLocal : KBControllableGameObject
{

    #region CONSTANTS
    private static readonly float hitFXLength = 0.250f;
    #endregion

    public enum ControlStyle { ThirdPerson, TopDown };

    public ControlStyle controlStyle;
    public PlayerType type;
    public PlayerStats stats;
    public bool acceptingInputs = true;
    public bool waitingForRespawn = false;
    public float invulnerabilityTime;
    public float spawnProtectionTime;
    public float bankLockoutTime;
    public float boostTime;
    public float boostRechargeRate;
    public float boostMax;

    public int level;

    public TimerScript timer;
    private float movespeed;
    private float lowerbodyRotateSpeed;
    private float upperbodyRotateSpeed;

    private CharacterController charController;

    private int layerMask;

    public string playerName;
    public PhotonPlayer networkPlayer;

    private Vector3 latestCorrectPos;
    private Vector3 onUpdatePos;
    private float fraction;
    private bool isShooting;

    public AudioClip grabSound;
    public GameObject upperBody;
    public Camera camera;
    public Vector3 mousePos;
    public Vector3 playerPositionOnScreen;
    public Vector3 mousePlayerDiff;
    public ProjectileAbilityBaseScript gun;
    public List<PlayerSpawnPoint> teamSpawnpoints;
    public float respawnTime;
    private Vector3 lookRotation;
    private int respawnTimerNumber;
    public int upgradePoints;
    public int maxHealth;
    public int[] pointToLevel = new int[4];
    private Item item;
    RotatableGuiItem playerGuiSquare;
    public Material redMat;
    public Material blueMat;
    public MeshRenderer teamIndicator;
    public bool canCapture;
    private AudioClip hitConfirm;
    private float hitFXTimer;
    public GameObject hitExplosion;
    public AudioClip gotHitSFX;
    public AudioClip deadSound;
    public AudioClip respawnSound;
    public AudioClip dropSound;

    public int killTokens;
    private bool triggerLockout;

    public override void Start()
    {
        // NOTE: All connected abilities & hitboxes need to be sent the player's team.
        
        base.Start();
        acceptingInputs = true;
        waitingForRespawn = false;
        triggerLockout = false;
        charController = GetComponent<CharacterController>();
        grabSound = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.ItemGrab]);
        playerGuiSquare = GetComponent<RotatableGuiItem>();
        latestCorrectPos = transform.position;
        onUpdatePos = transform.position;
        isShooting = false;
        hitConfirm = Resources.Load<AudioClip>(KBConstants.AudioConstants.CLIP_NAMES[KBConstants.AudioConstants.clip.HitConfirm]);
        SetupAbilities();
        GetComponentInChildren<HitboxBaseScript>().Team = team;

        movespeed = stats.speed;
        lowerbodyRotateSpeed = stats.lowerbodyRotationSpeed;

        int itemLayer = 8;
        int towerLayer = 13;
        int modelLayer = 15;
        int layerMask1 = 1 << itemLayer;
        int layerMask2 = 1 << towerLayer;
        int layerMask3 = 1 << modelLayer;
        layerMask = layerMask1 | layerMask2 | layerMask3;

        teamSpawnpoints = new List<PlayerSpawnPoint>();

        if (team != Team.None)
        {
            FindTeamSpawnpoints();
            if (teamSpawnpoints.Count > 0)
            {
                Respawn();
            }

        }
        else
        {
            enabled = false;
        }

        switch (team)
        {
            case Team.Red:
                teamIndicator.material = redMat;
                break;
            case Team.Blue:
                teamIndicator.material = blueMat;
                break;
            case Team.None:
                break;
            default:
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(upperBody.transform.position, upperBody.transform.TransformDirection(new Vector3(0, 0, 5.0f)), new Color(255, 0, 0, 255), 0.0f);
    }

    private void FixedUpdate()
    {
        
        /* TODO:
         * Fix aiming
         * Add indication of invulnerability
         */

        if (invulnerabilityTime > 0)
        {
            invulnerabilityTime -= Time.deltaTime;
            // activate visual indication of invulnerability
        }
        if (triggerLockout)
        {
            StartCoroutine(Lockout(bankLockoutTime));
        }

        if (!Input.GetButton("Boost"))
        {
            boostTime += boostRechargeRate * Time.deltaTime ;
            boostTime = Mathf.Clamp(boostTime, 0.0f, boostMax);
        }

        mousePos = Input.mousePosition;
        fraction = fraction + Time.deltaTime * 9;
        
        if (photonView.isMine)
        {
            playerPositionOnScreen = camera.WorldToScreenPoint(transform.position);
            mousePlayerDiff = playerPositionOnScreen - mousePos;

            if (acceptingInputs)
            {
                ControlKBAM();
            }
        }
        else
        {
            transform.localPosition = Vector3.Lerp(onUpdatePos, latestCorrectPos, fraction);    // set our pos between A and B
        }

        if (isShooting && !gun.GetActive())
        {
            gun.ActivateAbility();
        }
        else if (!isShooting && gun.GetActive())
        {
            gun.DeactivateAbility();
        }

        if (item != null)
        {
            item.transform.position = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);
            item.state = Item.ItemState.isPickedUp;
            if (canCapture != false)
            {
                canCapture = false;
            } 
        }
        else
        {
            if (canCapture != true)
            {
                canCapture = true;
            } 
        }

        CheckHealth(); 
    }  
    
    void OnGUI()
    {
        if (networkPlayer.isLocal)
        {
            GUI.Box(new Rect(0, 0, 100, 40), "Kill Tokens" + System.Environment.NewLine + killTokens.ToString());
            GUI.Box(new Rect(0, 40, 100, 40), "Boost" + System.Environment.NewLine + boostTime.ToString("0.00"));
        }
    }
    

    private void OnPhotonInstantiate(PhotonMessageInfo msg)
    {
        // This is our own player
        if (photonView.isMine)
        {
            GameObject newPlayerCameraObject = (GameObject)Instantiate(Resources.Load(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.PlayerCamera]));
            newPlayerCameraObject.transform.parent = transform;
            newPlayerCameraObject.GetComponent<KBCamera>().attachedPlayer = this;
            camera = newPlayerCameraObject.GetComponent<Camera>();

        }
        // This is just some remote controlled player, don't execute direct
        // user input on this. DO enable multiplayer controll
        else
        {
            name += msg.sender.name;
        }

        networkPlayer = msg.sender;
        GameManager.Instance.players.Add(this);
    }

    /// <summary>
    /// While script is observed (in a PhotonView), this is called by PUN with a stream to write or read.
    /// </summary>
    /// <remarks>
    /// The property stream.isWriting is true for the owner of a PhotonView. This is the only client that
    /// should write into the stream. Others will receive the content written by the owner and can read it.
    ///
    /// Note: Send only what you actually want to consume/use, too!
    /// Note: If the owner doesn't write something into the stream, PUN won't send anything.
    /// </remarks>
    /// <param name="stream">Read or write stream to pass state of this GameObject (or whatever else).</param>
    /// <param name="info">Some info about the sender of this stream, who is the owner of this PhotonView (and GameObject).</param>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            Vector3 pos = transform.localPosition;
            Quaternion rot = upperBody.transform.rotation;
            int hlth = health;
            int lvl = level;
            int mxhlth = maxHealth;
            bool isShting = isShooting;
            bool cnCpture = canCapture;
            int tm = (int)team;
            int kt = killTokens;

            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
            stream.Serialize(ref hlth);
            stream.Serialize(ref lvl);
            stream.Serialize(ref mxhlth);
            stream.Serialize(ref isShting);
            stream.Serialize(ref cnCpture);
            stream.Serialize(ref tm);
            stream.Serialize(ref kt);
        }
        else
        {
            // Receive latest state information
            Vector3 pos = Vector3.zero;
            Quaternion rot = Quaternion.identity;
            int hlth = 0;
            int lvl = 0;
            int mxhlth = 0;
            bool isShting = false;
            bool cnCpture = false;
            int tm = 0;
            int kt = 0;

            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
            stream.Serialize(ref hlth);
            stream.Serialize(ref lvl);
            stream.Serialize(ref mxhlth);
            stream.Serialize(ref isShting);
            stream.Serialize(ref cnCpture);
            stream.Serialize(ref tm);
            stream.Serialize(ref kt);

            latestCorrectPos = pos;                 // save this to move towards it in FixedUpdate()
            onUpdatePos = transform.localPosition;  // we interpolate from here to latestCorrectPos
            fraction = 0;                           // reset the fraction we alreay moved. see Update()

            upperBody.transform.rotation = rot;          // this sample doesn't smooth rotation
            health = hlth;
            level = lvl;
            maxHealth = mxhlth;
            isShooting = isShting;
            canCapture = cnCpture;
            team = (Team)tm;
            killTokens = kt;
        }
    }

    /// <summary>
    /// Collision method that is called when rigidbody hits another game object
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        PlasmaBullet bullet = collision.gameObject.GetComponent<PlasmaBullet>();
        if (bullet != null)
        {
            Debug.Log("Got Hit!");
            takeDamage(bullet.damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            Item i = other.gameObject.GetComponent<Item>();
            if (i.state == Item.ItemState.isDown && i != null)
            {
                if (i.team == team)
                {
                    PickupItem(other.gameObject.GetComponent<Item>());
                }
                else
                {
                    i.Respawn();
                    //other.gameObject.particleSystem.enableEmission = true;
                    ////Destroy(other.gameObject);
                }
            }
        }
    }

    public override int takeDamage(int amount)
    {
        if (invulnerabilityTime <= 0)
        {
            health -= amount;
            Instantiate(hitExplosion, transform.position, Quaternion.identity);
            
            if (photonView.isMine)
            {
                camera.GetComponent<ScreenShake>().StartShake(0.25f, 5.0f);
                audio.PlayOneShot(gotHitSFX);
            }

           
        }
        return health;
    }

    private void ControlKBAM()
    {
        float modifiedMoveSpeed = 0;
        if (item != null || (Input.GetButton("Boost") && boostTime == boostMax))
        {
            modifiedMoveSpeed = movespeed * 10;
            boostTime = 0;
        }
        else
        {
            modifiedMoveSpeed = movespeed;
        }

        switch (controlStyle)
        {
            case ControlStyle.ThirdPerson:
                float d = modifiedMoveSpeed * Input.GetAxis("Vertical");
                charController.SimpleMove(transform.TransformDirection(Vector3.forward) * d);
                transform.Rotate(0, Input.GetAxis("Horizontal") * lowerbodyRotateSpeed, 0);

                break;

            case ControlStyle.TopDown:
                Vector3 m = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                charController.SimpleMove(m.normalized * modifiedMoveSpeed);

                Quaternion newRot = Quaternion.LookRotation(upperBody.transform.position + new Vector3(-mousePlayerDiff.x, 0, -mousePlayerDiff.y));
                upperBody.transform.rotation = Quaternion.Lerp(upperBody.transform.rotation, newRot, upperbodyRotateSpeed * Time.deltaTime);
                break;

            default:
                break;
        }

        if (Input.GetMouseButtonDown(0))
        {
            //if (item != null)
            //{
                isShooting = true;
            //}
        }
        if (Input.GetMouseButtonUp(0))
        {
            isShooting = false;
        }

        if (Input.GetMouseButtonDown(1))
        {
            //SHERVIN: This must be sent across network.
            DropItem();
        }

        // DEBUG FUNCTIONS
        if (Input.GetKeyDown(KeyCode.T))
        {
            takeDamage(1);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            BankKills();
        }
    }

    private void CheckHealth()
    {
        if (health <= 0 && !waitingForRespawn) // should respawn
        {
            respawnTimerNumber = timer.StartTimer(respawnTime);
            DropItem();
            waitingForRespawn = true;
            audio.PlayOneShot(deadSound);
        }
        else if (waitingForRespawn)
        {
            acceptingInputs = false;
            if (!timer.IsTimerActive(respawnTimerNumber))
            {
                Debug.Log("Respawning!");
                Respawn();
            }
        }
    }

    private void FindTeamSpawnpoints()
    {
        if (team == Team.None)
        {
            Debug.LogWarning("Warning: Attempting to find spawnpoint on player with team none");
        }
        else
        {
            teamSpawnpoints = GameManager.Instance.GetSpawnPointsWithTeam(team);
        }
    }

    private void Respawn()
    {
        if (teamSpawnpoints.Count > 0)
        {
            transform.position = teamSpawnpoints[0].transform.position;
            waitingForRespawn = false;
            acceptingInputs = true;
            health = stats.health;
            maxHealth = health;
            movespeed = stats.speed;
            level = stats.level;
            killTokens = 0;
            invulnerabilityTime = spawnProtectionTime;
            lowerbodyRotateSpeed = stats.lowerbodyRotationSpeed;
            upperbodyRotateSpeed = stats.upperbodyRotationSpeed;

            if (photonView.isMine)
            {
                camera.GetComponent<ScreenShake>().StopShake();
            }
            
            audio.PlayOneShot(respawnSound);
        }
    }

    private void PickupItem(Item _item)
    {
        item = _item;
        item.state = Item.ItemState.isPickedUp;
        audio.PlayOneShot(grabSound);
    }

    private void DropItem()
    {
        if (item != null)
        {
            //item.Respawn();
            item.state = Item.ItemState.disabled;
            item.disableTime = Time.time;
            item = null;
            audio.PlayOneShot(dropSound);
        }
    }

    private void RunHitFX()
    {
        if (hitFXTimer > 0)
        {
            hitFXTimer -= Time.deltaTime;
        }
    }

    private void SetupAbilities()
    {
        gun = gameObject.GetComponentInChildren<ProjectileAbilityBaseScript>();
        gun.SetMaxRange(stats.attackRange);
        gun.owner = this;
        gun.Team = team;
    }

    public void ConfirmHit()
    {
        gun.audio.PlayOneShot(hitConfirm);
    }

    public void Die(GameObject killerObject)
    {
        PlayerLocal killerPlayer = killerObject.GetComponent<PlayerLocal>();
        killerPlayer.photonView.RPC("NotifyKill", PhotonTargets.Others, killerPlayer.networkPlayer);
    }

    [RPC]
    public void NotifyKill(PhotonPlayer killerPlayer)
    {
        if (killerPlayer.isLocal)
        {
            if (killTokens < 3)
            {
                killTokens++;
            }
            else
            {
                killTokens *= 2;
            }
        }
    }

    public void BankKills()
    {
        invulnerabilityTime = bankLockoutTime;
        triggerLockout = true;
        // make them look different
    }

    private IEnumerator Lockout(float time)
    {
        acceptingInputs = false;
        yield return new WaitForSeconds(time);
        acceptingInputs = true;
        killTokens = GameManager.Instance.AddPointsToScore(team, killTokens);
        triggerLockout = false;
    }
}