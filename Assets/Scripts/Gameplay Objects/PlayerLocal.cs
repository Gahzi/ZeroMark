using KBConstants;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(CharacterController))]
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

    public override void Start()
    {
        base.Start();
        acceptingInputs = true;
        waitingForRespawn = false;
        charController = GetComponent<CharacterController>();
        grabSound = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.ItemGrab]);
        playerGuiSquare = GetComponent<RotatableGuiItem>();

        latestCorrectPos = transform.position;
        onUpdatePos = transform.position;
        isShooting = false;


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

    void OnGUI()
    {
        if (photonView.isMine)
        {
        }
    }

    private void Update()
    {

        mousePos = Input.mousePosition;
        fraction = fraction + Time.deltaTime * 9;
        playerPositionOnScreen = camera.WorldToScreenPoint(transform.position);
        mousePlayerDiff = playerPositionOnScreen - mousePos;

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

        //if (!photonView.isMine)
        //{
        //    Debug.Log("transform.position: " + transform.position.ToString());
        //}

        if (item != null)
        {
            item.transform.position = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);
            item.State = Item.ItemState.isPickedUp;
        }

        CheckHealth();

        #region GUI Stuff
        playerGuiSquare.relativePosition = new Vector2(Screen.width / 2, 10.0f);
        playerGuiSquare.angle = Mathf.Sin(Time.time / 2.0f) * 360.0f;
        #endregion
    }

    void OnPhotonInstantiate(PhotonMessageInfo msg)
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

            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
            stream.Serialize(ref hlth);
            stream.Serialize(ref lvl);
            stream.Serialize(ref mxhlth);
            stream.Serialize(ref isShting);
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

            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
            stream.Serialize(ref hlth);
            stream.Serialize(ref lvl);
            stream.Serialize(ref mxhlth);
            stream.Serialize(ref isShting);

            latestCorrectPos = pos;                 // save this to move towards it in FixedUpdate()
            onUpdatePos = transform.localPosition;  // we interpolate from here to latestCorrectPos
            fraction = 0;                           // reset the fraction we alreay moved. see Update()

            upperBody.transform.rotation = rot;          // this sample doesn't smooth rotation
            health = hlth;
            level = lvl;
            maxHealth = mxhlth;
            isShooting = isShting;
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

    public override void takeDamage(int amount)
    {
        health -= amount;
    }

    private void ControlKBAM()
    {
        float modifiedMoveSpeed = 0;
        if (item != null)
        {
            modifiedMoveSpeed = movespeed * 2;
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
            if (item != null)
            {
                gun.ActivateAbility();
                isShooting = true;
            }

            Vector3 fwd = upperBody.transform.TransformDirection(Vector3.forward);
            //transform.position = upperBody.transform.position - upperBody.transform.forward * 5;
            //transform.position = new Vector3(transform.position.x - upperBody.transform.forward.x, transform.position.y, transform.position.z - upperBody.transform.forward.z);
        }
        if (Input.GetMouseButtonUp(0))
        {
            isShooting = false;
            gun.DeactivateAbility();
        }

        if (Input.GetMouseButtonDown(1))
        {
            DropItem();
        }
    }

    private void CheckHealth()
    {
        if (health <= 0 && !waitingForRespawn)
        {
            acceptingInputs = false;
            respawnTimerNumber = timer.StartTimer(respawnTime);
            DropItem();
            waitingForRespawn = true;
        }
        else if (waitingForRespawn)
        {
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
            lowerbodyRotateSpeed = stats.lowerbodyRotationSpeed;
            upperbodyRotateSpeed = stats.upperbodyRotationSpeed;
        }

    }

    private void PickupItem(Item _item)
    {
        item = _item;
        item.State = Item.ItemState.isPickedUp;
    }

    private void DropItem()
    {
        if (item != null)
        {
            //item.Respawn();
            item.State = Item.ItemState.disabled;
            item.disableTime = Time.time;
            item = null;
        }
    }
}