using UnityEngine;
using System.Collections.Generic;


[RequireComponent(typeof(CharacterController))]

public class Digger : KBControllableGameObject {

    public static float DIGGER_MOVEMENT_SPEED = 0.5f;

    public List<Item> itemInventory;

    private Vector3 latestCorrectPos;
    private Vector3 onUpdatePos;
    private float fraction;
    

    /// <summary>
    /// Use this for initialization
    /// </summary>
	void Start () 
    {
        itemInventory = new List<Item>();
	}
	
	/// <summary>
    /// Update is called once per frame
	/// </summary>
    void Update () 
    {
        if (isPlayerAttached())
        {
            //Vector3 newPosition = transform.position;
            Vector3 movementDelta = new Vector3(attachedPlayer.gamepad.leftStick.x * DIGGER_MOVEMENT_SPEED, 0, attachedPlayer.gamepad.leftStick.y * DIGGER_MOVEMENT_SPEED);


            //TODO: Write some movement prediction math to smooth out player movement over network.
            fraction = fraction + Time.deltaTime * 9;
            onUpdatePos += movementDelta;
            //transform.position = newPosition;

            transform.position = onUpdatePos;//lerpVector;
        }
	
	}

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        KBControllableGameObject colControllablePlayerObject = hit.collider.gameObject.GetComponent<KBControllableGameObject>();
        if (colControllablePlayerObject != null)
        {
            //attachPlayerToControllableGameObject(colControllablePlayerObject);
        }
    }


    /// <summary>
    /// Collision checking method for digger
    /// </summary>
    /// <param name="collision"> The object that is generated during a collision</param>
    void OnCollisionEnter(Collision collision)
    {
        Player colPlayer = collision.gameObject.GetComponent<Player>();
        //if(colPlayer !=DIGGER_MOVEMENT_SPEED)
    }



}
