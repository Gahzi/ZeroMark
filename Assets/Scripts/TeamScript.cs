using UnityEngine;
using System.Collections;
using KBConstants;

public class TeamScript : MonoBehaviour
{

    public Team team;
    public Team Team
    {
        get
        {
            return team;
        }
        set
        {
            team = value;
        }
    }

}
