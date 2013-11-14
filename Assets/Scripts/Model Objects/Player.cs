using UnityEngine;
using System.Collections;
using KBConstants;

[RequireComponent(typeof(GamepadInfo))]
[RequireComponent(typeof(PhotonView))]

public class Player : KBGameObject {
	
	public GamepadInfo gamepad;
    private Vector3 latestCorrectPos;
    private Vector3 onUpdatePos;
    private float fraction;
    
    protected PhotonView photonView;
	
	// Use this for initialization
	void Start () 
	{
		GamepadInfoHandler gamepadHandler = GamepadInfoHandler.Instance;
        Debug.Log("Attempting to attach Controller");
		gamepadHandler.AttachControllerToPlayer(this);

        GameObject newCameraObject = (GameObject)GameObject.Instantiate(Resources.Load(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.Camera]), Vector3.zero, Quaternion.identity);
        KBCamera cameraScript = newCameraObject.GetComponent<KBCamera>();
        cameraScript.attachedPlayer = this;

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
	
	// Update is called once per frame
	void Update () 
	{
        if (gamepad != null)
        {
           
            //Vector3 newPosition = transform.position;
            Vector3 movementDelta = new Vector3(gamepad.leftStick.x * PlayerConstants.PLAYER_MOVEMENT_SPEED, 0, gamepad.leftStick.y * PlayerConstants.PLAYER_MOVEMENT_SPEED);


            //TODO: Write some movement prediction math to smooth out player movement over network.
            fraction = fraction + Time.deltaTime * 9;
            onUpdatePos += movementDelta;
            //transform.position = newPosition;
            Vector3 lerpVector = Vector3.Lerp(onUpdatePos, latestCorrectPos, fraction);
            
            transform.position = onUpdatePos;//lerpVector;
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


}
