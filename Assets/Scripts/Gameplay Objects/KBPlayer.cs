using KBConstants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(CharacterController))]
public class KBPlayer : KBControllableGameObject
{
    #region CONSTANTS

    #region DRONE

    private static readonly int droneLowerRotationSpeed = 5;
    private static readonly int droneUpperRotationSpeed = 100;
    private static readonly int droneMovementSpeed = 35;
    private static readonly int droneBaseHealth = 150;
    private static readonly float droneAccel = 0.04f;
    private static readonly float dronePowerDecel = 0.25f;
    private static readonly float droneFriction = 0.05f;
    private static readonly float droneReverseSpeedFraction = 1.0f;
    private static readonly float droneRegenRate = 1.0f;

    #endregion DRONE

    #region MECH

    private static readonly int mechLowerRotationSpeed = 10;
    private static readonly int mechUpperRotationSpeed = 50;
    private static readonly int mechMovementSpeed = 25;
    private static readonly int mechBaseHealth = 450;
    private static readonly float mechRegenRate = 1.0f;

    #endregion MECH

    #region TANK

    private static readonly int tankLowerRotationSpeed = 200;
    private static readonly int tankUpperRotationSpeed = 45;
    private static readonly int tankMovementSpeed = 15;
    private static readonly int tankBaseHealth = 1600;
    private static readonly float tankAccel = 0.060f;
    private static readonly float tankPowerDecel = 0.150f;
    private static readonly float tankFriction = 0.010f;
    private static readonly float tankReverseSpeedFraction = 0.00f;
    private static readonly float tankRegenRate = 4.0f;

    #endregion TANK

    #region CORE

    private static readonly int coreLowerRotationSpeed = 140;
    private static readonly int coreUpperRotationSpeed = 140;
    private static readonly int coreMovementSpeed = 40;
    private static readonly int coreBaseHealth = 10000;
    private static readonly float coreAccel = 0.25f;
    private static readonly float corePowerDecel = 0.15f;
    private static readonly float coreFriction = 0.8f;
    private static readonly float coreReverseSpeedFraction = 0.35f;

    #endregion CORE

    #endregion CONSTANTS

    #region GamepadParameters

    public bool useController;
    private bool playerIndexSet = false;
    public PlayerIndex playerIndex;
    public GamePadState gamepadState;
    public GamePadState gamepadPrevState;
    public float stickDeadzone;

    #endregion GamepadParameters

    private bool tankStyleMove;
    public PlayerType type;
    public PlayerStats stats;
    public bool acceptingInputs = true;
    public bool waitingForRespawn = false;
    public float invulnerabilityTime;
    public float spawnProtectionTime;
    public float bankLockoutTime;
    public float teleportationRecharge = 5.0f;
    public GameObject spawnAnimator;
    public float spawnDelay = 2.50f;
    public GameObject invulnerabilityShield;
    private float timeInBankZone;
    private bool inBankZone;
    public GameObject bankIndicator;

    public TimerScript timer;
    private float movespeed;
    private float lowerbodyRotateSpeed;
    private float upperbodyRotateSpeed;
    private float collisionStopMod = 1.0f;

    private CharacterController charController;

    public string playerName;
    public PhotonPlayer networkPlayer;

    private Vector3 latestCorrectPos;
    private Vector3 onUpdatePos;
    private float fraction;

    public AudioClip itemPickupClip;
    public GameObject upperBody;
    public GameObject lowerBody;
    public PlayerGibModel mechGibBody;
    public PlayerGibModel droneGibBody;
    public PlayerGibModel tankGibBody;

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
    public float regenDelay;
    private float lastDamageTime;

    public Material redMat;
    public Material blueMat;

    private AudioClip hitConfirm;

    public HitFX hitExplosion;
    public AudioClip[] gotHitSFX;
    public AudioClip deadSound;
    public AudioClip respawnSound;
    public AudioClip dropSound;

    public int killCount;
    public int deathCount;
    public int killTokens;
    public int totalTokensGained;
    public int totalTokensLost;
    public int totalTokensBanked;

    public ProjectileAbilityBaseScript[] guns;

    private float forwardAccel;

    private new KBCamera camera;

    public GameObject ammoHud;

    public Chaff chaff;

