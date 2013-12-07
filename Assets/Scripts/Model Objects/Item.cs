using UnityEngine;
using System.Collections;
using KBConstants;

/// <summary>
/// Our model object for game items. This should be extended by every game item in the game.
/// </summary>
public class Item : KBGameObject
{

    public enum ItemState { isDown, isPickedUp, isInFactory };
    ItemState state;
    public ItemState State
    {
        set
        {
            state = value;
        }
        get
        {
            return state;
        }
    }
    private bool canPickup;
    public bool CanPickup
    {
        get
        {
            return canPickup;
        }
    }
    public Vector3 targetPosition;

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Start()
    {
        targetPosition = transform.position;
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, 5.0f * Time.deltaTime);
        
        switch (state)
        {
            case ItemState.isDown:
                canPickup = true;
                break;
            case ItemState.isPickedUp:
                break;
            case ItemState.isInFactory:
                canPickup = false;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
    
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Factory"))
        {
            State = ItemState.isInFactory;
        }
    }
}
