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
    public bool isActive;
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
        isActive = false;
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
        spawnedItem = PhotonNetwork.Instantiate(
            ObjectConstants.PREFAB_NAMES[ObjectConstants.type.Item],
            new Vector3(
                transform.position.x,
                2.0f,
                transform.position.z),
            Quaternion.identity, 0);
        Item i = spawnedItem.GetComponent<Item>();
        i.team = controllingTeam;
        lastSpawn = Time.time;
        isActive = false;
    }

    private void Update()
    {
        if (isActive)
        {
            if (Time.time > lastSpawn + timeUntilItemSpawn)
            {
                SpawnItem();
            }
        }
        if (controllingTeam != Team.None)
        {
            if (spawnedItem == null)
            {
                InitiateItemSpawn();
            }
        }
    }

    public void ReceiveActivationEvent(Team t)
    {
        switch (t)
        {
            case Team.Red:
                controllingTeam = t;
                InitiateItemSpawn();
                break;

            case Team.Blue:
                controllingTeam = t;
                InitiateItemSpawn();
                break;

            case Team.None:
                controllingTeam = t;
                isActive = false;
                break;

            default:
                break;
        }
    }

    private void InitiateItemSpawn()
    {
        isActive = true;
    }
}