using KBConstants;
using System.Collections.Generic;
using UnityEngine;

public class CaptureZone : KBGameObject
{
    public static int CAPTURE_REQUIRED = 1000;
    private static int RED_TEAM_CAPTURE_COUNT = CAPTURE_REQUIRED;
    private static int BLUE_TEAM_CAPTURE_COUNT = -CAPTURE_REQUIRED;
    public int captureTotal;
    public float capturePercent;
    public int upgradePointsOnCapture;

    public enum ZoneState { NotCaptured, RedCaptured, BlueCaptured };
    public enum ZoneTier { A, B, C };
    public ZoneTier tier;
    public ZoneState state = ZoneState.NotCaptured;

    public CaptureZone[] requiredZones = new CaptureZone[0];

    private bool switchState = true;
    public List<Player> players;

    private AudioClip captureProgress;
    private AudioClip captureComplete;

    private bool redUnlocked, blueUnlocked;

    private void Start()
    {
        base.Start();
        capturePercent = 50.0f;
        captureTotal = 0;
        state = ZoneState.NotCaptured;
        players = new List<Player>(10);

        captureComplete = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.CaptureComplete]);
        captureProgress = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.CaptureProgress]);

        upgradePointsOnCapture = 100;
    }

    private void OnDrawGizmos()
    {
        Vector3 p = new Vector3(transform.position.x, 5, transform.position.z);
        if (requiredZones.Length > 0)
        {
            foreach (var z in requiredZones)
            {
                Color c = Color.black;
                switch (z.tier)
                {
                    case ZoneTier.A:
                        c = Color.green;
                        break;

                    case ZoneTier.B:
                        c = Color.yellow;
                        break;

                    case ZoneTier.C:
                        c = Color.magenta;
                        break;

                    default:
                        break;
                }
                Gizmos.color = c;
                Gizmos.DrawLine(p, new Vector3(z.transform.position.x, 5, z.transform.position.z));
                //Debug.DrawLine(new Vector3(transform.position.x, 5, transform.position.z), new Vector3(z.transform.position.x, 5, z.transform.position.z), c);
            }
        }
        if (redUnlocked)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(p + Vector3.left, 1);
        }
        if (blueUnlocked)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(p + Vector3.right, 1);
        }
    }

    private void Update()
    {
        #region Unlock Handling

        redUnlocked = false;
        blueUnlocked = false;
        if (requiredZones.Length > 0)
        {
            foreach (var z in requiredZones)
            {
                switch (z.state)
                {
                    case ZoneState.NotCaptured:
                        break;

                    case ZoneState.RedCaptured:
                        redUnlocked = true;
                        break;

                    case ZoneState.BlueCaptured:
                        blueUnlocked = true;
                        break;

                    default:
                        break;
                }
            }
        }
        else
        {
            redUnlocked = true;
            blueUnlocked = true;
        }

        #endregion Unlock Handling

        int captureDelta = 0;

        foreach (KBGameObject o in collisionObjects)
        {
            Player p = o.gameObject.GetComponentInChildren<Player>();
            if (p != null)
            {
                switch (p.teamScript.Team)
                {
                    case Team.Red:
                        if (redUnlocked)
                        {
                            captureDelta += p.stats.captureSpeed;  
                        }
                        break;

                    case Team.Blue:
                        if (blueUnlocked)
                        {
                            captureDelta -= p.stats.captureSpeed;  
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
            case ZoneState.NotCaptured:
                captureTotal += captureDelta;
                break;

            case ZoneState.RedCaptured:
                //captureTotal += captureDelta;
                break;

            case ZoneState.BlueCaptured:
                //captureTotal += captureDelta;
                break;

            default:
                break;
        }

        CheckCaptured();
    }

    private void CheckCaptured()
    {
        if (captureTotal >= RED_TEAM_CAPTURE_COUNT)
        {
            state = ZoneState.RedCaptured;
            if (switchState)
            {
                audio.PlayOneShot(captureComplete);
                switchState = false;

                foreach (KBGameObject o in collisionObjects)
                {
                    Player p = o.gameObject.GetComponentInChildren<Player>();
                    if (p != null)
                    {
                        switch (p.teamScript.Team)
                        {
                            case Team.Red:
                                p.upgradePoints += upgradePointsOnCapture;
                                break;

                            case Team.Blue:
                                break;

                            case Team.None:
                                break;

                            default:
                                break;
                        }
                    }
                }

            }
        }
        else if (captureTotal <= BLUE_TEAM_CAPTURE_COUNT)
        {
            state = ZoneState.BlueCaptured;
            if (switchState)
            {
                audio.PlayOneShot(captureComplete);
                switchState = false;

                foreach (KBGameObject o in collisionObjects)
                {
                    Player p = o.gameObject.GetComponentInChildren<Player>();
                    if (p != null)
                    {
                        switch (p.teamScript.Team)
                        {
                            case Team.Red:

                                break;

                            case Team.Blue:
                                p.upgradePoints += upgradePointsOnCapture;
                                break;

                            case Team.None:
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        //if (other.gameObject.CompareTag("Player"))
        //{
        //    Player p = other.gameObject.GetComponent<Player>();
        //    players.Add(p);
        //    audio.clip = captureProgress;
        //    audio.Play();
        //    audio.loop = true;
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        //if (other.gameObject.CompareTag("Player"))
        //{
        //    Player p = other.gameObject.GetComponent<Player>();
        //    players.Remove(p);
        //    audio.Stop();
        //}
    }

}