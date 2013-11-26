using UnityEngine;
using System.Collections;

public class GoalZone : MonoBehaviour
{

    public float redTeamTime, blueTeamTime;

    void Start()
    {
        redTeamTime = 0;
        blueTeamTime = 0;
    }

    void Update()
    {

    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
           Player p = other.gameObject.GetComponent<Player>();
           switch (p.team)
           {
               case KBControllableGameObject.Team.red:
                   redTeamTime += Time.deltaTime;
                   break;
               case KBControllableGameObject.Team.blue:
                   blueTeamTime += Time.deltaTime;
                   break;
               case KBControllableGameObject.Team.none:
                   break;
               default:
                   break;
           }
        }
    }
}
