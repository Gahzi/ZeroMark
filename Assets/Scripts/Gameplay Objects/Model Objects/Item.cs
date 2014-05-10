using KBConstants;
using UnityEngine;

/// <summary>
/// Our model object for game items. This should be extended by every game item in the game.
/// </summary>
public class Item : KBGameObject
{
    public float respawnTime;
    private float respawnStart;
    public enum ItemState { isDown = 0, isPickedUp = 1, respawning = 2, disabled = 3 };
    private ItemState _state;
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
    public override void Start()
    {
        base.Start();
        gameObject.tag = "Item";
        _state = ItemState.isDown;
        transform.position = new Vector3(transform.position.x, 2.0f, transform.position.z);

        //targetPosition = transform.position;
        targetScale = Vector3.one;

        transform.Rotate(Vector3.forward, 45);
        transform.Rotate(Vector3.right, 33.3f);

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
                break;

            case ItemState.isPickedUp:
                particleSystem.enableEmission = false;
                break;

            case ItemState.respawning:
                if (Time.time > respawnStart + respawnTime)
                {
                    //SHERVIN: Can't destroy a photon game object that you don't own.
                    if (photonView.isMine)
                    {
                        // TODO : This should call ObjectPool.Recycle
                        PhotonNetwork.Destroy(gameObject);
                    }
                }
                particleSystem.enableEmission = true;
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
            float rspwnTime = respawnTime;
            float rspwnStrt = respawnStart;
            int st = (int)state;
            float dsbleTime = disableTime;
            int tm = (int)team;

            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
            stream.Serialize(ref hlth);
            stream.Serialize(ref rspwnTime);
            stream.Serialize(ref rspwnStrt);
            stream.Serialize(ref st);
            stream.Serialize(ref dsbleTime);
            stream.Serialize(ref tm);
        }
        else
        {
            // Receive latest state information
            Vector3 pos = Vector3.zero;
            Quaternion rot = Quaternion.identity;
            int hlth = 0;
            float rspwnTime = 0;
            float rspwnStrt = 0;
            int st = 0;
            float dsbleTime = 0;
            int tm = 0;

            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
            stream.Serialize(ref hlth);
            stream.Serialize(ref rspwnTime);
            stream.Serialize(ref rspwnStrt);
            stream.Serialize(ref st);
            stream.Serialize(ref dsbleTime);
            stream.Serialize(ref tm);

            transform.position = pos;
            transform.rotation = rot;          // this sample doesn't smooth rotation
            health = hlth;
            respawnTime = rspwnTime;
            respawnStart = rspwnStrt;
            state = (ItemState)st;
            disableTime = dsbleTime;
            team = (Team)tm;
        }
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