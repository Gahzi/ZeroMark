using KBConstants;
using System.Collections.Generic;
using UnityEngine;

public class CaptureZone : KBGameObject
{
    public static int CAPTURE_REQUIRED = 1000;
    private static int RED_TEAM_CAPTURE_COUNT = CAPTURE_REQUIRED;
    private static int BLUE_TEAM_CAPTURE_COUNT = -CAPTURE_REQUIRED;

    private static int RED_TEAM_CAPTURE_RATE = 1;
    private static int BLUE_TEAM_CAPTURE_RATE = -1;

    public enum GoalState { NotCaptured, RedCaptured, BlueCaptured, Contested };

    public GoalState state = GoalState.NotCaptured;

    public CaptureZone[] zoneDependencies = new CaptureZone[3];

    private bool playSound = true;

    private int redPlayerCount = 0;
    private int bluePlayerCount = 0;

    public List<Player> players;

    public int goalScore = 0;

    private AudioClip captureProgress;
    private AudioClip captureComplete;

    private void Start()
    {
        base.Start();
        goalScore = 0;
        state = GoalState.NotCaptured;
        players = new List<Player>(10);

        captureComplete = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.CaptureComplete]);
        captureProgress = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.CaptureProgress]);
    }

    private void Update()
    {
        redPlayerCount = 0;
        bluePlayerCount = 0;
        bool redUnlocked = false;
        bool blueUnlocked = false;

        foreach (var z in zoneDependencies)
        {
            switch (z.state)
            {
                case GoalState.NotCaptured:
                    break;

                case GoalState.RedCaptured:
                    redUnlocked = true;
                    break;

                case GoalState.BlueCaptured:
                    blueUnlocked = true;
                    break;

                case GoalState.Contested:
                    break;

                default:
                    break;
            }
        }

        foreach (KBGameObject o in collisionObjects)
        {
            Player t = o.gameObject.GetComponentInChildren<Player>();
            if (t != null)
            {
                switch (t.teamScript.Team)
                {
                    case Team.Red:
                        if (redUnlocked)
                        {
                            redPlayerCount++;
                        }

                        break;

                    case Team.Blue:
                        if (blueUnlocked)
                        {
                            bluePlayerCount++;
                        }

                        break;

                    case Team.None:
                        break;

                    default:
                        break;
                }
            }
        }

        switch (state)
        {
            case GoalState.NotCaptured:

                if (redPlayerCount > 0 && bluePlayerCount > 0)
                {
                    if (redPlayerCount == bluePlayerCount)
                    {
                        state = GoalState.Contested;
                    }
                }

                if (redPlayerCount > 0 || bluePlayerCount > 0)
                {
                    goalScore += (redPlayerCount * RED_TEAM_CAPTURE_RATE) + (bluePlayerCount * BLUE_TEAM_CAPTURE_RATE);
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
                if (redPlayerCount > bluePlayerCount || bluePlayerCount > redPlayerCount)
                {
                    state = GoalState.NotCaptured;
                }
                break;

            default:
                break;
        }

        CheckCaptured();
    }

    private void CheckCaptured()
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

    private void OnTriggerStay(Collider other)
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.gameObject.CompareTag("Tower"))
        {
            Player p = other.gameObject.GetComponent<Player>();
            players.Add(p);
            audio.clip = captureProgress;
            audio.Play();
            audio.loop = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        if (other.gameObject.CompareTag("Tower"))
        {
            Player p = other.gameObject.GetComponent<Player>();
            players.Remove(p);
            audio.Stop();
        }
    }

    private void IncrementGoalZoneScore(Team team)
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