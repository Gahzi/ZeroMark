using KBConstants;
using UnityEngine;

/// <summary>
/// Spawns items when activating capture zone has been captured.
/// </summary>
public class ItemSpawn : KBGameObject
{
    public Team controllingTeam;
    public bool waitingForSpawn;
    public CaptureZone connectedCaptureZone;
    public float timeUntilItemSpawn;
    private float lastSpawn;
    private bool spawnedItem;

    public override void Start()
    {
        base.Start();
        if (connectedCaptureZone == null)
        {
            Debug.LogWarning("ItemSpawn #" + gameObject.GetInstanceID() + " is missing a reference to its parent CaptureZone");
        }

        controllingTeam = Team.None;
        waitingForSpawn = true;
        spawnedItem = false;
    }

    private void OnDrawGizmos()
    {
        Vector3 p = new Vector3(transform.position.x, 5, transform.position.z);
        switch (controllingTeam)
        {
            case Team.Red:
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(p + Vector3.left, 1);
                break;

            case Team.Blue:
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(p + Vector3.right, 1);
                break;

            case Team.None:
                Gizmos.color = Color.grey;
                Gizmos.DrawSphere(p, 1);
                break;

            default:
                break;
        }
    }

    public void SpawnItem()
    {
        //createObject(ObjectConstants.type.Player, new Vector3(0, 0, 0), Quaternion.identity, Team.Red).GetComponent<PlayerLocal>(); ;
        GameManager.Instance.createObject(ObjectConstants.type.Item, new Vector3(
                transform.position.x,
                2.0f,
                transform.position.z),Quaternion.identity,controllingTeam);
            
        waitingForSpawn = false;
        spawnedItem = true;
    }

    private void Update()
    {
        if (controllingTeam != Team.None)
        {
            if (!spawnedItem && !waitingForSpawn)
            {
                waitingForSpawn = TriggerItemSpawn();
            }
            else if (waitingForSpawn && Time.time > lastSpawn + timeUntilItemSpawn)
            {
                SpawnItem();
            }
        }
        if (!waitingForSpawn)
        {
            lastSpawn = Time.time;
        }
    }

    public void ReceiveActivationEvent(Team t)
    {
        switch (t)
        {
            case Team.Red:
                controllingTeam = t;
                TriggerItemSpawn();
                break;

            case Team.Blue:
                controllingTeam = t;
                TriggerItemSpawn();
                break;

            case Team.None:
                controllingTeam = t;
                waitingForSpawn = false;
                break;

            default:
                break;
        }
    }

    private bool TriggerItemSpawn()
    {
        lastSpawn = Time.time;
        return true;
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
            int cntrlTm = (int)controllingTeam;
            bool wtngFrSpwn = waitingForSpawn;
            float tmUntlItmSpawn = timeUntilItemSpawn;
            float lstSpwn = lastSpawn;
            bool spwnedItm = spawnedItem;

            stream.Serialize(ref cntrlTm);
            stream.Serialize(ref wtngFrSpwn);
            stream.Serialize(ref tmUntlItmSpawn);
            stream.Serialize(ref lstSpwn);
            stream.Serialize(ref spwnedItm);
        }
        else
        {
            // Receive latest state information
            int cntrlTm = (int)controllingTeam;
            bool wtngFrSpwn = waitingForSpawn;
            float tmUntlItmSpawn = timeUntilItemSpawn;
            float lstSpwn = lastSpawn;
            bool spwnedItm = spawnedItem;

            stream.Serialize(ref cntrlTm);
            stream.Serialize(ref wtngFrSpwn);
            stream.Serialize(ref tmUntlItmSpawn);
            stream.Serialize(ref lstSpwn);
            stream.Serialize(ref spwnedItm);



            //latestCorrectPos = pos;                 // save this to move towards it in FixedUpdate()
            //onUpdatePos = transform.localPosition;  // we interpolate from here to latestCorrectPos
            //fraction = 0;                           // reset the fraction we alreay moved. see Update()

            controllingTeam = (Team)cntrlTm;
            waitingForSpawn = wtngFrSpwn;
            timeUntilItemSpawn = tmUntlItmSpawn;
            lastSpawn = lstSpwn;
            spawnedItem = spwnedItm;
        }
    }
}