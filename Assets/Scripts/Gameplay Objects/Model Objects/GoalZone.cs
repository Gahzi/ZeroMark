using UnityEngine;
using System.Collections.Generic;
using KBConstants;

public class GoalZone : KBGameObject
{
    public static int CAPTURE_REQUIRED = 1000;
    static int RED_TEAM_CAPTURE_COUNT = CAPTURE_REQUIRED;
    static int BLUE_TEAM_CAPTURE_COUNT = -CAPTURE_REQUIRED;

    static int RED_TEAM_CAPTURE_RATE = 1;
    static int BLUE_TEAM_CAPTURE_RATE = -1;

    public enum GoalState { NotCaptured, RedCaptured, BlueCaptured, Contested };
    public GoalState state = GoalState.NotCaptured;

    bool playSound = true;

    int redTowerCount = 0;
    int blueTowerCount = 0;

    public List<Tower> towers;

    public int goalScore = 0;

    AudioClip captureProgress;
    AudioClip captureComplete;

    void Start()
    {
        base.Start();
        goalScore = 0;
        state = GoalState.NotCaptured;
        towers = new List<Tower>(10);

        captureComplete = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.CaptureComplete]);
        captureProgress = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.CaptureProgress]);
    }

    void Update()
    {
        redTowerCount = 0;
        blueTowerCount = 0;
        foreach (KBGameObject o in collisionObjects)
        {
            Tower t = o.gameObject.GetComponentInChildren<Tower>();
            if (t != null)
            {
                switch (t.teamScript.Team)
                {
                    case Team.Red:
                        redTowerCount++;
                        break;
                    case Team.Blue:
                        blueTowerCount++;
                        break;
                    case Team.None:
                        break;
                    default:
                        break;
                }
            }
            //if (t == null)
            //{
            //    Debug.Log("Null tower");
            //    break;
            //}

        }
        
        switch (state)
        {
            case GoalState.NotCaptured:

                if (redTowerCount > 0 && blueTowerCount > 0)
                {
                    if (redTowerCount == blueTowerCount)
                    {
                        state = GoalState.Contested;
                    }
                }

                if (redTowerCount > 0 || blueTowerCount > 0)
                {
                    goalScore += (redTowerCount * RED_TEAM_CAPTURE_RATE) + (blueTowerCount * BLUE_TEAM_CAPTURE_RATE);
                }
                else
                {
                    if (goalScore > 0) goalScore -= 1;
                    else if (goalScore < 0) goalScore += 1;
                }
                break;
            case GoalState.RedCaptured:
                break;
            case GoalState.BlueCaptured:
                break;
            case GoalState.Contested:
                if (redTowerCount > blueTowerCount || blueTowerCount > redTowerCount)
                {
                    state = GoalState.NotCaptured;
                }
                break;
            default:
                break;
        }

        CheckCaptured();
    }

    void CheckCaptured()
    {
        if (goalScore >= RED_TEAM_CAPTURE_COUNT)
        {
            state = GoalState.RedCaptured;
            if (playSound)
            {
                audio.PlayOneShot(captureComplete);
                playSound = false;
            }

        }
        else if (goalScore <= BLUE_TEAM_CAPTURE_COUNT)
        {
            state = GoalState.BlueCaptured;
            if (playSound)
            {
                audio.PlayOneShot(captureComplete);
                playSound = false;
            }
        }
    }


    void OnTriggerStay(Collider other)
    {
        //towers.Clear();
        //if (other.gameObject.CompareTag("Tower"))
        //{
        //    Tower p = other.gameObject.GetComponent<Tower>();
        //    towers.Add(p);
        //    //IncrementGoalZoneScore(other.gameObject.GetComponent<Tower>().teamScript.Team);
        //}
    }


    void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.gameObject.CompareTag("Tower"))
        {

            Tower p = other.gameObject.GetComponent<Tower>();
            towers.Add(p);
            //switch (p.teamScript.Team)
            //{
            //    case Team.Red:
            //        {
            //            redTowerCount += 1;
            //            break;
            //        }
            //    case Team.Blue:
            //        {
            //            blueTowerCount += 1;
            //            break;
            //        }
            //}
            audio.clip = captureProgress;
            audio.Play();
            audio.loop = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        if (other.gameObject.CompareTag("Tower"))
        {
            Tower p = other.gameObject.GetComponent<Tower>();
            towers.Remove(p);
            //switch (p.teamScript.Team)
            //{
            //    case Team.Red:
            //        {
            //            redTowerCount -= 1;
            //            break;

            //        }
            //    case Team.Blue:
            //        {
            //            blueTowerCount -= 1;
            //            break;
            //        }
            //}
            audio.Stop();
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
