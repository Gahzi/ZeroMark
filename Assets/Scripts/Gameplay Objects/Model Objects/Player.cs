using UnityEngine;
using System.Collections;
using KBConstants;

[RequireComponent(typeof(GamepadInfo))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PhotonView))]

public class Player : KBControllableGameObject
{

    public static float PLAYER_MOVEMENT_SPEED = 30f;
    public static float PLAYER_PULL_SPEED = 5f;
    public static float PLAYER_PULL_ROTATE_SPEED = 0.1f;
    public static float PLAYER_MOVEMENT_DEADZONE = 0.3f;
    public static float PLAYER_ROTATE_SPEED = 7.0f;
    public static float PLAYER_PULL_DISTANCE = 15f;

    //TODO: MOVE THESE TO TOWER OR SOMETHING
    public static float PLAYER_TOWER_PUSH_LERP_STRENGTH = 0.75f;
    public static float PLAYER_ITEM_PUSH_LERP_STRENGTH = 10.0f;

    private float currentMovespeed;
    private float currentRotateSpeed;

    public GamepadInfo gamepad;
    private Vector3 latestCorrectPos;
    private Vector3 onUpdatePos;
    private float fraction;
    private GUISelectCube selectCube;
    private GameObject selectedObj;

    protected PhotonView photonView;

    int layerMask;

    public AudioClip grabSound;

    // Use this for initialization
    void Start()
    {
        grabSound = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.ItemGrab]);
        
        currentMovespeed = PLAYER_MOVEMENT_SPEED;
        currentRotateSpeed = PLAYER_ROTATE_SPEED;
        
        int itemLayer = 8;
        int towerLayer = 13;
        int layerMask1 = 1 << itemLayer;
        int layerMask2 = 1 << towerLayer;
        layerMask = layerMask1 | layerMask2;

        health = 100;

        Team = Team.Red;

        GamepadInfoHandler gamepadHandler = GamepadInfoHandler.Instance;
        Debug.Log("Attempting to attach Controller");
        gamepadHandler.AttachControllerToPlayer(this);

        GameObject.Instantiate(Resources.Load(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.GUISelectCube]), Vector3.zero, Quaternion.identity);
        selectCube = GameObject.FindObjectOfType<GUISelectCube>();
        selectCube.renderer.enabled = false;
        GameObject newCameraObject = (GameObject)GameObject.Instantiate(Resources.Load(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.Camera]), Vector3.zero, Quaternion.identity);
        KBCamera cameraScript = newCameraObject.GetComponent<KBCamera>();
        cameraScript.attachedObject = this;

        photonView = this.GetComponent<PhotonView>();

        //TODO: Prefer to do this stuff in code as seen below instead of dragging bullshit in Unity Editor.
        //this.photonView.observed = this.transform;
        //this.photonView.synchronization = ViewSynchronization.ReliableDeltaCompressed;
    }

    public void SetGamepad(GamepadInfo newGamepad)
    {
        gamepad = newGamepad;
        Debug.Log("Gamepad was set to Player");
    }

    void Update()
    {
        if (selectCube == null)
        {
            GameObject.Instantiate(Resources.Load(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.GUISelectCube]), Vector3.zero, Quaternion.identity);
            selectCube = GameObject.FindObjectOfType<GUISelectCube>();
            selectCube.renderer.enabled = false;
        }

        if (gamepad != null)
        {
            Vector3 fwd = transform.TransformDirection(Vector3.forward);
            if (selectedObj == null)
            {
                // Raycasting for "pushable"
                RaycastHit hit;
                Vector3 rayStart = transform.position;
                rayStart.y = rayStart.y + 2.0f;
                //rayStart = Vector3.RotateTowards(rayStart, PLAYER_PULL_DISTANCE * fwd, Mathf.PI*2, 1000f);
                if (Physics.Raycast(rayStart, fwd, out hit, PLAYER_PULL_DISTANCE, layerMask))
                {
                    selectCube.transform.parent = hit.collider.transform;
                    selectCube.renderer.enabled = true;
                    if (gamepad.buttonDown[9] && selectedObj == null)
                    {
                        selectedObj = hit.collider.gameObject;
                        audio.PlayOneShot(grabSound);
                    }
                }
                else
                {
                    selectCube.renderer.enabled = false;
                }
            } 
            else if (selectedObj != null)
            {
                if (gamepad.button[9])
                {
                    Vector3 objVec = selectedObj.transform.position;

                    if (selectedObj.CompareTag("Item"))
                    {
                        Item i = selectedObj.GetComponent<Item>();
                        if (i.State == Item.ItemState.isDown)
                        {
                            objVec = transform.position + 4 * fwd;
                            objVec.y = selectedObj.transform.position.y;
                            i.targetPosition = objVec;
                        }
                    }
                    else if (selectedObj.CompareTag("Tower"))
                    {
                        Tower t = selectedObj.GetComponent<Tower>();
                        objVec = transform.position + PLAYER_PULL_DISTANCE * fwd;
                        objVec.y = selectedObj.transform.position.y;
                        t.targetPosition = objVec;
                    }
                }
            }



            if (gamepad.buttonUp[9] && selectedObj != null)
            {
                selectedObj = null;
            }

            transform.Rotate(Vector3.up, currentRotateSpeed * gamepad.rightStick.x);

            //Vector3 newPosition = transform.position;
            //Vector3 movementDelta = new Vector3(gamepad.leftStick.x * currentMovespeed, 0, gamepad.leftStick.y * currentMovespeed);
            Vector3 movementDelta = new Vector3(gamepad.leftStick.x * currentMovespeed, 0, gamepad.leftStick.y * currentMovespeed);
            //Debug.Log("MovementDelta: " + movementDelta.ToString());
            //TODO: Write some movement prediction math to smooth out player movement over network.
            //fraction = fraction + Time.deltaTime * 9;
            //Debug.Log("Fraction: " + fraction.ToString());
            //onUpdatePos += movementDelta;
            //Debug.Log("onUpdatePos: " + onUpdatePos.ToString());
            //transform.position = newPosition;
            if (movementDelta.normalized.magnitude > PLAYER_MOVEMENT_DEADZONE)
            {
                rigidbody.velocity = transform.localRotation * movementDelta;
            }
            else// if (movementDelta.Equals(Vector3.zero))
            {
                rigidbody.velocity = Vector3.zero;
            }


            //transform.position = onUpdatePos;//lerpVector;



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

    void attachPlayerToControllableGameObject(KBControllableGameObject newGameObject)
    {
        newGameObject.attachedPlayer = this;
        this.renderer.enabled = false;
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
    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("OnCollisionEnter!");
        KBControllableGameObject colControllablePlayerObject = collision.gameObject.GetComponent<KBControllableGameObject>();
        if (colControllablePlayerObject != null)
        {
            attachPlayerToControllableGameObject(colControllablePlayerObject);
        }
    }

    public override void takeDamage(int amount)
    {
        health -= amount;
    }


}
