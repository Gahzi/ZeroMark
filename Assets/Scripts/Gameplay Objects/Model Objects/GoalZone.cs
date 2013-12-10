using UnityEngine;
using System.Collections;
using KBConstants;

public class GoalZone : MonoBehaviour
{
    static int RED_TEAM_CAPTURE_COUNT = 5000;
    static int BLUE_TEAM_CAPTURE_COUNT = -5000;
    
    static int RED_TEAM_CAPTURE_RATE = 1;
    static int BLUE_TEAM_CAPTURE_RATE = -1;

    public enum GoalState { NotCaptured, RedCaptured, BlueCaptured };
    public GoalState state = GoalState.NotCaptured;

    int redTowerCount = 0;
    int blueTowerCount = 0;

    public int goalScore = 0;

    void Start()
    {
        goalScore = 0;
        state = GoalState.NotCaptured;
    }

    void Update()
    {
        if (state == GoalState.NotCaptured)
        {
            if (redTowerCount > 0 || blueTowerCount > 0) goalScore += (redTowerCount * RED_TEAM_CAPTURE_RATE) + (blueTowerCount * BLUE_TEAM_CAPTURE_RATE);
            else
            {
                if (goalScore > 0) goalScore -= 1;
                else if (goalScore < 0) goalScore += 1;
            }
        }

        CheckCaptured();
    }

    void CheckCaptured()
    {
        if (goalScore >= RED_TEAM_CAPTURE_COUNT)
        {
            state = GoalState.RedCaptured;
        }
        else if (goalScore <= BLUE_TEAM_CAPTURE_COUNT)
        {
            state = GoalState.BlueCaptured;
        }
    }

    /*
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player p = other.gameObject.GetComponent<Player>();
            switch (p.team)
            {
                case KBControllableGameObject.Team.Red:
                    {
                        if (state == GoalState.NotOccupied)
                        {
                            state = GoalState.RedOccupied;
                        }
                        else if (state == GoalState.BlueOccupied)
                        {
                            state = GoalState.Contested;
                        }
                        break;
                    }
                case KBControllableGameObject.Team.Blue:
                    {
                        if (state == GoalState.NotOccupied)
                        {
                            state = GoalState.BlueOccupied;
                        }
                        else if (state == GoalState.RedOccupied)
                        {
                            state = GoalState.Contested;
                        }
                        break;
                    }
            }
        }
    }
     * */

    void OnTriggerEnter(Collider other)
    {
         if (other.gameObject.CompareTag("Tower"))
        {
            Tower p = other.gameObject.GetComponent<Tower>();
            switch (p.Team)
            {
                case Team.Red:
                    {
                        redTowerCount += 1;
                        break;
                    }
                case Team.Blue:
                    {
                        blueTowerCount += 1;
                        break;
                    }
            }
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Tower"))
        {
            Tower p = other.gameObject.GetComponent<Tower>();
            switch (p.Team)
            {
                case Team.Red:
                    {
                        redTowerCount -= 1;
                        break;
                    
                    }
                case Team.Blue:
                    {
                        blueTowerCount -= 1;
                        break;
                    }
            }
        }
    }

    void IncrementGoalZoneScore(Team team)
    {
        switch (team)
        {
            case KBConstants.Team.Red:
                goalScore += 1;
                break;
            case KBConstants.Team.Blue:
                goalScore -= 1;
                break;
            case KBConstants.Team.None:
                break;
            default:
                break;
        }
    }
}
