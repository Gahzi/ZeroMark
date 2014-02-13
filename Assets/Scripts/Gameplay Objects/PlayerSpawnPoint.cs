using UnityEngine;

public class PlayerSpawnPoint : KBGameObject
{

    private void Start()
    {
    }

    private void Update()
    {
    }

    private void OnDrawGizmos()
    {
        Vector3 p = new Vector3(transform.position.x, 5, transform.position.z);

        switch (team)
        {
            case KBConstants.Team.Red:
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(p, Vector3.one * 10);
                break;
            case KBConstants.Team.Blue:
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(p, Vector3.one * 10);
                break;
            case KBConstants.Team.None:
                break;
            default:
                break;
        }
    }
}