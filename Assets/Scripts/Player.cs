using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GamepadInfo))]

public class Player : KBGameObject {
	
	private GamepadInfo gamepad;
	
	// Use this for initialization
	void Start () 
	{
		GamepadInfoHandler gamepadHandler = GamepadInfoHandler.Instance;
		gamepadHandler.AttachControllerToPlayer(this);
	}
	
	public void SetGamepad(GamepadInfo newGamepad)
	{
		gamepad = newGamepad;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
