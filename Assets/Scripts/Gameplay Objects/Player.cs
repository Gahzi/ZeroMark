using KBConstants;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PhotonView))]
public class Player : KBControllableGameObject
{
    public PlayerType type;

    //public static float PLAYER_MOVEMENT_SPEED = 30f;
    public static float PLAYER_MOVEMENT_DEADZONE = 0.1f;

    //public static float PLAYER_ROTATE_SPEED = 7.0f;

    public PlayerStats stats;

    private float currentMovespeed;
    private float currentRotateSpeed;

    public GamepadInfo gamepad;
    private Vector3 latestCorrectPos;
    private Vector3 onUpdatePos;
    private float fraction;
    public GameObject heldItem;
    private ProjectileAbilityBaseScript gun;
    public GameObject top;
    public GameObject bottom;

    protected PhotonView photonView;

    private int layerMask;

    public AudioClip grabSound;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        InitializeStats(type);
        grabSound = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.ItemGrab]);

        currentMovespeed = stats.speed;
        currentRotateSpeed = stats.rotationSpeed;

        int itemLayer = 8;
        int towerLayer = 13;
        int modelLayer = 15;
        int layerMask1 = 1 << itemLayer;
        int layerMask2 = 1 << towerLayer;
        int layerMask3 = 1 << modelLayer;
        layerMask = layerMask1 | layerMask2 | layerMask3;

        health = 100;

        GamepadInfoHandler gamepadHandler = GamepadInfoHandler.Instance;
        Debug.Log("Attempting to attach Controller");
        gamepadHandler.AttachControllerToPlayer(this);

        photonView = this.GetComponent<PhotonView>();

        gun = gameObject.GetComponentInChildren<ProjectileAbilityBaseScript>();
        gun.SetMaxRange(stats.attackRange);

        //TODO: Prefer to do this stuff in code as seen below instead of dragging bullshit in Unity Editor.
        //this.photonView.observed = this.transform;
        //this.photonView.synchronization = ViewSynchronization.ReliableDeltaCompressed;
    }

    /// <summary>
    /// Initializes player stats based on the player's type as set in the inspector.
    /// </summary>
    /// <param name="type">enum PlayerType</param>
    private void InitializeStats(PlayerType type)
    {
        switch (type)
        {
            case PlayerType.attack:
                stats.health = 3;
                stats.attack = 1;
                stats.attackRange = 1;
                stats.captureSpeed = 1;
                stats.rotationSpeed = 1;
                stats.speed = 1;
                stats.visionRange = 1;
                break;

            case PlayerType.recon:
                stats.health = 3;
                stats.attack = 1;
                stats.attackRange = 1;
                stats.captureSpeed = 1;
                stats.rotationSpeed = 5;
                stats.speed = 10;
                stats.visionRange = 1;
                break;

            case PlayerType.defense:
                stats.health = 3;
                stats.attack = 1;
                stats.attackRange = 1;
                stats.captureSpeed = 1;
                stats.rotationSpeed = 1;
                stats.speed = 1;
                stats.visionRange = 1;
                break;

            default:
                break;
        }
    }

    public void SetGamepad(GamepadInfo newGamepad)
    {
        gamepad = newGamepad;
        Debug.Log("Gamepad was set to Player");
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(bottom.transform.position, bottom.transform.TransformDirection(new Vector3(0, 0, 20.0f)), new Color(0, 255, 0, 255), 0.0f);
        Debug.DrawRay(top.transform.position, top.transform.TransformDirection(new Vector3(0, 0, 20.0f)), new Color(255, 0, 0, 255), 0.0f);
    }

    private void Update()
    {
        if (gamepad != null)
        {
            if (gamepad.leftStick.magnitude > PLAYER_MOVEMENT_DEADZONE)
            {
                bottom.transform.LookAt(transform.position + new Vector3(-gamepad.leftStick.x, bottom.transform.position.y, -gamepad.leftStick.y));
                Vector3 fwd = bottom.transform.TransformDirection(Vector3.forward);
                rigidbody.velocity = fwd * currentMovespeed;

            }
            else
            {
                rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, Vector3.zero, 0.5f * Time.deltaTime);
            }

            if (gamepad.rightStick.magnitude > PLAYER_MOVEMENT_DEADZONE)
            {
                top.transform.LookAt(transform.position + new Vector3(-gamepad.rightStick.x, top.transform.position.y, -gamepad.rightStick.y));
            }

            if (gamepad.button[0])
            {
                if (!gun.GetActive())
                {
                    gun.ActivateAbility();
                }
            }

            if (gamepad.buttonUp[0])
            {
                gun.DeactivateAbility();
            }
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

    private void attachPlayerToControllableGameObject(KBControllableGameObject newGameObject)
    {
        newGameObject.attachedPlayer = this;
        //this.renderer.enabled = false;
        KBCamera cameraScript = Camera.main.gameObject.GetComponent<KBCamera>();

        if (cameraScript != null)
        {
            cameraScript.attachedObject = newGameObject;
        }
    }

    /// <summary>
    /// Collision method that is called when rigidbody hits another game object
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("OnCollisionEnter!");
        KBControllableGameObject colControllablePlayerObject = collision.gameObject.GetComponent<KBControllableGameObject>();
        if (colControllablePlayerObject != null)
        {
            attachPlayerToControllableGameObject(colControllablePlayerObject);
        }

        // TODO: Handle collision with items here
    }

    public override void takeDamage(int amount)
    {
        health -= amount;
    }
}