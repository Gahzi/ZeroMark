using KBConstants;
using System.Collections.Generic;
using UnityEngine;

public class CaptureZone : KBGameObject
{
    public static int CAPTURE_REQUIRED = 1000;
    private static int RED_TEAM_CAPTURE_COUNT = CAPTURE_REQUIRED;
    private static int BLUE_TEAM_CAPTURE_COUNT = -CAPTURE_REQUIRED;
    private int captureDelta;
    public int captureTotal;
    public int captureDecayOnUnoccupied;
    public float captureAutoPoints;
    public float capturePercent;
    public int upgradePointsOnCapture;
    public ScoreboardScript scoreboard;
    public string descriptiveName;

    public enum ZoneState { Unoccupied, Red, Blue };

    public enum ZoneTier { A, B, C };

    public ZoneTier tier;
    public ZoneState state = ZoneState.Unoccupied;

    public CaptureZone[] requiredZones = new CaptureZone[0];
    public ItemSpawn[] connectedItemSpawns = new ItemSpawn[0];

    private bool canSwitchState = true;
    public List<PlayerLocal> players;

    private AudioClip captureProgress;
    private AudioClip captureComplete;

    private bool redUnlocked, blueUnlocked;

    private float captureFraction = 0;

    private void Start()
    {
        base.Start();
        capturePercent = 50.0f;
        captureTotal = 0;
        state = ZoneState.Unoccupied;
        players = new List<PlayerLocal>(10);

        captureComplete = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.CaptureComplete]);
        captureProgress = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.CaptureProgress]);

        if (upgradePointsOnCapture == 0)
        {
            upgradePointsOnCapture = 100;
            Debug.LogWarning("Upgrade points on capture for CZ#" + gameObject.GetInstanceID() + " not set. Defaulting to 100");
        }

        foreach (ItemSpawn i in connectedItemSpawns)
        {
            i.connectedCaptureZone = this;
        }
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
        if (connectedItemSpawns.Length > 0)
        {
            foreach (var i in connectedItemSpawns)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(p, new Vector3(i.transform.position.x, 5, i.transform.position.z));
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
                    case ZoneState.Unoccupied:
                        break;

                    case ZoneState.Red:
                        redUnlocked = true;
                        break;

                    case ZoneState.Blue:
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

        captureDelta = 0;

        foreach (KBGameObject o in collisionObjects)
        {
            if (o != null)
            {
                PlayerLocal p = o.gameObject.GetComponentInChildren<PlayerLocal>();
                if (p != null)
                {
                    switch (p.team)
                    {
                        case Team.Red:
                            if (redUnlocked && p.health > 0)
                            {
                                captureDelta += p.stats.captureSpeed;
                            }
                            break;

                        case Team.Blue:
                            if (blueUnlocked && p.health > 0)
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
        }

        foreach (var o in requiredZones)
        {
            switch (o.state)
            {
                case ZoneState.Unoccupied:
                    break;
                case ZoneState.Red:
                    captureFraction += captureAutoPoints;
                    //captureDelta += 1;
                    break;
                case ZoneState.Blue:
                    captureFraction -= captureAutoPoints;
                    //captureDelta -= 1;
                    break;
                default:
                    break;
            }
        }
        if (Mathf.Abs(captureFraction) >= 1)
        {
            captureDelta += (int)captureFraction;
            captureFraction = 0;
        }

        if (tier == ZoneTier.C)
        {
            captureTotal += captureDelta / 10;
        }
        else
        {
            captureTotal += captureDelta;
        }

        captureTotal = captureTotal.LimitToRange(-1000, 1000);

        CheckCaptured();

        if (captureTotal > 0)
        {
            renderer.material.color = new Color(captureTotal / 1000.0f, 0, 0, (captureTotal / 1000.0f) * 0.5f);

        }
        else if (captureTotal < 0)
        {
            renderer.material.color = new Color(0, 0, captureTotal / 1000.0f, (captureTotal / 1000.0f) * 0.5f);
        }
        else
        {
            renderer.material.color = new Color(0, 0, 0, 0.35f);
        }

    }

    private void CheckCaptured()
    {
        if (canSwitchState)
        {
            if (captureTotal >= RED_TEAM_CAPTURE_COUNT) // 1000
            {
                CaptureEvent(Team.Red);
            }
            else if (captureTotal <= BLUE_TEAM_CAPTURE_COUNT) // -1000
            {
                CaptureEvent(Team.Blue);
            }
        }

        if (state == ZoneState.Blue && captureTotal >= 0) // Blue has lost control
        {
            CaptureEvent(Team.None);
        }

        if (state == ZoneState.Red && captureTotal <= 0) // Red has lost control
        {
            CaptureEvent(Team.None);
        }

        if (state == ZoneState.Unoccupied && captureDelta == 0)
        {
            if (Mathf.Abs(captureTotal) <= captureDecayOnUnoccupied)
            {
                captureTotal = 0;
            }
            if (captureTotal > 0)
            {
                captureTotal -= captureDecayOnUnoccupied;
            }
            else if (captureTotal < 0)
            {
                captureTotal += captureDecayOnUnoccupied;
            }

        }
    }

    /// <summary>
    /// This method handles any one-off events that should take place when a capture point is captured for the first time
    ///
    /// Relevant events:
    /// Players earn upgrade points
    /// Any connected item spawns begin spawning items
    /// GUI notifications
    /// State change
    /// </summary>
    /// <param name="t">The team that has captures the zone</param>
    private void CaptureEvent(Team t)
    {
        canSwitchState = false;

        foreach (ItemSpawn i in connectedItemSpawns)
        {
            i.ReceiveActivationEvent(t);
        }

        switch (t)
        {
            case Team.Red:
                audio.PlayOneShot(captureComplete);
                state = ZoneState.Red;
                // TODO : some kind of global message notifying a change in state
                break;

            case Team.Blue:
                audio.PlayOneShot(captureComplete);
                state = ZoneState.Blue;
                // TODO : some kind of global message notifying a change in state
                break;

            case Team.None:
                // TODO : Sound for loss of control;
                state = ZoneState.Unoccupied;
                canSwitchState = true;
                // TODO : some kind of global message notifying a change in state
                break;

            default:
                break;
        }

        if (t == Team.Red || t == Team.Blue)
        {
            foreach (KBGameObject o in collisionObjects)
            {
                PlayerLocal p = o.gameObject.GetComponentInChildren<PlayerLocal>();
                if (p != null)
                {
                    switch (p.team)
                    {
                        case Team.Red:
                            p.upgradePoints += upgradePointsOnCapture;
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

    private void OnTriggerStay(Collider other)
    {
        if (tier == ZoneTier.C)
        {
            if (other.gameObject.CompareTag("Item"))
            {
                Item i = other.gameObject.GetComponent<Item>();
                if (i.state == Item.ItemState.isDown)
                {
                    switch (i.team)
                    {
                        case Team.Red:
                            captureTotal += 100;
                            i.Respawn();
                            break;
                        case Team.Blue:
                            captureTotal -= 100;
                            i.Respawn();
                            break;
                        case Team.None:
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                }
            }
        }
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