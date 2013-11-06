using UnityEngine;
using System.Collections;
using KBConstants;

[RequireComponent(typeof(GamepadInfo))]
public class Player : KBGameObject {
	
	private GamepadInfo gamepad;
    private Vector3 latestCorrectPos;
    private Vector3 onUpdatePos;
    private float fraction;
	
	// Use this for initialization
	void Start () 
	{
		GamepadInfoHandler gamepadHandler = GamepadInfoHandler.Instance;
        Debug.Log("Attempting to attach Controller");
		gamepadHandler.AttachControllerToPlayer(this);
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
            Vector3 movementDelta = new Vector3(gamepad.leftStick.x * PlayerConstants.PLAYER_MOVEMENT_SPEED, gamepad.leftStick.y * PlayerConstants.PLAYER_MOVEMENT_SPEED, 0);

            fraction = fraction + Time.deltaTime * 9;
            onUpdatePos += movementDelta;
            //transform.position = newPosition;
            Vector3 lerpVector = Vector3.Lerp(onUpdatePos, latestCorrectPos, fraction);
            Debug.Log(lerpVector.ToString());
            transform.position = onUpdatePos;
        }
	}

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
 	    base.OnPhotonSerializeView(stream, info);
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
