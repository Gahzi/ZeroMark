using UnityEngine;
using System.Collections;

public class GoalZone : MonoBehaviour
{
    static int RED_TEAM_CAPTURE_COUNT = 5000;
    static int BLUE_TEAM_CAPTURE_COUNT = -5000;
    
    static int RED_TEAM_CAPTURE_RATE = 1;
    static int BLUE_TEAM_CAPTURE_RATE = -1;

    public enum GoalState { NotCaptured, RedCaptured, BlueCaptured };
    public GoalState state = GoalState.NotCaptured;

    int redPlayerCount = 0;
    int bluePlayerCount = 0;

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
            if (redPlayerCount > 0 || bluePlayerCount > 0) goalScore += (redPlayerCount * RED_TEAM_CAPTURE_RATE) + (bluePlayerCount * BLUE_TEAM_CAPTURE_RATE);
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
         if (other.gameObject.CompareTag("Player"))
        {
            Player p = other.gameObject.GetComponent<Player>();
            switch (p.team)
            {
                case KBControllableGameObject.Team.Red:
                    {
                        redPlayerCount += 1;
                        break;
                    }
                case KBControllableGameObject.Team.Blue:
                    {
                        bluePlayerCount += 1;
                        break;
                    }
            }
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player p = other.gameObject.GetComponent<Player>();
            switch (p.team)
            {
                case KBControllableGameObject.Team.Red:
                    {
                        redPlayerCount -= 1;
                        break;
                    
                    }
                case KBControllableGameObject.Team.Blue:
                    {
                        bluePlayerCount -= 1;
                        break;
                    }
            }
        }
    }

    void IncrementGoalZoneScore(Player.Team team)
    {
        switch (team)
        {
            case KBControllableGameObject.Team.Red:
                goalScore += 1;
                break;
            case KBControllableGameObject.Team.Blue:
                goalScore -= 1;
                break;
            case KBControllableGameObject.Team.None:
                break;
            default:
                break;
        }
    }
}
