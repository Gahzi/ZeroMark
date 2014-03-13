﻿using KBConstants;
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

    public AudioClip grabSound;
    public GameObject upperBody;
    public GameObject lowerBody;
    public Vector3 mousePos;
    public Vector3 playerPositionOnScreen;
    public Vector3 mousePlayerDiff;

    public List<PlayerSpawnPoint> teamSpawnpoints;
    public float respawnTime;
    private int respawnTimer;
    public int upgradePoints;
    public int maxHealth;
    public int[] pointToLevel = new int[4];

    public Material redMat;
    public Material blueMat;
    public MeshRenderer teamIndicator;
    private AudioClip hitConfirm;
    private float hitFXTimer;
    public GameObject hitExplosion;
    public AudioClip gotHitSFX;
    public AudioClip deadSound;
    public AudioClip respawnSound;
    public AudioClip dropSound;

    public ProjectileAbilityBaseScript[] gun;
    public int killTokens;
    private bool triggerLockout;
    private int activeAbility;

    //private bool autoFire;
    private bool isShooting;

    private bool secondaryWeaponLinkedFire;
    private int lastPrimaryFire;
    private bool primaryWeaponLinkedFire;
    private int lastSecondaryFire;

    public override void Start()
    {
        base.Start();
        acceptingInputs = true;
        waitingForRespawn = false;
        triggerLockout = false;
        charController = GetComponent<CharacterController>();
        grabSound = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.ItemGrab]);
        latestCorrectPos = transform.position;
        onUpdatePos = transform.position;
        isShooting = false;
        hitConfirm = Resources.Load<AudioClip>(KBConstants.AudioConstants.CLIP_NAMES[KBConstants.AudioConstants.clip.HitConfirm]);
        SetupAbilities();
        GetComponentInChildren<HitboxBaseScript>().Team = team;
        //autoFire = false;
        movespeed = stats.speed;
        lowerbodyRotateSpeed = stats.lowerbodyRotationSpeed;

        lastPrimaryFire = 0;
        lastSecondaryFire = 2;

        int itemLayer = 8;
        int towerLayer = 13;
        int modelLayer = 15;
        int layerMask1 = 1 << itemLayer;
        int layerMask2 = 1 << towerLayer;
        int layerMask3 = 1 << modelLayer;
        layerMask = layerMask1 | layerMask2 | layerMask3;

        teamSpawnpoints = new List<PlayerSpawnPoint>();

        switch (team)
        {
            case Team.Red:
                {
                    //teamIndicator.material = redMat;
                    FindTeamSpawnpoints();
                    if (teamSpawnpoints.Count > 0)
                    {
                        Respawn();
                    }
                    break;
                }

            case Team.Blue:
                {
                    //teamIndicator.material = blueMat;
                    FindTeamSpawnpoints();
                    if (teamSpawnpoints.Count > 0)
                    {
                        Respawn();
                    }
                    break;
                }

            case Team.None:
                {
                    enabled = false;
                    break;
                }

            default:
                break;
        }

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
        Screen.showCursor = false;

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
        
                playerPositionOnScreen = Camera.main.WorldToScreenPoint(transform.position);
                mousePlayerDiff = playerPositionOnScreen - mousePos;
                ControlKBAM();

                if (isShooting)
                {
                    //gun[activeAbility].PlayerFire();
                }

                for (int i = 0; i < gun.Length; i++)
                {
                    if (gun[i].ammo == 0)
                    {
                        gun[i].PlayerTriggerReload();
                    }
                }

                if (!Input.GetButton("Boost"))
                {
                    boostTime += boostRechargeRate * Time.deltaTime;
                    boostTime = Mathf.Clamp(boostTime, 0.0f, boostMax);
                }
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
        if (photonView.isMine)
        {
            if (gun[activeAbility].reloading)
            {
                GUI.Box(new Rect(Screen.width / 2, Screen.height / 2 + 100, 100, 20), "RELOADING");
            }

            GUI.Box(new Rect(0, 0, 100, 80),
                "Kill Tokens" + System.Environment.NewLine +
                killTokens.ToString() + System.Environment.NewLine
                );
            GUI.Box(new Rect(0, 60, 100, 40), "Boost" + System.Environment.NewLine + boostTime.ToString("0.00"));

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
            int lvl = level;
            int mxhlth = maxHealth;
            bool isShting = isShooting;
            bool wtngFrRspwn = waitingForRespawn;
            int tm = (int)team;
            int ab = activeAbility;
            float invltime = invulnerabilityTime;

            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
            stream.Serialize(ref hlth);
            stream.Serialize(ref lvl);
            stream.Serialize(ref mxhlth);
            stream.Serialize(ref isShting);
            stream.Serialize(ref wtngFrRspwn);
            stream.Serialize(ref tm);
            stream.Serialize(ref ab);
            stream.Serialize(ref invltime);
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
            bool wtngFrRspwn = false;
            int tm = 0;
            int ab = 0;
            float invltime = 0.0f;

            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
            stream.Serialize(ref hlth);
            stream.Serialize(ref lvl);
            stream.Serialize(ref mxhlth);
            stream.Serialize(ref isShting);
            stream.Serialize(ref wtngFrRspwn);
            stream.Serialize(ref tm);
            stream.Serialize(ref ab);
            stream.Serialize(ref invltime);

            latestCorrectPos = pos;                 // save this to move towards it in FixedUpdate()
            onUpdatePos = transform.localPosition;  // we interpolate from here to latestCorrectPos
            fraction = 0;                           // reset the fraction we alreay moved. see Update()

            upperBody.transform.rotation = rot;          // this sample doesn't smooth rotation
            health = hlth;
            level = lvl;
            maxHealth = mxhlth;
            isShooting = isShting;
            waitingForRespawn = wtngFrRspwn;
            team = (Team)tm;
            activeAbility = ab;
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
        
    }

    private void ControlKBAM()
    {
        float modifiedMoveSpeed = 0;
        if (Input.GetButton("Boost") && boostTime == boostMax)
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

                Vector3 m = Vector3.zero;
                if (tankStyleMove)
                {
                    m = Input.GetAxis("Vertical") * lowerBody.transform.forward;
                    charController.SimpleMove(m.normalized * modifiedMoveSpeed);
                    lowerBody.transform.Rotate(Vector3.up, Input.GetAxis("Horizontal") * lowerbodyRotateSpeed * Time.deltaTime);

                    if (type == PlayerType.tank)
                    {
                        Quaternion newRot = Quaternion.LookRotation(upperBody.transform.position + new Vector3(-mousePlayerDiff.x, 0, -mousePlayerDiff.y));
                        upperBody.transform.rotation = Quaternion.Lerp(upperBody.transform.rotation, newRot, upperbodyRotateSpeed * Time.deltaTime);
                    }
                    else
                    {
                        upperBody.transform.rotation = lowerBody.transform.rotation;
                    }
                }
                else
                {
                    Quaternion newRot = Quaternion.LookRotation(upperBody.transform.position + new Vector3(-mousePlayerDiff.x, 0, -mousePlayerDiff.y));
                    upperBody.transform.rotation = Quaternion.Lerp(upperBody.transform.rotation, newRot, upperbodyRotateSpeed * Time.deltaTime);
                    lowerBody.transform.rotation = upperBody.transform.rotation;
                    m = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                    charController.SimpleMove(m.normalized * modifiedMoveSpeed);
                }
                break;

            default:
                break;
        }

        //if (autoFire)
        //{
        if ((Input.GetMouseButton(0) || Input.GetMouseButton(1)))// && !gun[activeAbility].reloading)
        {
            isShooting = true;
            if (Input.GetMouseButton(0))
            {
                if (primaryWeaponLinkedFire)
                {
                    gun[0].PlayerFire();
                    gun[1].PlayerFire();
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
                        gun[lastPrimaryFire].PlayerFire();
                    }
                    else if (!gun[lastPrimaryFire].available && gun[lastPrimaryFire].halfwayCooled)
                    {
                        gun[otherGun].PlayerFire();
                        lastPrimaryFire = otherGun;
                    }
                }
            }
            if (Input.GetMouseButton(1))
            {
                if (secondaryWeaponLinkedFire)
                {
                    gun[2].PlayerFire();
                    gun[3].PlayerFire();
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
                        gun[lastSecondaryFire].PlayerFire();
                    }
                    else if (!gun[lastSecondaryFire].available && gun[lastSecondaryFire].halfwayCooled)
                    {
                        gun[otherGun].PlayerFire();
                        lastSecondaryFire = otherGun;
                    }
                }
            }
        }
        else
        {
            isShooting = false;
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
            TakeDamage(1);
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
                gun[activeAbility].audio.PlayOneShot(hitConfirm);
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
                audio.PlayOneShot(gotHitSFX);
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

    private void Respawn()
    {
        if (teamSpawnpoints.Count > 0 && photonView.isMine)
        {
            if (team == Team.Blue)
            {
                GameManager.Instance.CreateObject((int)ObjectConstants.type.KillTagBlue, transform.position, Quaternion.identity, (int)team);
            }
            else if (team == Team.Red)
            {
                GameManager.Instance.CreateObject((int)ObjectConstants.type.KillTagRed, transform.position, Quaternion.identity, (int)team);
            }
            
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

            Camera.main.GetComponent<ScreenShake>().StopShake();
            audio.PlayOneShot(respawnSound);
        }
    }

    private void SetupAbilities()
    {
        gun = gameObject.GetComponentsInChildren<ProjectileAbilityBaseScript>();
        for (int i = 0; i < gun.Length; i++)
        {
            gun[i].owner = this;
            gun[i].Team = team;
        }
        activeAbility = 0;
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
}