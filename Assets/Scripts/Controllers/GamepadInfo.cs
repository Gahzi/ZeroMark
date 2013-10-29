using UnityEngine;
using System.Collections;

public class GamepadInfo : MonoBehaviour
{

    /// <summary>
    /// Helper class containing gamepad data.
    /// Vectors for controller axes and array for button states.
    /// Includes booleans that allows button-like trigger behavior
    /// Data sent from GamepadInputHandler.
    /// </summary>
    /// 

    // These member variables can be made private, but are set
    // public so we can debug the gamepads easily

    // TODO: Separate triggers in input manager

    public int gamepadNumber;
    public Vector2 leftStick;
    public Vector2 rightStick;
    public float leftTrigger;
	public float rightTrigger;
    public Vector2 dpad;
    public bool[] button; // 0.A 1.B 2.X 3.Y 4.LS 5.RS 6.Back 7.Start 8.LB 9.RB
    public bool[] buttonDown;
    public bool[] buttonUp;
	/*
    public bool triggerReleased;
    public bool leftTrigger; // left trigger is > 0
    public bool rightTrigger; // right trigger is < 0
    public bool leftTriggerDown;
    public bool rightTriggerDown;
    public bool leftTriggerUp;
    public bool rightTriggerUp;

    private static float triggerDeadzone = 0.05f;
	*/
    void Start()
    {
        button = new bool[10];
		buttonDown = new bool[10];
		buttonUp = new bool[10];
		leftStick = new Vector2(0, 0);
        rightStick = new Vector2(0, 0);
        leftTrigger = 0;
		rightTrigger = 0;
        dpad = new Vector2(0, 0);
		
        for (int i = 0; i < 9; i++)
        {
            button[i] = false;
            buttonDown[i] = false;
            buttonUp[i] = false;
        }
    }

    void Update()
    {
    }

    public void SetData(int _gamepadNumber, Vector2 _leftStick, Vector2 _rightStick, float _leftTrigger,float _rightTrigger, Vector2 _dpad, bool[] _button, bool[] _buttonDown, bool[] _buttonUp)
    {
        gamepadNumber = _gamepadNumber;
        leftStick = _leftStick;
        rightStick = _rightStick;
        dpad = _dpad;
        leftTrigger = _leftTrigger;
		rightTrigger = _rightTrigger;
        button = _button;
        buttonDown = _buttonDown;
        buttonUp = _buttonUp;
    }
}
