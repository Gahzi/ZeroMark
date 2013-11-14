using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]

/// <summary>
/// 
/// </summary>
public class Kaiju : KBControllableGameObject
{

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
                Debug.Log("Kaiju #" + gameObject.GetInstanceID().ToString() + "is dead!");
                break;
            default:
                break;
        }

        if (isPlayerAttached())
        {

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
