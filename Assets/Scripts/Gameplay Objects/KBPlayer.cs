using KBConstants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(CharacterController))]
public class KBPlayer : KBControllableGameObject
{
    #region CONSTANTS

    private static readonly float hitFXLength = 0.250f;

    #endregion CONSTANTS

    public enum ControlStyle { ThirdPerson, TopDown };

    public ControlStyle controlStyle;
    private bool tankStyleMove;
    public PlayerType type;
    public PlayerStats stats;
    public bool acceptingInputs = true;
    public bool waitingForRespawn = false;
    public float invulnerabilityTime;
    public float spawnProtectionTime;
    public float bankLockoutTime;

    public TimerScript timer;
    private float movespeed;
    private float lowerbodyRotateSpeed;
    private float upperbodyRotateSpeed;

    private CharacterController charController;

    public string playerName;
    public PhotonPlayer networkPlayer;

    private Vector3 latestCorrectPos;
    private Vector3 onUpdatePos;
    private float fraction;

    public AudioClip itemPickupClip;
    public GameObject upperBody;
    public GameObject lowerBody;
    //public GameObject hitbox;
    public GameObject hitboxDrone;
    public GameObject hitboxMech;
    public GameObject hitboxTank;

    #region Player Type Components

    public GameObject upperBodyDrone;
    public GameObject lowerBodyDrone;
    public GameObject upperBodyMech;
    public GameObject lowerBodyMech;
    public GameObject upperBodyTank;
    public GameObject lowerBodyTank;
    public GameObject upperBodyCore;
    public GameObject lowerBodyCore;
    private List<GameObject> allBodies;

    #endregion Player Type Components

    public Vector3 mousePos;
    public Vector3 playerPositionOnScreen;
    public Vector3 mousePlayerDiff;

    public List<PlayerSpawnPoint> teamSpawnpoints;
    public float respawnTime;
    private int respawnTimer;
    //public int upgradePoints;
    public int maxHealth;

    public Material redMat;
    public Material blueMat;
    //public MeshRenderer teamIndicator;
    private AudioClip hitConfirm;
    private float hitFXTimer;
    public GameObject hitExplosion;
    public AudioClip[] gotHitSFX;
    public AudioClip deadSound;
    public AudioClip respawnSound;
    public AudioClip dropSound;

    public ProjectileAbilityBaseScript[] gun;
    public int killTokens;
    private bool triggerLockout;

    private bool secondaryWeaponLinkedFire;
    private int lastPrimaryFire;
    private bool primaryWeaponLinkedFire;
    private int lastSecondaryFire;

    public new void Awake()
    {
        base.Awake();

        allBodies = new List<GameObject>(6);
        allBodies.Add(upperBodyDrone);
        allBodies.Add(upperBodyMech);
        allBodies.Add(upperBodyTank);
        allBodies.Add(upperBodyCore);
        allBodies.Add(lowerBodyDrone);
        allBodies.Add(lowerBodyMech);
        allBodies.Add(lowerBodyTank);
        allBodies.Add(lowerBodyCore);
        //allBodies.Add(hitboxDrone);
        //allBodies.Add(hitboxMech);
        //allBodies.Add(hitboxTank);
    }

