using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]

/// <summary>
/// 
/// </summary>
public class Kaiju : KBControllableGameObject
{
    public static float KAIJU_MOVEMENT_SPEED = 50f;

    #region CONSTANTS
    private enum CoreNames { red, green, blue, black };
    private enum State { idle, dead };
    private static int CORE_HEALTH_DEFAULT = 100;
    private static int CORE_HEALTH_MAXIMUM_DEFAULT = 100;
    #endregion

    private int[] coreHealth; // red, green, blue, black
    private State currentState;

    void Start()
    {
        currentState = State.idle;
        coreHealth = new int[4];

        for (int i = 0; i < coreHealth.Length; i++)
        {
            coreHealth[i] = CORE_HEALTH_DEFAULT;
        }
    }

    void Update()
    {

        switch (currentState)
        {
            case State.idle:
                break;
            case State.dead:
                Debug.Log("Kaiju named: " + gameObject.name.ToString() + "is dead!");
                break;
            default:
                break;
        }

        if (isPlayerAttached())
        {
            Vector3 movementDelta = new Vector3(attachedPlayer.gamepad.leftStick.x * KAIJU_MOVEMENT_SPEED, 0, attachedPlayer.gamepad.leftStick.y * KAIJU_MOVEMENT_SPEED);
            //Debug.Log("MovementDelta: " + movementDelta.ToString());
            //TODO: Write some movement prediction math to smooth out player movement over network.
            //fraction = fraction + Time.deltaTime * 9;
            //Debug.Log("Fraction: " + fraction.ToString());
            //onUpdatePos += movementDelta;
            //Debug.Log("onUpdatePos: " + onUpdatePos.ToString());
            //transform.position = newPosition;
            if (movementDelta.Equals(Vector3.zero))
            {
                rigidbody.velocity = Vector3.zero;
            }
            else
            {
                rigidbody.velocity = movementDelta;
                //rigidbody.AddForce(movementDelta, ForceMode.VelocityChange);
            }
        }

    }

    #region COLLISION & TRIGGERS
    void OnTriggerEnter(Collider other)
    {
    }

    void OnCollisionEnter(Collision collision)
    {
    }

    #endregion

    #region MOVEMENT
    void move() { }
    #endregion

    #region CORE MANAGEMENT
    void damageCore(int which, int amount)
    {
        modifyCoreHealth(which, amount);
        // Additional code, e.g. activating animations
    }

    void healCore(int which, int amount)
    {
        modifyCoreHealth(which, amount);
        // Additional code, e.g. activating animations
    }

    /// <summary>
    /// Modifies the health of the indicated core by amount
    /// </summary>
    /// <param name="which">The core to modify, can pass CoreName as int</param>
    /// <param name="amount"></param>
    void modifyCoreHealth(int which, int amount)
    {
        coreHealth[which] -= amount;

        if (coreHealth[which] < 0)
        {
            coreHealth[which] = 0;
        }
        else if (coreHealth[which] > CORE_HEALTH_MAXIMUM_DEFAULT)
        {
            coreHealth[which] = CORE_HEALTH_MAXIMUM_DEFAULT;
        }
    }
    #endregion

    #region ABILITIES
    void activateAbilityOne() { }
    void activateAbilityTwo() { }
    void activateAbilityThree() { }
    void activateAbilityFour() { }
    #endregion

}
