using UnityEngine;
using System.Collections;
using KBConstants;

/// <summary>
/// Our model object for game items. This should be extended by every game item in the game.
/// </summary>
public class Item : KBGameObject
{
    public ItemType itemType;
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
        int r = Random.Range(1, 4);
        switch (r)
        {
            case 1:
                itemType = ItemType.one;
                renderer.material = Resources.Load<Material>(MaterialConstants.MATERIAL_NAMES[MaterialConstants.type.ItemOneMat]);
                break;
            case 2:
                itemType = ItemType.two;
                renderer.material = Resources.Load<Material>(MaterialConstants.MATERIAL_NAMES[MaterialConstants.type.ItemTwoMat]);
                break;
            case 3:
                itemType = ItemType.three;
                renderer.material = Resources.Load<Material>(MaterialConstants.MATERIAL_NAMES[MaterialConstants.type.ItemThreeMat]);
                break;
            default:
                break;
        }
        transform.Rotate(Vector3.forward, 45);
        transform.Rotate(Vector3.right, 33.3f);
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