    public override void Start()
    {
        base.Start();

        Screen.showCursor = false;

        #region Resource & reference loading
        charController = GetComponent<CharacterController>();
        itemPickupClip = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.ItemPickup01]);
        hitConfirm = Resources.Load<AudioClip>(KBConstants.AudioConstants.CLIP_NAMES[KBConstants.AudioConstants.clip.HitConfirm]);
        hitboxDrone.GetComponent<HitboxBaseScript>().Team = team;
        hitboxMech.GetComponent<HitboxBaseScript>().Team = team;
        hitboxTank.GetComponent<HitboxBaseScript>().Team = team;
        //GetComponentInChildren<HitboxBaseScript>().Team = team;

        #endregion Resource & reference loading

        latestCorrectPos = transform.position;
        onUpdatePos = transform.position;

        

        InitializeForRespawn();

        #region Spawning

        teamSpawnpoints = new List<PlayerSpawnPoint>();
        FindTeamSpawnpoints();
        RespawnToPrespawn();
        #endregion Spawning
    }

    private void InitializeForRespawn()
    {
        switch (type)
        {
            case PlayerType.mech:
                tankStyleMove = false;
                break;

            case PlayerType.drone:
                tankStyleMove = true;
                break;

            case PlayerType.tank:
                tankStyleMove = true;
                break;

            case PlayerType.core:
                tankStyleMove = true;
                break;

            default:
                break;
        }

        acceptingInputs = true;
        waitingForRespawn = false;
        triggerLockout = false;
        movespeed = stats.speed;
        lowerbodyRotateSpeed = stats.lowerbodyRotationSpeed;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(upperBody.transform.position, upperBody.transform.TransformDirection(new Vector3(0, 0, 5.0f)), new Color(255, 0, 0, 255), 0.0f);
    }

    private void FixedUpdate()
    {
        if (invulnerabilityTime > 0)
        {
            invulnerabilityTime -= Time.deltaTime;
            if (invulnerabilityTime <= 0)
            {
                invulnerabilityTime = 0;
            }
            // activate visual indication of invulnerability
        }

        if (triggerLockout)
        {
            StartCoroutine(Lockout(bankLockoutTime));
        }

        mousePos = Input.mousePosition;
        fraction = fraction + Time.deltaTime * 9;

        if (photonView.isMine)
        {
            if (acceptingInputs)
            {
                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    Camera.main.orthographicSize--;
                }
                else if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    Camera.main.orthographicSize++;
                }
                playerPositionOnScreen = Camera.main.WorldToScreenPoint(transform.position);
                mousePlayerDiff = playerPositionOnScreen - mousePos;
                ControlKBAM();
            }
        }
        else
        {
            transform.localPosition = Vector3.Lerp(onUpdatePos, latestCorrectPos, fraction);    // set our pos between A and B
        }

        CheckHealth();
    }

    private void OnGUI()
    {
        if (networkPlayer.isLocal)
        {
            //if (gun[activeAbility].reloading)
            //{
            //    GUI.Box(new Rect(Screen.width / 2, Screen.height / 2 + 100, 100, 20), "RELOADING");
            //}

            //GUI.Box(new Rect(0, 0, 100, 80),
            //    "Kill Tokens" + System.Environment.NewLine +
            //    killTokens.ToString() + System.Environment.NewLine
            //    );
            //GUI.Box(new Rect(0, 60, 100, 40), "Boost" + System.Environment.NewLine + boostTime.ToString("0.00"));

            if (triggerLockout)
            {
                GUI.Box(new Rect(Screen.width / 2 - 80, Screen.height / 2, 160, 20), "BANKING TOKENS");
            }
        }
    }

    private void OnPhotonInstantiate(PhotonMessageInfo msg)
    {
        networkPlayer = msg.sender;
        name += msg.sender.name;
        GameManager.Instance.players.Add(this);
    }

    [RPC]
    public void Setup(PhotonPlayer netPlayer, int tm)
    {
        Team playerTeam = (Team)tm;
        networkPlayer = netPlayer;
        team = playerTeam;

        if (networkPlayer == PhotonNetwork.player)
        {
            GameManager.Instance.localPlayer = this;
            GameObject newPlayerCameraObject = (GameObject)Instantiate(Resources.Load(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.PlayerCamera]));
            newPlayerCameraObject.transform.parent = transform;
            newPlayerCameraObject.GetComponent<KBCamera>().attachedPlayer = this;
            Camera.SetupCurrent(newPlayerCameraObject.GetComponent<Camera>());
        }
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
            int mxhlth = maxHealth;
            bool wtngFrRspwn = waitingForRespawn;
            int tm = (int)team;
            float invltime = invulnerabilityTime;

            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
            stream.Serialize(ref hlth);
            stream.Serialize(ref mxhlth);
            stream.Serialize(ref wtngFrRspwn);
            stream.Serialize(ref tm);
            stream.Serialize(ref invltime);
        }
        else
        {
            // Receive latest state information
            Vector3 pos = Vector3.zero;
            Quaternion rot = Quaternion.identity;
            int hlth = 0;
            int mxhlth = 0;
            bool wtngFrRspwn = false;
            int tm = 0;
            float invltime = 0.0f;

            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
            stream.Serialize(ref hlth);
            stream.Serialize(ref mxhlth);
            stream.Serialize(ref wtngFrRspwn);
            stream.Serialize(ref tm);
            stream.Serialize(ref invltime);

            latestCorrectPos = pos;                 // save this to move towards it in FixedUpdate()
            onUpdatePos = transform.localPosition;  // we interpolate from here to latestCorrectPos
            fraction = 0;                           // reset the fraction we alreay moved. see Update()

            upperBody.transform.rotation = rot;          // this sample doesn't smooth rotation
            health = hlth;
            maxHealth = mxhlth;
            waitingForRespawn = wtngFrRspwn;
            team = (Team)tm;
            invulnerabilityTime = invltime;
        }
    }

    private new void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.gameObject.CompareTag("BankZone"))
        {
            if (killTokens > 0)
            {
                BankZone b = other.gameObject.GetComponent<BankZone>();
                b.AddPoints(killTokens, team);
                killTokens = 0;
            }
        }


        if (other.gameObject.CompareTag("SpawnDrone") || other.gameObject.CompareTag("SpawnMech") || other.gameObject.CompareTag("SpawnTank"))
        {
            photonView.RPC("SwitchType", PhotonTargets.AllBuffered, other.gameObject.tag.ToString());
        }
    }

    private void ControlKBAM()
    {
        float speed = 0;
        float modifiedMoveSpeed = 0;
        modifiedMoveSpeed = movespeed;

        switch (controlStyle)
        {
            case ControlStyle.ThirdPerson:
                float d = modifiedMoveSpeed * Input.GetAxis("Vertical");
                charController.SimpleMove(transform.TransformDirection(Vector3.forward) * d);
                transform.Rotate(0, Input.GetAxis("Horizontal") * lowerbodyRotateSpeed, 0);
                break;

            case ControlStyle.TopDown:

                Vector3 m = Vector3.zero;
                if (tankStyleMove)
                {
                    m = Input.GetAxis("Vertical") * lowerBody.transform.forward;
                    speed = m.normalized.magnitude * modifiedMoveSpeed;
                    charController.SimpleMove(m.normalized * modifiedMoveSpeed);
                    lowerBody.transform.Rotate(Vector3.up, Input.GetAxis("Horizontal") * lowerbodyRotateSpeed * Time.deltaTime);

                    if (type == PlayerType.tank)
                    {
                        Quaternion newRot = Quaternion.LookRotation(upperBody.transform.position + new Vector3(-mousePlayerDiff.x, 0, -mousePlayerDiff.y));
                        upperBody.transform.rotation = Quaternion.Lerp(upperBody.transform.rotation, newRot, upperbodyRotateSpeed * Time.deltaTime);
                    }
                    else // Currently Drone case only
                    {
                        upperBody.transform.rotation = lowerBody.transform.rotation;
                    }
                }
                else
                {
                    Quaternion newRot = Quaternion.LookRotation(upperBody.transform.position + new Vector3(-mousePlayerDiff.x, 0, -mousePlayerDiff.y));
                    upperBody.transform.rotation = Quaternion.Lerp(upperBody.transform.rotation, newRot, upperbodyRotateSpeed * Time.deltaTime);
                    m = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                    speed = m.normalized.magnitude * modifiedMoveSpeed;
                    Quaternion bottomRotation = Quaternion.LookRotation(m.normalized);
                    lowerBody.transform.rotation = Quaternion.Lerp(lowerBody.transform.rotation, bottomRotation, 5f * Time.deltaTime);
                    charController.SimpleMove(m.normalized * modifiedMoveSpeed);
                }
                break;

            default:
                break;
        }

        if (gun.GetLength(0) > 0)
        {
            if ((Input.GetMouseButton(0) || Input.GetMouseButton(1)))// && !gun[activeAbility].reloading)
            {
                if (Input.GetMouseButton(0))
                {
                    if (gun[0].ammo <= 0 || gun[1].ammo <= 0)
                    {
                        int[] guns = { 0, 1 };
                        photonView.RPC("Reload", PhotonTargets.All, guns);
                        
                    }
                    else
                    {
                        if (primaryWeaponLinkedFire)
                        {
                        int[] guns = { 0,1};
                        float[] speeds = { speed,speed};
                        photonView.RPC("Fire", PhotonTargets.All, guns, speeds);
                        //gun[0].PlayerFire(speed);
                        //gun[1].PlayerFire(speed);
                        }
                        else
                        {
                            int otherGun;
                            if (lastPrimaryFire == 0)
                            {
                                otherGun = 1;
                            }
                            else
                            {
                                otherGun = 0;
                            }

                            if (gun[lastPrimaryFire].available)
                            {
                            //Debug.Log(gun[lastPrimaryFire].cooldown.ToString());
                            int[] guns = { lastPrimaryFire };
                            float[] speeds = { speed };
                            photonView.RPC("Fire", PhotonTargets.All, guns, speeds);
                            //gun[lastPrimaryFire].PlayerFire(speed);
                            }
                            else if (!gun[lastPrimaryFire].available && gun[lastPrimaryFire].halfwayCooled)
                            {
                            
                                lastPrimaryFire = otherGun;
                            int[] guns = { otherGun };
                            float[] speeds = { speed };
                            photonView.RPC("Fire", PhotonTargets.All, guns, speeds);
                            //gun[otherGun].PlayerFire(speed);
                            
                            }
                        }
                    }
                }
                if (Input.GetMouseButton(1))
                {
                    if (gun[2].ammo <= 0 || gun[3].ammo <= 0)
                    {
                        gun[2].ammo = 0;
                        gun[3].ammo = 0;

                        int[] guns = { 2, 3 };
                        photonView.RPC("Reload", PhotonTargets.All, guns);
                    }
                    else
                    {

                        if (secondaryWeaponLinkedFire)
                        {
                        int[] guns = { 2, 3 };
                        float[] speeds = { speed, speed };
                        photonView.RPC("Fire", PhotonTargets.All, guns, speeds);
                        //gun[2].PlayerFire(speed);
                        //gun[3].PlayerFire(speed);
                        }
                        else
                        {
                            int otherGun;
                            if (lastSecondaryFire == 2)
                            {
                                otherGun = 3;
                            }
                            else
                            {
                                otherGun = 2;
                            }
                            if (gun[lastSecondaryFire].available) // TODO This doesn't synchronize as intended. Can shoot same side twice if you let go and wait until it cools.
                            {
                            int[] guns = { lastSecondaryFire };
                            float[] speeds = { speed };
                            photonView.RPC("Fire", PhotonTargets.All, guns, speeds);
                            //gun[lastSecondaryFire].PlayerFire(speed);
                            }
                            else if (!gun[lastSecondaryFire].available && gun[lastSecondaryFire].halfwayCooled)
                            {
                                lastSecondaryFire = otherGun;
                            int[] guns = { otherGun };
                            float[] speeds = { speed };
                            photonView.RPC("Fire", PhotonTargets.All, guns, speeds);
                            //gun[otherGun].PlayerFire(speed);
                            
                            }
                        }
                    }
                }
            }
            } 

        

        if (Input.GetKeyDown(KeyCode.Z))
        {
            secondaryWeaponLinkedFire = !secondaryWeaponLinkedFire;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            primaryWeaponLinkedFire = !primaryWeaponLinkedFire;
        }

        // DEBUG FUNCTIONS
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(100);
        }

        //if (Input.GetKeyDown(KeyCode.Return))
        //{
        //    BankKills();
        //}
    }

    [RPC]
    private void Fire(int[] gunIndex, float[] speed)
    {
        for (int i = 0; i < gunIndex.Length; i++)
        {
            int rightGun = gunIndex[i];
            float rightSpeed = speed[i];
            gun[rightGun].PlayerFire(rightSpeed);
        }
        
    }

    [RPC]
    private void Reload(int[] gunIndex)
    {
        for (int i = 0; i < gunIndex.Length; i++)
        {
            int rightGun = gunIndex[i];
            gun[rightGun].ammo = 0;
            gun[rightGun].PlayerTriggerReload();
        }

    }

    private void CheckHealth()
    {
        if (health <= 0 && !waitingForRespawn) // should respawn
        {
            respawnTimer = timer.StartTimer(respawnTime);
            waitingForRespawn = true;
            audio.PlayOneShot(deadSound);
        }
        else if (waitingForRespawn && photonView.isMine)
        {
            acceptingInputs = false;
            if (!timer.IsTimerActive(respawnTimer))
            {
                Debug.Log("Respawning!");
                RespawnToPrespawn();
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

    public void ConfirmHit(KBPlayer victimObject)
    {
        photonView.RPC("ConfirmHitToOthers", PhotonTargets.All, victimObject.networkPlayer);
        //MIGHT BE ABLE TO REMOVE THIS IF WE DONT NOTICE A TIMING DIFFERENCE IN HITS LOCALLY VS RPC TO ALL
        //Instantiate(hitExplosion, victimObject.transform.position, Quaternion.identity);
        //gun[activeAbility].audio.PlayOneShot(hitConfirm);
    }

    [RPC]
    public void ConfirmHitToOthers(PhotonPlayer hitPhotonPlayer)
    {
        List<KBPlayer> currentPlayers = GameManager.Instance.players;

        for (int i = 0; i < currentPlayers.Count; i++)
        {
            if (currentPlayers[i].networkPlayer == hitPhotonPlayer)
            {
                Instantiate(hitExplosion, currentPlayers[i].transform.position, Quaternion.identity);
                gun[0].audio.PlayOneShot(hitConfirm);
            }
        }
    }

    public override int TakeDamage(int amount)
    {
        if (photonView.isMine)
        {
            if (invulnerabilityTime <= 0)
            {
                health -= amount;
                Instantiate(hitExplosion, transform.position, Quaternion.identity);
                Camera.main.GetComponent<ScreenShake>().StartShake(0.25f, 5.0f);
                audio.PlayOneShot(gotHitSFX[Random.Range(0, gotHitSFX.Length)]);
            }
            return health;
        }
        else
        {
            return health;
        }
    }

    public void Die(GameObject killerObject)
    {
        KBPlayer killerPlayer = killerObject.GetComponent<KBPlayer>();
        killerPlayer.photonView.RPC("NotifyKill", PhotonTargets.Others, killerPlayer.networkPlayer);
    }

    [RPC]
    public void NotifyKill(PhotonPlayer killerPlayer)
    {
        if (killerPlayer.isLocal && networkPlayer == PhotonNetwork.player)
        {
            killTokens++;
        }
    }

    /// <summary>
    /// Spawns the player in the dropship (type select area)
    /// </summary>
    private void RespawnToPrespawn()
    {
        killTokens = 0;
        waitingForRespawn = false;
        acceptingInputs = true;

        if (team == Team.Blue)
        {
            GameManager.Instance.CreateObject((int)ObjectConstants.type.KillTagBlue, transform.position, Quaternion.identity, (int)team);
        }
        else if (team == Team.Red)
        {
            GameManager.Instance.CreateObject((int)ObjectConstants.type.KillTagRed, transform.position, Quaternion.identity, (int)team);
        }

        Camera.main.GetComponent<ScreenShake>().StopShake();
        transform.position = GameObject.FindGameObjectWithTag("Prespawn").transform.position;
        health = 1000;
    }

    /// <summary>
    /// Spawns the player in the combat area.
    /// </summary>
    private void Spawn()
    {
        if (teamSpawnpoints.Count > 0 && photonView.isMine)
        {
            transform.position = teamSpawnpoints[0].transform.position;
            waitingForRespawn = false;
            acceptingInputs = true;
            health = stats.health;
            maxHealth = health;
            movespeed = stats.speed;
            invulnerabilityTime = spawnProtectionTime;
            lowerbodyRotateSpeed = stats.lowerbodyRotationSpeed;
            upperbodyRotateSpeed = stats.upperbodyRotationSpeed;

            Camera.main.GetComponent<ScreenShake>().StopShake();
            audio.PlayOneShot(respawnSound);
        }
    }

    private void SetupAbilities()
    {
        gun = upperBody.GetComponentsInChildren<ProjectileAbilityBaseScript>();
        for (int i = 0; i < gun.Length; i++)
        {
            gun[i].owner = this;
            gun[i].Team = team;
        }
        lastPrimaryFire = 0;
        lastSecondaryFire = 2;
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

    private void FireWeapons(int weaponOne, int weaponTwo, bool linked)
    {
    }

    [RPC]
    private void SwitchType(string typeTrigger, PhotonMessageInfo info)
    {
        if (networkPlayer == info.sender)
        {
            for (int i = 0; i < allBodies.Count; i++)
            {
                allBodies[i].SetActive(false);
            }

            switch (typeTrigger)
            {
                case "SpawnDrone":

                    upperBody = upperBodyDrone;
                    lowerBody = lowerBodyDrone;
                    //hitbox = hitboxDrone;
                    type = PlayerType.drone;

                    break;
                case "SpawnMech":

                    upperBody = upperBodyMech;
                    lowerBody = lowerBodyMech;
                    //hitbox = hitboxMech;
                    type = PlayerType.mech;

                    break;
                case "SpawnTank":

                    upperBody = upperBodyTank;
                    lowerBody = lowerBodyTank;
                    //hitbox = hitboxTank;
                    type = PlayerType.tank;

                    break;

                case "SpawnCore":

                    upperBody = upperBodyCore;
                    lowerBody = lowerBodyCore;
                    type = PlayerType.core;
                    break;

                default:
                    break;
            }
            upperBody.SetActive(true);
            lowerBody.SetActive(true);
            //hitbox.SetActive(true);

            if (team == KBConstants.Team.Blue)
            {
                SetActiveIfFound(this.transform,"BlueBody", true);
                SetActiveIfFound(this.transform, "RedBody", false);
            }
            else
            {
                SetActiveIfFound(this.transform,"BlueBody", false);
                SetActiveIfFound(this.transform,"RedBody", true);
            }


            //Switch Stats
            GameManager.Instance.SetPlayerStats(networkPlayer);
            InitializeForRespawn();
            SetupAbilities();

            
            Spawn();
        }
    }

    private void SetActiveIfFound(Transform trans, string objectName, bool val)
    {
        GameObject foundObject = SetActiveChildIfFound(trans, objectName, val);

        if (foundObject != null)
        {
            foundObject.SetActive(val);
        }
        else
        {
            Debug.LogWarning("[Player] Couldn't find player model");
        }
    }


    private GameObject SetActiveChildIfFound(Transform cTranform,string objectName, bool val)
    {
        GameObject foundGameObject = null;
        
        if(cTranform.gameObject.name.Equals(objectName))
        {
            return cTranform.gameObject;
        }
        else
        {
            for (int i = 0; i < cTranform.childCount; i++)
            {
                Transform t = cTranform.GetChild(i);
                foundGameObject = SetActiveChildIfFound(t, objectName, val);
                if (foundGameObject != null)
                {
                    break;
                }
            }
        }

        return foundGameObject;
    }
}