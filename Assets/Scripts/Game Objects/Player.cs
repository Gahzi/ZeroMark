using UnityEngine;
using System.Collections;
using KBConstants;

[RequireComponent(typeof(GamepadInfo))]

public class Player : KBGameObject {
	
	private GamepadInfo gamepad;
	
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
            Debug.Log(gamepad.leftStick.ToString());
            Vector3 newPosition = transform.position;
            Vector3 movementDelta = new Vector3(gamepad.leftStick.x * PlayerConstants.PLAYER_MOVEMENT_SPEED, gamepad.leftStick.y * PlayerConstants.PLAYER_MOVEMENT_SPEED, 0);

            newPosition += movementDelta;
            transform.position = newPosition;
        }
        
        
	}   
}
