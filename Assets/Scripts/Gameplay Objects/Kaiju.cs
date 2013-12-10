﻿using UnityEngine;
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
    private static int CORE_ENERGY_DEFAULT = 100;
    private static int CORE_ENERGY_MAXIMUM_DEFAULT = 100;
    #endregion

    private int[] coreEnergy; // red, green, blue, black
    private State currentState;

    void Start()
    {
        currentState = State.idle;
        coreEnergy = new int[4];

        for (int i = 0; i < coreEnergy.Length; i++)
        {
            coreEnergy[i] = CORE_ENERGY_DEFAULT;
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
    
    // OnGUI is called for rendering and handling GUI events
    void OnGUI()
    {
        if (Event.current.isKey && Event.current.type == EventType.keyDown)
        {
            switch (Event.current.keyCode)
            {
                case KeyCode.A:
                    coreGetEnergy(0);
                    break;
                case KeyCode.Z:
                    coreDamage(0, 10);
                    break;
                default:
                    break;
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
    void coreDamage(int which, int amount)
    {
        modifyCoreEnergy(which, amount);
        // Additional code, e.g. activating animations
    }

    void coreHeal(int which, int amount)
    {
        modifyCoreEnergy(which, amount);
        // Additional code, e.g. activating animations
    }

    int coreGetEnergy(int which)
    {
        Debug.Log("[" + System.DateTime.Now.Hour.ToString() + ":" + System.DateTime.Now.Minute.ToString() + ":" + System.DateTime.Now.Second.ToString() + "] " + "Kaiju#" + gameObject.GetInstanceID() + " core " + which.ToString() + " has " + coreEnergy[which].ToString() + " Energy.");
        return coreEnergy[which];
    }
    /// <summary>
    /// Modifies the Energy of the indicated core by amount
    /// </summary>
    /// <param name="which">The core to modify, can pass CoreName as int</param>
    /// <param name="amount"></param>
    void modifyCoreEnergy(int which, int amount)
    {
        coreEnergy[which] -= amount;

        if (coreEnergy[which] < 0)
        {
            coreEnergy[which] = 0;
        }
        else if (coreEnergy[which] > CORE_ENERGY_MAXIMUM_DEFAULT)
        {
            coreEnergy[which] = CORE_ENERGY_MAXIMUM_DEFAULT;
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