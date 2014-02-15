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
    public enum ItemState { isDown = 0, isPickedUp = 1, respawning = 2, disabled = 3 };
    private ItemState _state;
    RotatableGuiItem rGui;
    public float disableTime;
    private float disableTimer = 0.25f;
    public ItemState state
    {
        set
        {
            _state = value;
        }
        get
        {
            return _state;
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
        _state = ItemState.isDown;
        transform.position = new Vector3(transform.position.x, 2.0f, transform.position.z);

        //targetPosition = transform.position;
        targetScale = Vector3.one;
        GenerateType();

        transform.Rotate(Vector3.forward, 45);
        transform.Rotate(Vector3.right, 33.3f);

        rGui = GetComponent<RotatableGuiItem>();
        rGui.ScreenpointToAlign = RotatableGuiItem.AlignmentScreenpoint.BottomLeft;
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

    public void GenerateType()
    {
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
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        //transform.position = Vector3.Lerp(transform.position, targetPosition, 5.0f * Time.deltaTime);
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, 1.0f * Time.deltaTime);

        switch (_state)
        {
            case ItemState.isDown:
                if (transform.position.y > 2.0f)
                {
                    transform.position = new Vector3(transform.position.x, 2.0f, transform.position.z);
                }

                particleSystem.enableEmission = false;
                rGui.enabled = true;

                rGui.angle = Mathf.Sin(Time.time / 2.0f) * 360.0f;
                rGui.size = new Vector2((Mathf.Sin(Time.time) + 1) * 16.0f + 16.0f, (Mathf.Sin(Time.time) + 1) * 16.0f + 16.0f);
                Vector3 sPos = Camera.main.WorldToScreenPoint(transform.position);
                rGui.relativePosition = new Vector2(sPos.x, -sPos.y);
                break;

            case ItemState.isPickedUp:
                particleSystem.enableEmission = false;
                rGui.enabled = false;
                break;

            case ItemState.respawning:
                if (Time.time > respawnStart + respawnTime)
                {
                    //SHERVIN: Can't destroy a photon game object that you don't own.
                    if (photonView.isMine)
                    {
                        PhotonNetwork.Destroy(gameObject);
                    }
                }
                particleSystem.enableEmission = true;
                rGui.enabled = false;
                break;

            case ItemState.disabled:
                if (Time.time > disableTime + disableTimer)
                {
                    _state = ItemState.isDown;
                }
                break;

            default:
                break;
        }

    }

    /// <summary>
    /// While script is observed (in a PhotonView), this is called by PUN with a stream to write or read.
    /// </summary>
    /// <remarks>
    /// The property stream.isWriting is true for the owner of a PhotonView. This is the only client that
    /// should write into the stream. Others will receive the content written by the owner and can read it.
    ///
    /// Note: Send only what you actually want to consume/use, too!
    /// Note: If the owner doesn't write something into the stream, PUN won't send anything.
    /// </remarks>
    /// <param name="stream">Read or write stream to pass state of this GameObject (or whatever else).</param>
    /// <param name="info">Some info about the sender of this stream, who is the owner of this PhotonView (and GameObject).</param>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            Vector3 pos = transform.localPosition;
            Quaternion rot = transform.rotation;
            int hlth = health;
            //ItemType itmType = itemType;
            int itmType = (int)itemType;
            float rspwnTime = respawnTime;
            float rspwnStrt = respawnStart;
            int st = (int)state;
            float dsbleTime = disableTime;

            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
            stream.Serialize(ref hlth);
            stream.Serialize(ref itmType);
            stream.Serialize(ref rspwnTime);
            stream.Serialize(ref rspwnStrt);
            stream.Serialize(ref st);
            stream.Serialize(ref dsbleTime);
        }
        else
        {
            // Receive latest state information
            Vector3 pos = Vector3.zero;
            Quaternion rot = Quaternion.identity;
            int hlth = 0;
            int itmType = 0;
            float rspwnTime = 0;
            float rspwnStrt = 0;
            int st = 0;
            float dsbleTime = 0;

            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
            stream.Serialize(ref hlth);
            stream.Serialize(ref itmType);
            stream.Serialize(ref rspwnTime);
            stream.Serialize(ref rspwnStrt);
            stream.Serialize(ref st);
            stream.Serialize(ref dsbleTime);

            transform.position = pos;
            transform.rotation = rot;          // this sample doesn't smooth rotation
            health = hlth;
            itemType = (ItemType)itmType;
            respawnTime = rspwnTime;
            respawnStart = rspwnStrt;
            state = (ItemState)st;
            disableTime = dsbleTime;
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
        _state = ItemState.respawning;
        respawnStart = Time.time;
    }
}