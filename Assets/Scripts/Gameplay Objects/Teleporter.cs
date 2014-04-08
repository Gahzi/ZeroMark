using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public GameObject linkedTeleporter;

    private void OnDrawGizmosSelected()
    {
        if (linkedTeleporter != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, linkedTeleporter.transform.position);
        }
    }
}