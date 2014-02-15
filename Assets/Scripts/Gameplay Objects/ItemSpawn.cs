using KBConstants;
using UnityEngine;

/// <summary>
/// Spawns items when activating capture zone has been captured.
/// </summary>
public class ItemSpawn : MonoBehaviour
{
    public Item RedTeamItem;
    public Item BlueTeamItem;
    public Team controllingTeam;
    public bool waitingForSpawn;
    public CaptureZone connectedCaptureZone;
    public float timeUntilItemSpawn;
    private float lastSpawn;
    private GameObject spawnedItem;

    private void Start()
    {
        if (connectedCaptureZone == null)
        {
            Debug.LogWarning("ItemSpawn #" + gameObject.GetInstanceID() + " is missing a reference to its parent CaptureZone");
        }

        controllingTeam = Team.None;
        waitingForSpawn = true;
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
        spawnedItem = PhotonNetwork.Instantiate(
            ObjectConstants.PREFAB_NAMES[ObjectConstants.type.Item],
            new Vector3(
                transform.position.x,
                2.0f,
                transform.position.z),
            Quaternion.identity, 0);
        Item i = spawnedItem.GetComponent<Item>();
        if (i != null)
        {
            i.team = controllingTeam;
        }
        waitingForSpawn = false;
    }

    private void Update()
    {
        if (controllingTeam != Team.None)
        {
            if (spawnedItem == null && !waitingForSpawn)
            {
                waitingForSpawn = TriggerItemSpawn();
            }
            else if (waitingForSpawn && Time.time > lastSpawn + timeUntilItemSpawn)
            {
                SpawnItem();
            }
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
            Vector3 pos = transform.localPosition;
            Quaternion rot = transform.localRotation;

            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
        }
        else
        {
            // Receive latest state information
            Vector3 pos = Vector3.zero;
            Quaternion rot = Quaternion.identity;

            stream.Serialize(ref pos);
            stream.Serialize(ref rot);

            //latestCorrectPos = pos;                 // save this to move towards it in FixedUpdate()
            //onUpdatePos = transform.localPosition;  // we interpolate from here to latestCorrectPos
            //fraction = 0;                           // reset the fraction we alreay moved. see Update()

            transform.position = pos;
            transform.localRotation = rot;          // this sample doesn't smooth rotation
        }
    }
}