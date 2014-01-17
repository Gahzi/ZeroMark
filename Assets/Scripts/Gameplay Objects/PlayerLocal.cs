using KBConstants;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(TimerScript))]
public class PlayerLocal : KBControllableGameObject
{
    public enum ControlStyle { ThirdPerson, TopDown };

    public ControlStyle controlStyle;
    public PlayerType type;
    public PlayerStats stats;
    public bool acceptingInputs = true;
    public bool waitingForRespawn = false;

    public int level;

    public TimerScript timer;
    private float movespeed;
    private float lowerbodyRotateSpeed;
    private float upperbodyRotateSpeed;
    private GamepadInfo gamepad;
    private CharacterController controller;
    private Vector3 latestCorrectPos;
    private Vector3 onUpdatePos;
    private float fraction;
    protected PhotonView photonView;
    private int layerMask;
    public AudioClip grabSound;
    public GameObject upperBody;
    public Camera camera;
    public Vector3 mousePos;
    public Vector3 playerPositionOnScreen;
    public Vector3 mousePlayerDiff;
    public ProjectileAbilityBaseScript gun;
    public PlayerSpawnZone spawnZone;
    public float respawnTime;
    private Vector3 lookRotation;
    private int respawnTimerNumber;
    public int upgradePoints;
    public int maxHealth;
    public int[] pointToLevel = new int[4];

