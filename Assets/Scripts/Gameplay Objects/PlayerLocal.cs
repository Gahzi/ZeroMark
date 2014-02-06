using KBConstants;
using System.Collections.Generic;
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

    public override void Start()
    {
        base.Start();
        acceptingInputs = true;
        waitingForRespawn = false;
        charController = GetComponent<CharacterController>();
        grabSound = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.ItemGrab]);

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

        if (teamScript.team != Team.None)
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
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(upperBody.transform.position, upperBody.transform.TransformDirection(new Vector3(0, 0, 5.0f)), new Color(255, 0, 0, 255), 0.0f);
    }
    
    void OnGUI()
    {
        if (photonView.isMine)
        {
            //GUI.Box(new Rect(0, 0, 100, 50), "Top-left");
            GUI.Box(new Rect(Screen.width - 200, 0, 200, 200), GameManager.GetCaptureZoneStateString());
            GUI.Box(new Rect(0, Screen.height - 50, 100, 50), GameManager.GetTeamScoreString());
            //GUI.Box(new Rect(Screen.width - 100, Screen.height - 50, 100, 50), "Bottom-right");


            foreach (var i in GameManager.Instance.captureZones)
            {
                Vector3 t = i.gameObject.transform.position;
                Vector3 a = camera.WorldToViewportPoint(t);
                if (a.z > 0)
                {
                    if (a.y > 0.35f)
                    {
                        a.y = 0.35f;
                    }
                    GUI.Label(new Rect(Screen.width * a.x, Screen.height * 0.35f, 100, 50), i.tier.ToString());
                }
            }
        }
    }
    

    private void Update()
    {
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
        else if(!isShooting && gun.GetActive())
        {
            gun.DeactivateAbility();
        }

        //if (!photonView.isMine)
        //{
        //    Debug.Log("transform.position: " + transform.position.ToString());
        //}

        CheckHealth();
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


    public override void takeDamage(int amount)
    {
        health -= amount;
    }

    private void ControlKBAM()
    {
        switch (controlStyle)
        {
            case ControlStyle.ThirdPerson:
                float d = movespeed * Input.GetAxis("Vertical");
                charController.SimpleMove(transform.TransformDirection(Vector3.forward) * d);
                transform.Rotate(0, Input.GetAxis("Horizontal") * lowerbodyRotateSpeed, 0);


                break;

            case ControlStyle.TopDown:
                Vector3 m = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                charController.SimpleMove(m.normalized * movespeed);

                Quaternion newRot = Quaternion.LookRotation(upperBody.transform.position + new Vector3(-mousePlayerDiff.x, 0, -mousePlayerDiff.y));
                upperBody.transform.rotation = Quaternion.Lerp(upperBody.transform.rotation, newRot, upperbodyRotateSpeed * Time.deltaTime);
                break;

            default:
                break;
        }

        if (Input.GetMouseButtonDown(0))
        {
            isShooting = true;
            gun.ActivateAbility();
        }
        if (Input.GetMouseButtonUp(0))
        {
            isShooting = false;
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
                Debug.Log("Respawning!");
                Respawn();
            }
        }
    }

    private void FindTeamSpawnpoints()
    {
        if(teamScript.team == Team.None)
        {
            Debug.LogWarning("Warning: Attempting to find spawnpoint on player with team none");
        }
        else
        {
            teamSpawnpoints = GameManager.Instance.GetSpawnPointsWithTeam(teamScript.team);
        }
    }

    private void Respawn()
    {
        if(teamSpawnpoints.Count > 0)
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
}