    public void SetStats()
    {
        stats = new PlayerStats();

        switch (type)
        {
            case PlayerType.mech:
                stats.health = mechBaseHealth;
                stats.lowerbodyRotationSpeed = mechLowerRotationSpeed;
                stats.upperbodyRotationSpeed = mechUpperRotationSpeed;
                stats.speed = mechMovementSpeed;
                stats.regerationRate = mechRegenRate;
                break;

            case PlayerType.drone:
                stats.health = droneBaseHealth;
                stats.lowerbodyRotationSpeed = droneLowerRotationSpeed;
                stats.upperbodyRotationSpeed = droneUpperRotationSpeed;
                stats.speed = droneMovementSpeed;
                stats.regerationRate = droneRegenRate;
                break;

            case PlayerType.tank:
                stats.health = tankBaseHealth;
                stats.lowerbodyRotationSpeed = tankLowerRotationSpeed;
                stats.upperbodyRotationSpeed = tankUpperRotationSpeed;
                stats.speed = tankMovementSpeed;
                stats.regerationRate = tankRegenRate;
                break;

            case PlayerType.core:
                stats.health = coreBaseHealth;
                stats.lowerbodyRotationSpeed = coreLowerRotationSpeed;
                stats.upperbodyRotationSpeed = coreUpperRotationSpeed;
                stats.speed = coreMovementSpeed;
                break;

            default:
                break;
        }
    }

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
    }

    public override void Start()
    {
        base.Start();

        camera = Camera.main.GetComponent<KBCamera>();

        #region Resource & reference loading

        ObjectPool.CreatePool(hitExplosion);
        charController = GetComponent<CharacterController>();
        itemPickupClip = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.ItemPickup01]);
        hitConfirm = Resources.Load<AudioClip>(KBConstants.AudioConstants.CLIP_NAMES[KBConstants.AudioConstants.clip.HitConfirm]);

        teamSpawnpoints = new List<PlayerSpawnPoint>();
        SetTeam(team);

        #endregion Resource & reference loading

        latestCorrectPos = transform.position;
        onUpdatePos = transform.position;

        InitializeForRespawn();
        RespawnToPrespawn();
        ObjectPool.CreatePool(chaff);
        ObjectPool.CreatePool(mechGibBody);
    }

    private void InitializeForRespawn()
    {
        switch (type)
        {
            case PlayerType.mech:
                tankStyleMove = false;
                break;

            case PlayerType.drone:
                tankStyleMove = false;
                break;

            case PlayerType.tank:
                tankStyleMove = true;
                break;

            case PlayerType.core:
                tankStyleMove = false;
                break;

            default:
                break;
        }

        acceptingInputs = true;
        waitingForRespawn = false;
        movespeed = stats.speed;
        lowerbodyRotateSpeed = stats.lowerbodyRotationSpeed;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(upperBody.transform.position, upperBody.transform.TransformDirection(new Vector3(0, 0, 5.0f)), new Color(255, 0, 0, 255), 0.0f);
    }

    private void FixedUpdate()
    {
        if (inBankZone)
        {
            timeInBankZone += Time.deltaTime;
        }
        if (timeInBankZone > 3.0f && killTokens > 0 && inBankZone)
        {
            int tokensToBank = 0;
            if (killTokens > GameConstants.levelTwoThreshold)
            {
                tokensToBank = killTokens - GameConstants.levelTwoThreshold;
            }
            else if (killTokens > GameConstants.levelOneThreshold)
            {
                tokensToBank = killTokens - GameConstants.levelOneThreshold;
            }
            else
            {
                tokensToBank = killTokens;
            }

            for (int i = 0; i < GameManager.Instance.bankZones.Count; i++)
            {
                GameManager.Instance.bankZones[i].photonView.RPC("AddPoints", PhotonTargets.AllBuffered, tokensToBank, (int)team);
            }
            killTokens -= tokensToBank;
            totalTokensBanked += tokensToBank;
            timeInBankZone = 0.0f;
        }

        if (Time.time > lastDamageTime + regenDelay)
        {
            float floatHealth = Mathf.MoveTowards(health, stats.health, stats.regerationRate);
            health = Mathf.FloorToInt(floatHealth);
        }

        if (teleportationRecharge > 0)
        {
            teleportationRecharge -= Time.deltaTime;
        }

        if (invulnerabilityTime > 0)
        {
            if (!invulnerabilityShield.activeInHierarchy)
            {
                invulnerabilityShield.SetActive(true);
            }
            invulnerabilityTime -= Time.deltaTime;
            if (invulnerabilityTime <= 0)
            {
                invulnerabilityTime = 0;
                invulnerabilityShield.SetActive(false);
            }
        }

        mousePos = Input.mousePosition;

        fraction = fraction + Time.deltaTime * 9;

        if (photonView.isMine)
        {
            if (acceptingInputs)
            {
                //if (Input.GetAxis("Mouse ScrollWheel") > 0 && camera.zoomTarget > 0.75f)
                //{
                //    camera.zoomTarget *= 1.0f / 1.05f;
                //}
                //else if (Input.GetAxis("Mouse ScrollWheel") < 0 && camera.zoomTarget < 2.0f)
                //{
                //    camera.zoomTarget *= 1.05f;
                //}
                playerPositionOnScreen = Camera.main.WorldToScreenPoint(transform.position);
                mousePlayerDiff = playerPositionOnScreen - mousePos;
                if (useController)
                {

                    if (!playerIndexSet || !gamepadPrevState.IsConnected)
                    {
                        for (int i = 0; i < 4; ++i)
                        {
                            PlayerIndex testPlayerIndex = (PlayerIndex)i;
                            GamePadState testState = GamePad.GetState(testPlayerIndex);
                            if (testState.IsConnected)
                            {
                                Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                                playerIndex = testPlayerIndex;
                                playerIndexSet = true;
                            }
                        }
                    }

                    gamepadPrevState = gamepadState;
                    gamepadState = GamePad.GetState(playerIndex);
                    ControlGamepad();
                }
                else
                {
                    ControlKBAM();
                }
            }
        }
        else
        {
            transform.localPosition = Vector3.Lerp(onUpdatePos, latestCorrectPos, fraction);    // set our pos between A and B
        }

        CheckHealth();

        // DEBUG FUNCTIONS
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(100);
        }
    }

    private void OnGUI()
    {
        //if (networkPlayer.isLocal)
        //{
        //    if (triggerLockout)
        //    {
        //        GUI.Box(new Rect(Screen.width / 2 - 80, Screen.height / 2, 160, 20), "BANKING TOKENS");
        //    }
        //}
    }

    private void OnPhotonInstantiate(PhotonMessageInfo msg)
    {
        networkPlayer = msg.sender;
        name += msg.sender.name;
        SetStats();
        GameManager.Instance.players.Add(this);
    }

    [RPC]
    public void Setup(PhotonPlayer netPlayer, int tm)
    {
        team = (Team)tm;
        networkPlayer = netPlayer;

        if (networkPlayer == PhotonNetwork.player)
        {
            GameManager.Instance.localPlayer = this;
            GameObject newPlayerCameraObject = (GameObject)Instantiate(Resources.Load(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.PlayerCamera]));
            newPlayerCameraObject.transform.parent = transform;
            newPlayerCameraObject.GetComponent<KBCamera>().attachedPlayer = this;
            Camera.SetupCurrent(newPlayerCameraObject.GetComponent<Camera>());

            name = PhotonNetwork.playerName;
        }
        else
        {
            SetActiveIfFound(transform, "Reticle", false);
            SetActiveIfFound(transform, "AmmoHud", false);
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
            int kt = killTokens;
            int kc = killCount;
            int dc = deathCount;
            int ttg = totalTokensGained;
            int ttl = totalTokensLost;
            int ttb = totalTokensBanked;
            int hlth = health;
            int mxhlth = stats.health;
            bool wtngFrRspwn = waitingForRespawn;
            int tm = (int)team;
            float invltime = invulnerabilityTime;

            int[] amms = new int[guns.Length];

            for (int i = 0; i < guns.Length; i++)
            {
                amms[i] = guns[i].ammo;
            }

            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
            stream.Serialize(ref kt);
            stream.Serialize(ref kc);
            stream.Serialize(ref dc);
            stream.Serialize(ref ttg);
            stream.Serialize(ref ttl);
            stream.Serialize(ref ttb);
            stream.Serialize(ref hlth);
            stream.Serialize(ref mxhlth);
            stream.Serialize(ref wtngFrRspwn);
            stream.Serialize(ref tm);
            stream.Serialize(ref invltime);

            for (int j = 0; j < amms.Length; j++)
            {
                stream.Serialize(ref amms[j]);
            }
        }
        else
        {
            // Receive latest state information
            Vector3 pos = Vector3.zero;
            Quaternion rot = Quaternion.identity;
            int kt = 0;
            int kc = 0;
            int dc = 0;
            int ttg = 0;
            int ttl = 0;
            int ttb = 0;
            int hlth = 0;
            int mxhlth = 0;
            bool wtngFrRspwn = false;
            int tm = 0;
            float invltime = 0.0f;
            int[] amms = new int[guns.Length];

            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
            stream.Serialize(ref kt);
            stream.Serialize(ref kc);
            stream.Serialize(ref dc);
            stream.Serialize(ref ttg);
            stream.Serialize(ref ttl);
            stream.Serialize(ref ttb);
            stream.Serialize(ref hlth);
            stream.Serialize(ref mxhlth);
            stream.Serialize(ref wtngFrRspwn);
            stream.Serialize(ref tm);
            stream.Serialize(ref invltime);

            for (int i = 0; i < amms.Length; i++)
            {
                stream.Serialize(ref amms[i]);
            }

            latestCorrectPos = pos;                 // save this to move towards it in FixedUpdate()
            onUpdatePos = transform.localPosition;  // we interpolate from here to latestCorrectPos
            fraction = 0;                           // reset the fraction we alreay moved. see Update()

            upperBody.transform.rotation = rot;          // this sample doesn't smooth rotation
            killTokens = kt;
            killCount = kc;
            deathCount = dc;
            totalTokensGained = ttg;
            totalTokensLost = ttl;
            totalTokensBanked = ttb;
            health = hlth;
            waitingForRespawn = wtngFrRspwn;
            team = (Team)tm;
            invulnerabilityTime = invltime;

            for (int j = 0; j < amms.Length; j++)
            {
                guns[j].ammo = amms[j];
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Environment"))
        {
            collisionStopMod = 0.00f;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Environment"))
        {
            collisionStopMod = 1.0f;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Environment"))
        {
            collisionStopMod = 0.0f;
        }
    }

    private new void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BankZone"))
        {
            inBankZone = true;
            bankIndicator.SetActive(true);
        }

        if (other.gameObject.CompareTag("Teleporter"))
        {
            Teleporter t = other.gameObject.GetComponent<Teleporter>();
            if (t != null && t.linkedTeleporter != null && teleportationRecharge <= 0)
            {
                Vector3 newPos = t.linkedTeleporter.transform.position;
                transform.position = new Vector3(newPos.x, newPos.y, newPos.z);
                teleportationRecharge = 5.0f;
            }
        }

        if (other.gameObject.CompareTag("SpawnRedDrone") || other.gameObject.CompareTag("SpawnRedMech") || other.gameObject.CompareTag("SpawnRedTank"))
        {
            SetTeam(Team.Red);
            StartCoroutine(Spawn(other.gameObject.tag.ToString()));
        }
        else if (other.gameObject.CompareTag("SpawnBlueDrone") || other.gameObject.CompareTag("SpawnBlueMech") || other.gameObject.CompareTag("SpawnBlueTank"))
        {
            SetTeam(Team.Blue);
            StartCoroutine(Spawn(other.gameObject.tag.ToString()));
        }
    }

    private void OnTriggerStay(Collider other)
    {
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("BankZone"))
        {
            inBankZone = false;
            timeInBankZone = 0.0f;
            bankIndicator.SetActive(false);
        }
    }

    private void ControlGamepad()
    {
        if (!Screen.lockCursor)
        {
            Screen.lockCursor = true;
        }
        if (Screen.showCursor)
        {
            Screen.showCursor = false;
        }

        #region Movement

        float modifiedMoveSpeed = 0;
        if (tankStyleMove)
        {
            float accel = 0;
            float decel = 0;
            float friction = 0;
            float reverseSpeed = 0;
            switch (type)
            {
                case PlayerType.drone:
                    accel = droneAccel;
                    decel = dronePowerDecel;
                    friction = droneFriction;
                    reverseSpeed = droneReverseSpeedFraction;
                    break;

                case PlayerType.tank:
                    accel = tankAccel;
                    decel = tankPowerDecel;
                    friction = tankFriction;
                    reverseSpeed = tankReverseSpeedFraction;
                    break;

                case PlayerType.core:
                    accel = coreAccel;
                    decel = corePowerDecel;
                    friction = coreFriction;
                    reverseSpeed = coreReverseSpeedFraction;
                    break;
            }

            // Movement
            if (gamepadState.ThumbSticks.Left.Y > stickDeadzone)
            {
                forwardAccel = Mathf.Lerp(forwardAccel, 1.0f, accel);
            }
            else if (gamepadState.ThumbSticks.Left.Y < stickDeadzone && gamepadState.ThumbSticks.Left.Y > -stickDeadzone)
            {
                forwardAccel = Mathf.Lerp(forwardAccel, 0.0f, friction);
            }
            else if (gamepadState.ThumbSticks.Left.Y < -stickDeadzone)
            {
                forwardAccel = Mathf.Lerp(forwardAccel, -reverseSpeed, decel);
            }
            modifiedMoveSpeed = movespeed * forwardAccel * collisionStopMod;
            charController.SimpleMove(lowerBody.transform.forward * modifiedMoveSpeed);
            if (collisionStopMod == 0)
            {
                forwardAccel = 0;
            }

            // Rotation
            lowerBody.transform.Rotate(Vector3.up, gamepadState.ThumbSticks.Left.X * lowerbodyRotateSpeed * Time.deltaTime);
            Quaternion newRot = Quaternion.LookRotation(new Vector3(gamepadState.ThumbSticks.Right.X, 0, gamepadState.ThumbSticks.Right.Y));
            upperBody.transform.rotation = Quaternion.Lerp(upperBody.transform.rotation, newRot, upperbodyRotateSpeed * Time.deltaTime);
        }
        else
        {
            // Movement
            Vector3 m = Vector3.zero;

            float x, y;

            if (gamepadState.ThumbSticks.Left.Y > stickDeadzone || gamepadState.ThumbSticks.Left.Y < -stickDeadzone)
            {
                y = gamepadState.ThumbSticks.Left.Y;
            }
            else
            {
                y = 0;
            }

            if (gamepadState.ThumbSticks.Left.X > stickDeadzone || gamepadState.ThumbSticks.Left.X < -stickDeadzone)
            {
                x = gamepadState.ThumbSticks.Left.X;
            }
            else
            {
                x = 0;
            }

            m = new Vector3(x, 0, y);
            modifiedMoveSpeed = movespeed;
            charController.SimpleMove(m.normalized * modifiedMoveSpeed);

            //Rotation
            Quaternion newRot = Quaternion.LookRotation(new Vector3(gamepadState.ThumbSticks.Right.X, 0, gamepadState.ThumbSticks.Right.Y));
            upperBody.transform.rotation = Quaternion.Lerp(upperBody.transform.rotation, newRot, upperbodyRotateSpeed * Time.deltaTime);
            if (m.normalized != Vector3.zero)
            {
                Quaternion bottomRotation = Quaternion.LookRotation(m.normalized);
                lowerBody.transform.rotation = Quaternion.Lerp(lowerBody.transform.rotation, bottomRotation, lowerbodyRotateSpeed * Time.deltaTime);
            }
        }

        #endregion Movement

        #region Weapons

        if (guns.Length > 0)
        {
            if ((gamepadState.Triggers.Left != 0 || gamepadState.Triggers.Right != 0))
            {
                if (gamepadState.Triggers.Left > 0)
                {
                    if (guns[0].ammo <= 0)
                    {
                        int[] reloadingGuns = { 0 };
                        photonView.RPC("Reload", PhotonTargets.All, reloadingGuns);
                    }
                    else
                    {
                        if (guns[0].available)
                        {
                            int[] shootingGuns = { 0 };
                            float[] speeds = { modifiedMoveSpeed };
                            photonView.RPC("Fire", PhotonTargets.All, shootingGuns, speeds);
                        }
                    }
                }
                if (gamepadState.Triggers.Right > 0)
                {
                    if (guns[1].ammo <= 0)
                    {
                        guns[1].ammo = 0;
                        int[] reloadingGuns = { 1 };
                        photonView.RPC("Reload", PhotonTargets.All, reloadingGuns);
                    }
                    else
                    {
                        if (guns[1].available)
                        {
                            int[] shootingGuns = { 1 };
                            float[] speeds = { modifiedMoveSpeed };
                            photonView.RPC("Fire", PhotonTargets.All, shootingGuns, speeds);
                        }
                    }
                }
            }

            if (gamepadState.Buttons.LeftShoulder == ButtonState.Pressed)
            {
                int[] reloadingGuns = { 0 };
                photonView.RPC("Reload", PhotonTargets.All, reloadingGuns);
            }

            if (gamepadState.Buttons.RightShoulder == ButtonState.Pressed)
            {
                int[] reloadingGuns = { 1 };
                photonView.RPC("Reload", PhotonTargets.All, reloadingGuns);
            }

        }



        if (gamepadState.Buttons.Start == ButtonState.Pressed)
        {
            if (GUIManager.Instance.state.Equals(GUIManager.GUIManagerState.Hidden))
            {
                GUIManager.Instance.state = GUIManager.GUIManagerState.ShowingStatTab;
            }
        }
        else
        {
            if (GUIManager.Instance.state.Equals(GUIManager.GUIManagerState.ShowingStatTab))
            {
                GUIManager.Instance.state = GUIManager.GUIManagerState.Hidden;
            }
        }

        #endregion Weapons
    }

    private void ControlKBAM()
    {
        #region Movement

        float modifiedMoveSpeed = 0;
        if (tankStyleMove)
        {
            float accel = 0;
            float decel = 0;
            float friction = 0;
            float reverseSpeed = 0;
            switch (type)
            {
                case PlayerType.drone:
                    accel = droneAccel;
                    decel = dronePowerDecel;
                    friction = droneFriction;
                    reverseSpeed = droneReverseSpeedFraction;
                    break;

                case PlayerType.tank:
                    accel = tankAccel;
                    decel = tankPowerDecel;
                    friction = tankFriction;
                    reverseSpeed = tankReverseSpeedFraction;
                    break;

                case PlayerType.core:
                    accel = coreAccel;
                    decel = corePowerDecel;
                    friction = coreFriction;
                    reverseSpeed = coreReverseSpeedFraction;
                    break;
            }

            // Movement
            if (Input.GetAxis("Vertical") > 0)
            {
                forwardAccel = Mathf.Lerp(forwardAccel, 1.0f, accel);
            }
            else if (Input.GetAxis("Vertical") == 0)
            {
                forwardAccel = Mathf.Lerp(forwardAccel, 0.0f, friction);
            }
            else if (Input.GetAxis("Vertical") < 0)
            {
                forwardAccel = Mathf.Lerp(forwardAccel, -reverseSpeed, decel);
            }
            modifiedMoveSpeed = movespeed * forwardAccel * collisionStopMod;
            charController.SimpleMove(lowerBody.transform.forward * modifiedMoveSpeed);
            if (collisionStopMod == 0)
            {
                forwardAccel = 0;
            }

            // Rotation
            lowerBody.transform.Rotate(Vector3.up, Input.GetAxis("Horizontal") * lowerbodyRotateSpeed * Time.deltaTime);
            Quaternion newRot = Quaternion.LookRotation(new Vector3(-mousePlayerDiff.x, 0, -mousePlayerDiff.y));
            upperBody.transform.rotation = Quaternion.Lerp(upperBody.transform.rotation, newRot, upperbodyRotateSpeed * Time.deltaTime);
        }
        else
        {
            // Movement
            Vector3 m = Vector3.zero;
            m = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            modifiedMoveSpeed = movespeed;
            charController.SimpleMove(m.normalized * modifiedMoveSpeed);

            //Rotation
            Quaternion newRot = Quaternion.LookRotation(new Vector3(-mousePlayerDiff.x, 0, -mousePlayerDiff.y));
            upperBody.transform.rotation = Quaternion.Lerp(upperBody.transform.rotation, newRot, upperbodyRotateSpeed * Time.deltaTime);
            if (m.normalized != Vector3.zero)
            {
                Quaternion bottomRotation = Quaternion.LookRotation(m.normalized);
                lowerBody.transform.rotation = Quaternion.Lerp(lowerBody.transform.rotation, bottomRotation, lowerbodyRotateSpeed * Time.deltaTime);
            }
        }

        #endregion Movement

        #region Weapons

        if (guns.Length > 0)
        {
            if ((Input.GetMouseButton(0) || Input.GetMouseButton(1)))
            {
                if (Input.GetMouseButton(0))
                {
                    if (guns[0].ammo <= 0)
                    {
                        int[] reloadingGuns = { 0 };
                        photonView.RPC("Reload", PhotonTargets.All, reloadingGuns);
                    }
                    else
                    {
                        if (guns[0].available)
                        {
                            int[] shootingGuns = { 0 };
                            float[] speeds = { modifiedMoveSpeed };
                            photonView.RPC("Fire", PhotonTargets.All, shootingGuns, speeds);
                        }
                    }
                }
                if (Input.GetMouseButton(1))
                {
                    if (guns[1].ammo <= 0)
                    {
                        guns[1].ammo = 0;
                        int[] reloadingGuns = { 1 };
                        photonView.RPC("Reload", PhotonTargets.All, reloadingGuns);
                    }
                    else
                    {
                        if (guns[1].available)
                        {
                            int[] shootingGuns = { 1 };
                            float[] speeds = { modifiedMoveSpeed };
                            photonView.RPC("Fire", PhotonTargets.All, shootingGuns, speeds);
                        }
                    }
                }
            }

            if (Input.GetButtonDown("ReloadPrimary"))
            {
                int[] reloadingGuns = { 0 };
                photonView.RPC("Reload", PhotonTargets.All, reloadingGuns);
            }

            if (Input.GetButtonDown("ReloadSecondary"))
            {
                int[] reloadingGuns = { 1 };
                photonView.RPC("Reload", PhotonTargets.All, reloadingGuns);
            }

        }

        if (Input.GetKey(KeyCode.Tab))
        {
            GUIManager.Instance.state = GUIManager.GUIManagerState.ShowingStatTab;
        }
        else
        {
            GUIManager.Instance.state = GUIManager.GUIManagerState.Hidden;
        }

        #endregion Weapons
    }

    [RPC]
    private void Fire(int[] gunIndex, float[] speed)
    {
        for (int i = 0; i < gunIndex.Length; i++)
        {
            int currentGun = gunIndex[i];
            float currentGunSpeed = speed[i];
            guns[currentGun].PlayerFire(currentGunSpeed);
        }
    }

    [RPC]
    private void Reload(int[] gunIndex)
    {
        for (int i = 0; i < gunIndex.Length; i++)
        {
            int currentGun = gunIndex[i];
            guns[currentGun].ammo = 0;
            guns[currentGun].PlayerTriggerReload();
        }
    }

    private void CheckHealth()
    {
        if (health <= 0 && !waitingForRespawn) // should respawn
        {
            deathCount++;
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

    private void DoExplosionAnimation(KBPlayer player)
    {
        PlayerGibModel gib = null;
        switch (player.type)
        {
            case PlayerType.mech:
                gib = mechGibBody;
                break;

            case PlayerType.drone:
                gib = droneGibBody;
                break;

            case PlayerType.tank:
                gib = tankGibBody;
                break;

            case PlayerType.core:
                break;

            default:
                break;
        }
        PlayerGibModel g = ObjectPool.Spawn(gib, player.gameObject.transform.position + Vector3.up * 3, player.gameObject.transform.rotation);
        g.Init();

        player.upperBody.SetActive(false);
        player.lowerBody.SetActive(false);
        player.ammoHud.SetActive(false);

        player.particleSystem.Emit(5000);

        int n = 5;
        Chaff c = null;
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                for (int h = 0; h < n; h++)
                {
                    c = ObjectPool.Spawn(chaff,
                        new Vector3(
                            player.gameObject.transform.position.x - (n / 2.0f * chaff.transform.localScale.x) + (n * chaff.transform.localScale.x * i),
                            player.gameObject.transform.position.y - (n / 2.0f * chaff.transform.localScale.x) + (n * chaff.transform.localScale.x * i),
                            player.gameObject.transform.position.z - (n / 2.0f * chaff.transform.localScale.x) + (n * chaff.transform.localScale.x * i)
                            ),
                            Quaternion.identity
                            );
                    c.Init();
                }
            }
        }
        //c.gameObject.rigidbody.AddExplosionForce(1.0f, transform.position, 1.0f);
    }

    private void FindTeamSpawnpoints()
    {
        if (team == Team.None && type != PlayerType.core)
        {
            Debug.Log("No Team, no team spawnpoints found");
            //Debug.LogWarning("Warning: Attempting to find spawnpoint on player with team none");
        }
        else
        {
            teamSpawnpoints = GameManager.Instance.GetSpawnPointsWithTeam(team);
        }
    }

    public void ConfirmHit(KBPlayer victimObject, int damage)
    {
        photonView.RPC("ConfirmHitToOthers", PhotonTargets.All, victimObject.networkPlayer, damage);
    }

    [RPC]
    public void ConfirmHitToOthers(PhotonPlayer hitPhotonPlayer, int damage)
    {
        List<KBPlayer> currentPlayers = GameManager.Instance.players;

        for (int i = 0; i < currentPlayers.Count; i++)
        {
            if (currentPlayers[i].networkPlayer == hitPhotonPlayer)
            {
                HitFX fx = ObjectPool.Spawn(hitExplosion, currentPlayers[i].transform.position, Quaternion.identity);
                fx.DoEffect(damage);
                guns[0].audio.PlayOneShot(hitConfirm);
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
                HitFX fx = ObjectPool.Spawn(hitExplosion, transform.position, Quaternion.identity);
                fx.DoEffect(amount);
                Camera.main.GetComponent<ScreenShake>().StartShake(0.125f, 10.0f * (amount / 100));
                audio.PlayOneShot(gotHitSFX[Random.Range(0, gotHitSFX.Length)]);
                lastDamageTime = Time.time;
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
        killerPlayer.photonView.RPC("NotifyKill", PhotonTargets.All, killerPlayer.networkPlayer, this.networkPlayer);
    }

    [RPC]
    public void NotifyKill(PhotonPlayer killerPlayer, PhotonPlayer victimPlayer)
    {
        if (killerPlayer.isLocal && networkPlayer == PhotonNetwork.player)
        {
            killCount++;
            totalTokensGained++;
            killTokens++;
        }
        List<KBPlayer> currentPlayers = GameManager.Instance.players;

        for (int i = 0; i < currentPlayers.Count; i++)
        {
            if (currentPlayers[i].networkPlayer == victimPlayer)
            {
                DoExplosionAnimation(currentPlayers[i]);
            }
        }
    }

    /// <summary>
    /// Spawns the player in the dropship (type select area)
    /// </summary>
    private void RespawnToPrespawn()
    {
        //Spawn as Core
        photonView.RPC("SwitchType", PhotonTargets.AllBuffered, "SpawnCore");

        totalTokensLost += killTokens;
        waitingForRespawn = false;
        acceptingInputs = true;

        for (int i = 0; i < guns.Length; i++)
        {
            guns[i].ammo = 0;
        }

        ammoHud.SetActive(false);

        Camera.main.GetComponent<ScreenShake>().StopShake();
        Vector3 deathPosition = transform.position;
        transform.position = GameObject.FindGameObjectWithTag("Prespawn").transform.position;
        health = coreBaseHealth;

		if(team != Team.None)
		{
			GenerateKillTags(killTokens, deathPosition);	
		}
    }

    private void GenerateKillTags(int _points, Vector3 center)
    {
        int pointsToDrop = Mathf.FloorToInt(_points * GameConstants.pointPercentDropOnDeath);
        if (pointsToDrop == 0)
        {
            pointsToDrop = 1;
        }

        while (pointsToDrop > 0)
        {
            int thisTagValue = 0;
            if (pointsToDrop <= 8)
            {
                thisTagValue = 1;
            }
            else if (pointsToDrop > 8 && pointsToDrop <= 32)
            {
                thisTagValue = 8;
            }
            else if (pointsToDrop > 32 && pointsToDrop <= 128)
            {
                thisTagValue = 32;
            }
            else if (pointsToDrop > 128)
            {
                thisTagValue = 128;
            }

            GameObject newTag = null;
            if (team == Team.Blue)
            {
                newTag = GameManager.Instance.CreateObject((int)ObjectConstants.type.KillTagBlue, center, Quaternion.identity, (int)team);
            }
            else if (team == Team.Red)
            {
                newTag = GameManager.Instance.CreateObject((int)ObjectConstants.type.KillTagRed, center, Quaternion.identity, (int)team);
            }
            newTag.transform.localScale *= 1.0f + (pointsToDrop / 100.0f);
            newTag.transform.Translate(new Vector3(Random.Range(-10.0f, 10.0f), 0, Random.Range(-10.0f, 10.0f)));
            newTag.GetPhotonView().RPC("SetPointValue", PhotonTargets.AllBuffered, thisTagValue);
            pointsToDrop -= thisTagValue;
        }

        killTokens = 0;
    }

    /// <summary>
    /// Spawns the player in the combat area.
    /// </summary>
    private IEnumerator Spawn(string tag)
    {
        audio.PlayOneShot(respawnSound);
        spawnAnimator.GetComponent<SpawnAnimation>().Activate();
        acceptingInputs = false;
        yield return new WaitForSeconds(spawnDelay);
        if (teamSpawnpoints.Count > 0 && photonView.isMine)
        {
            photonView.RPC("SwitchType", PhotonTargets.AllBuffered, tag);
            int spawnPointIndex = Random.Range(0, teamSpawnpoints.Count - 1);
            transform.position = teamSpawnpoints[spawnPointIndex].transform.position;
            waitingForRespawn = false;
            acceptingInputs = true;
            health = stats.health;
            movespeed = stats.speed;
            invulnerabilityTime = spawnProtectionTime;
            lowerbodyRotateSpeed = stats.lowerbodyRotationSpeed;
            upperbodyRotateSpeed = stats.upperbodyRotationSpeed;

            Camera.main.GetComponent<ScreenShake>().StopShake();
        }
        spawnAnimator.GetComponent<SpawnAnimation>().Reset();
    }

    private void SetupAbilities()
    {
        guns = upperBody.GetComponentsInChildren<ProjectileAbilityBaseScript>();
        for (int i = 0; i < guns.Length; i++)
        {
            guns[i].owner = this;
            guns[i].Team = team;
            guns[i].ammo = guns[i].clipSize;
            guns[i].reloading = false;
        }
    }

    private void SetTeam(Team newTeam)
    {
        team = newTeam;
        FindTeamSpawnpoints();
        hitboxDrone.GetComponent<HitboxBaseScript>().Team = team;
        hitboxMech.GetComponent<HitboxBaseScript>().Team = team;
        hitboxTank.GetComponent<HitboxBaseScript>().Team = team;

        if (newTeam == KBConstants.Team.Blue)
        {
            SetActiveIfFound(this.transform, "BlueBody", true);
            SetActiveIfFound(this.transform, "RedBody", false);
        }
        else if (newTeam == KBConstants.Team.Red)
        {
            SetActiveIfFound(this.transform, "BlueBody", false);
            SetActiveIfFound(this.transform, "RedBody", true);
        }
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

            if (typeTrigger.Equals("SpawnRedDrone") || typeTrigger.Equals("SpawnBlueDrone"))
            {
                upperBody = upperBodyDrone;
                lowerBody = lowerBodyDrone;
                type = PlayerType.drone;
            }

            else if (typeTrigger.Equals("SpawnRedMech") || typeTrigger.Equals("SpawnBlueMech"))
            {
                upperBody = upperBodyMech;
                lowerBody = lowerBodyMech;
                type = PlayerType.mech;
            }

            else if (typeTrigger.Equals("SpawnRedTank") || typeTrigger.Equals("SpawnBlueTank"))
            {
                upperBody = upperBodyTank;
                lowerBody = lowerBodyTank;
                type = PlayerType.tank;
            }

            else if (typeTrigger.Equals("SpawnCore"))
            {
                upperBody = upperBodyCore;
                lowerBody = lowerBodyCore;
                type = PlayerType.core;
            }

            upperBody.SetActive(true);
            lowerBody.SetActive(true);

            //if (team == KBConstants.Team.Blue)
            //{
            //    SetActiveIfFound(this.transform, "BlueBody", true);
            //    SetActiveIfFound(this.transform, "RedBody", false);
            //}
            //else if (team == KBConstants.Team.Red)
            //{
            //    SetActiveIfFound(this.transform, "BlueBody", false);
            //    SetActiveIfFound(this.transform, "RedBody", true);
            //}

            //Switch Stats
            SetStats();
            InitializeForRespawn();
            SetupAbilities();
            ammoHud.SetActive(true);

            if (photonView.isMine)
            {
                GetComponentInChildren<WeaponAmmoGUI>().Setup();
            }
        }
    }

    private void SetActiveIfFound(Transform trans, string objectName, bool val)
    {
        List<GameObject> foundObjects = SetActiveChildIfFound(trans, objectName, val);

        if (foundObjects.Count > 0)
        {
            for (int i = 0; i < foundObjects.Count; i++)
            {
                foundObjects[i].SetActive(val);
            }
        }
        else
        {
            Debug.LogWarning("[Player] Couldn't find " + objectName.ToString() + " player model");
        }
    }

    private List<GameObject> SetActiveChildIfFound(Transform cTransform, string objectName, bool val)
    {
        List<GameObject> foundGameObjects = new List<GameObject>();

        for (int i = 0; i < cTransform.childCount; i++)
        {
            GameObject currentchild = cTransform.GetChild(i).gameObject;

            if (currentchild.name.Equals(objectName))
            {
                foundGameObjects.Add(currentchild);
            }

            if (currentchild.transform.childCount > 0)
            {
                foundGameObjects.AddRange(SetActiveChildIfFound(currentchild.transform, objectName, val));
            }
        }

        return foundGameObjects;
    }

    public IEnumerator GamepadVibrateTriggers(float time)
    {
        GamePad.SetVibration(playerIndex, gamepadState.Triggers.Left, gamepadState.Triggers.Right);
        yield return new WaitForSeconds(time);
        GamePad.SetVibration(playerIndex, 0, 0);
    }

    public IEnumerator GamepadVibrateAll(float time, float magnitude)
    {
        GamePad.SetVibration(playerIndex, magnitude, magnitude);
        yield return new WaitForSeconds(time);
        GamePad.SetVibration(playerIndex, 0, 0);
    }
}