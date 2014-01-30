using KBConstants;
using UnityEngine;

/// <summary>
/// Our model object for game items. This should be extended by every game item in the game.
/// </summary>
public class Item : KBGameObject
{
    public ItemType itemType;
    public float respawnTime;
    private float respawnStart;
    public enum ItemState { isDown, isPickedUp, respawning };

    public Team team;
    private ItemState state;

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

    private Vector3 targetScale;

    /// <summary>
    /// Use this for initialization
    /// </summary>
    private void Start()
    {
        base.Start();
        gameObject.tag = "Item";

        transform.position = new Vector3(transform.position.x, 2.0f, transform.position.z);

        //targetPosition = transform.position;
        targetScale = Vector3.one;
        int r = Random.Range(0, 100); // TODO : Need a way of creating items without random types

        int legendaryChance = 5;
        int rareChance = 15;
        int uncommonChance = 30;
        int commonChance = 100 - legendaryChance - rareChance - uncommonChance;

        if (r < legendaryChance)
        {
            setItemType(ItemType.legendary);
        }
        else if (r >= legendaryChance && r < legendaryChance + rareChance)
        {
            setItemType(ItemType.rare);
        }
        else if (r >= legendaryChance + rareChance && r < legendaryChance + rareChance + uncommonChance)
        {
            setItemType(ItemType.uncommon);
        }
        else
        {
            setItemType(ItemType.common);
        }

        transform.Rotate(Vector3.forward, 45);
        transform.Rotate(Vector3.right, 33.3f);
    }

    public void setItemType(ItemType type)
    {
        itemType = type;
        switch (type)
        {
            case ItemType.common:
                renderer.material = Resources.Load<Material>(MaterialConstants.MATERIAL_NAMES[MaterialConstants.type.ItemCommon]);
                break;

            case ItemType.uncommon:
                renderer.material = Resources.Load<Material>(MaterialConstants.MATERIAL_NAMES[MaterialConstants.type.ItemUncommon]);
                break;

            case ItemType.rare:
                renderer.material = Resources.Load<Material>(MaterialConstants.MATERIAL_NAMES[MaterialConstants.type.ItemRare]);
                break;

            case ItemType.legendary:
                renderer.material = Resources.Load<Material>(MaterialConstants.MATERIAL_NAMES[MaterialConstants.type.ItemLegendary]);
                break;

            case ItemType.undefined:
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        //transform.position = Vector3.Lerp(transform.position, targetPosition, 5.0f * Time.deltaTime);
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, 1.0f * Time.deltaTime);

        switch (state)
        {
            case ItemState.isDown:
                canPickup = true;
                particleSystem.enableEmission = false;
                break;

            case ItemState.isPickedUp:
                particleSystem.enableEmission = false;
                break;

            case ItemState.respawning:
                if (Time.time > respawnStart + respawnTime)
                {
                    Destroy(gameObject);
                }
                particleSystem.enableEmission = true;
                break;

            default:
                break;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
    }

    private void OnTriggerEnter(Collider other)
    {
    }

    public void StartGrowAnimation()
    {
        targetScale = new Vector3(3.0f, 3.0f, 3.0f);
    }

    public void Respawn()
    {
        state = ItemState.respawning;
        respawnStart = Time.time;
    }
}