    public override void Start()
    {
        base.Start();
        acceptingInputs = true;
        waitingForRespawn = false;
        controller = GetComponent<CharacterController>();
        grabSound = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.ItemGrab]);

        gun = gameObject.GetComponentInChildren<ProjectileAbilityBaseScript>();
        gun.SetMaxRange(100);

        movespeed = stats.speed;
        lowerbodyRotateSpeed = stats.lowerbodyRotationSpeed;

        int itemLayer = 8;
        int towerLayer = 13;
        int modelLayer = 15;
        int layerMask1 = 1 << itemLayer;
        int layerMask2 = 1 << towerLayer;
        int layerMask3 = 1 << modelLayer;
        layerMask = layerMask1 | layerMask2 | layerMask3;

        //GamepadInfoHandler gamepadHandler = GamepadInfoHandler.Instance;
        //Debug.Log("Attempting to attach Controller");
        //gamepadHandler.AttachControllerToPlayer(this);

        photonView = this.GetComponent<PhotonView>();

        //gun = gameObject.GetComponentInChildren<ProjectileAbilityBaseScript>();
        //gun.SetMaxRange(stats.attackRange);

        //TODO: Prefer to do this stuff in code as seen below instead of dragging bullshit in Unity Editor.
        //this.photonView.observed = this.transform;
        //this.photonView.synchronization = ViewSynchronization.ReliableDeltaCompressed;

        Respawn();
    }

    public void SetGamepad(GamepadInfo newGamepad)
    {
        gamepad = newGamepad;
        Debug.Log("Gamepad was set to Player");
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(upperBody.transform.position, upperBody.transform.TransformDirection(new Vector3(0, 0, 5.0f)), new Color(255, 0, 0, 255), 0.0f);
    }
    
    void OnGUI()
    {
        //GUI.Box(new Rect(0, 0, 100, 50), "Top-left");
        GUI.Box(new Rect(Screen.width - 200, 0, 200, 200), GameManager.GetCaptureZoneStateString());
        GUI.Box(new Rect(0, Screen.height - 50, 100, 50), GameManager.GetTeamScoreString());
        //GUI.Box(new Rect(Screen.width - 100, Screen.height - 50, 100, 50), "Bottom-right");
    }
    

    private void Update()
    {
        mousePos = Input.mousePosition;
        playerPositionOnScreen = camera.WorldToScreenPoint(transform.position);
        mousePlayerDiff = playerPositionOnScreen - mousePos;

        if (acceptingInputs)
        {
            if (gamepad != null)
            {
                ControlGamepad();
            }
            else
            {
                ControlKBAM();
            }
        }

        CheckHealth();
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
            Quaternion rot = transform.localRotation;
            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
        }
        else
        {
            // Receive latest state information
            Vector3 pos = Vector3.zero;
            Quaternion rot = Quaternion.identity;

            stream.Serialize(ref pos);
            stream.Serialize(ref rot);

            latestCorrectPos = pos;                 // save this to move towards it in FixedUpdate()
            onUpdatePos = transform.localPosition;  // we interpolate from here to latestCorrectPos
            fraction = 0;                           // reset the fraction we alreay moved. see Update()

            transform.localRotation = rot;          // this sample doesn't smooth rotation
        }
    }

    //private void attachPlayerToControllableGameObject(KBControllableGameObject newGameObject)
    //{
    //    newGameObject.attachedPlayer = this;
    //    //this.renderer.enabled = false;
    //    KBCamera cameraScript = Camera.main.gameObject.GetComponent<KBCamera>();

    //    if (cameraScript != null)
    //    {
    //        cameraScript.attachedPlayer = newGameObject;
    //    }
    //}

    /// <summary>
    /// Collision method that is called when rigidbody hits another game object
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
    }

    public override void takeDamage(int amount)
    {
        health -= amount;
    }

    private void ControlGamepad()
    {
        //if (gamepad.leftStick.magnitude > PLAYER_MOVEMENT_DEADZONE)
        //{
        //    bottom.transform.LookAt(transform.position + new Vector3(-gamepad.leftStick.x, bottom.transform.position.y, -gamepad.leftStick.y));
        //    Vector3 fwd = bottom.transform.TransformDirection(Vector3.forward);
        //    rigidbody.velocity = fwd * currentMovespeed;
        //}
        //else
        //{
        //    rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, Vector3.zero, 0.5f * Time.deltaTime);
        //}

        //if (gamepad.rightStick.magnitude > PLAYER_MOVEMENT_DEADZONE)
        //{
        //    top.transform.LookAt(transform.position + new Vector3(-gamepad.rightStick.x, top.transform.position.y, -gamepad.rightStick.y));
        //}

        //if (gamepad.button[0])
        //{
        //    if (!gun.GetActive())
        //    {
        //        gun.ActivateAbility();
        //    }
        //}

        //if (gamepad.buttonUp[0])
        //{
        //    gun.DeactivateAbility();
        //}
    }

    private void ControlKBAM()
    {
        switch (controlStyle)
        {
            case ControlStyle.ThirdPerson:
                float d = movespeed * Input.GetAxis("Vertical");
                controller.SimpleMove(transform.TransformDirection(Vector3.forward) * d);
                transform.Rotate(0, Input.GetAxis("Horizontal") * lowerbodyRotateSpeed, 0);
                break;

            case ControlStyle.TopDown:
                Vector3 m = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                controller.SimpleMove(m.normalized * movespeed);

                Quaternion newRot = Quaternion.LookRotation(upperBody.transform.position + new Vector3(-mousePlayerDiff.x, 0, -mousePlayerDiff.y));
                upperBody.transform.rotation = Quaternion.Lerp(upperBody.transform.rotation, newRot, upperbodyRotateSpeed * Time.deltaTime);
                break;

            default:
                break;
        }

        if (Input.GetMouseButtonDown(0))
        {
            gun.ActivateAbility();
        }
        if (Input.GetMouseButtonUp(0))
        {
            gun.DeactivateAbility();
        }
    }

    private void CheckHealth()
    {
        if (health <= 0 && !waitingForRespawn)
        {
            acceptingInputs = false;
            respawnTimerNumber = timer.StartTimer(respawnTime);
            waitingForRespawn = true;
        }
        else if (waitingForRespawn)
        {
            if (!timer.IsTimerActive(respawnTimerNumber))
            {
                Respawn();
            }
        }
    }

    private void Respawn()
    {
        transform.position = spawnZone.transform.position;
        waitingForRespawn = false;
        acceptingInputs = true;
        health = stats.health;
        maxHealth = health;
        movespeed = stats.speed;
        level = stats.level;
        lowerbodyRotateSpeed = stats.lowerbodyRotationSpeed;
        upperbodyRotateSpeed = stats.upperbodyRotationSpeed;
    }
}