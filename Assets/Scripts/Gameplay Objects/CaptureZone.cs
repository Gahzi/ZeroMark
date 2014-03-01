using KBConstants;
using System.Collections.Generic;
using UnityEngine;

public class CaptureZone : Zone
{
    
    // Capture zone class should probably inherit from a more generic type of zone.
    
    #region CONSTANTS

    public static int CAPTURE_REQUIRED = 1000;
    private static int RED_TEAM_CAPTURE_COUNT = CAPTURE_REQUIRED;
    private static int BLUE_TEAM_CAPTURE_COUNT = -CAPTURE_REQUIRED;
    private static int TierACaptureBonusPercent = 25;
    private static int TierACaptureRate = 1;
    private static int TierBCaptureRate = 5;
    private static int TierCCaptureRate = 50;

    #endregion CONSTANTS

    //private int captureDelta;
    public int captureTotal;
    //public int captureDecayOnUnoccupied;
    public float captureAutoPoints;
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
    public List<KBPlayer> players;
    private AudioClip captureProgress;
    private AudioClip captureComplete;
    public RotatableGuiItem rGui;
    public bool redUnlocked, blueUnlocked;
    private float captureFraction = 0.0f;

    public override void Start()
    {
        base.Start();
        captureTotal = 0;
        state = ZoneState.Unoccupied;
        players = new List<KBPlayer>(10);

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

    protected void OnDrawGizmos()
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

    protected virtual void FixedUpdate()
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

        //captureDelta = 0;

        switch (GameManager.Instance.State)
        {
            case GameManager.GameState.PreGame:
                break;

            case GameManager.GameState.InGame:
                CalculateCaptureTotal();
                RunVisualFeedback(captureTotal);
                CheckCaptured();
                break;

            case GameManager.GameState.RedWins:
                break;

            case GameManager.GameState.BlueWins:
                break;

            case GameManager.GameState.Tie:
                break;

            case GameManager.GameState.EndGame:
                break;

            default:
                break;
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

        //if (state == ZoneState.Unoccupied && captureDelta == 0)
        //{
        //    if (Mathf.Abs(captureTotal) <= captureDecayOnUnoccupied)
        //    {
        //        captureTotal = 0;
        //    }
        //    if (captureTotal > 0)
        //    {
        //        captureTotal -= captureDecayOnUnoccupied;
        //    }
        //    else if (captureTotal < 0)
        //    {
        //        captureTotal += captureDecayOnUnoccupied;
        //    }
        //}
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
                KBPlayer p = o.gameObject.GetComponentInChildren<KBPlayer>();
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
                ConvertItemToCapturePoints(other.gameObject.GetComponent<Item>());
            }
        }
    }

    //protected override void OnTriggerEnter(Collider other)
    //{
    //    base.OnTriggerEnter(other);
    //}

    //protected override void OnTriggerExit(Collider other)
    //{
    //    base.OnTriggerExit(other);
    //}

    /// <summary>
    /// Calculates the capture delta & applies team-wide percentage bonus to final rate and applies final delta to capture total.
    /// </summary>
    private void CalculateCaptureTotal()
    {
        int delta = CalculateAdjustedCaptureDelta(CalculateRawCaptureDelta());
        if (delta > 0)
        {
            delta *= ((GameManager.Instance.redBonus / 100 * TierACaptureBonusPercent) + 1);
        }
        else if (delta < 0)
        {
            delta *= ((GameManager.Instance.blueBonus / 100 * TierACaptureBonusPercent) + 1);
        }
        if (Mathf.Abs(captureFraction) >= 1) // accumulates decimal capture point values and adds it in if the abs > 1
        {
            captureTotal += (int)captureFraction;
            captureFraction = 0;
        }
        captureTotal += delta;
        captureTotal = captureTotal.LimitToRange(-1000, 1000);
    }

    /// <summary>
    /// Checks for players inside collider and determines overall capture rate.
    /// </summary>
    /// <returns>Capture delta based on players</returns>
    private int CalculateRawCaptureDelta()
    {
        int x = 0;
        
        foreach (KBGameObject o in collisionObjects)
        {
            KBPlayer p = o.gameObject.GetComponentInChildren<KBPlayer>();
            if (p != null)
            {
                switch (p.team)
                {
                    case Team.Red:
                        if (redUnlocked && p.health > 0)
                        {
                            x += p.stats.captureSpeed;
                        }
                        break;

                    case Team.Blue:
                        if (blueUnlocked && p.health > 0)
                        {
                            x -= p.stats.captureSpeed;
                        }
                        break;

                    case Team.None:
                        break;

                    default:
                        break;
                }
            }
        }
        return x;
    }
    
    /// <summary>
    /// Applies capture delta to the zone's capture total, adjusted for tier multipler.
    /// </summary>
    /// <param name="rawCaptureDelta"></param>
    /// <returns>Final capture total</returns>
    private int CalculateAdjustedCaptureDelta(int rawCaptureDelta)
    {
        float floatCapDelta = 0.0f;
        int finalCaptureDelta = 0;
        switch (tier)
        {
            case ZoneTier.A:
                floatCapDelta += (float)rawCaptureDelta * (float)TierACaptureRate;
                break;
            case ZoneTier.B:
                floatCapDelta += (float)rawCaptureDelta / (float)TierBCaptureRate;
                break;
            case ZoneTier.C:
                floatCapDelta += (float)rawCaptureDelta / (float)TierCCaptureRate;
                break;
            default:
                break;
        }
        finalCaptureDelta = Mathf.FloorToInt(floatCapDelta);
        captureFraction += floatCapDelta - finalCaptureDelta;
        return finalCaptureDelta;
    }

    private void CalculateAutoCapturePoints()
    {
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
    }

    protected void RunVisualFeedback(int capturePoints)
    {
        if (capturePoints > 0)
        {
            renderer.material.color = new Color(
                capturePoints / 1000.0f,
                0,
                0,
                (capturePoints / 1000.0f) * 0.5f + 0.5f
                );
        }
        else if (capturePoints < 0)
        {
            renderer.material.color = new Color(
                0,
                0,
                capturePoints / 1000.0f,
                (capturePoints / 1000.0f) * 0.5f + 0.5f
                );
        }
        else
        {
            renderer.material.color = new Color(0, 0, 0, 0.35f);
        }
    }

    private void ConvertItemToCapturePoints(Item i)
    {
        if (i.state == Item.ItemState.isDown && i != null)
        {
            switch (i.team)
            {
                case Team.Red:
                    captureTotal += 100;
                    collisionObjects.Remove(i); // TODO : should have to remove this manually?
                    i.Respawn();
                    break;

                case Team.Blue:
                    captureTotal -= 100;
                    collisionObjects.Remove(i);
                    i.Respawn();
                    break;

                case Team.None:
                    break;

                default:
                    break;
            }
        }
    }

    protected void SetRGUI()
    {
        rGui = GetComponent<RotatableGuiItem>();
        if (rGui == null)
        {
            gameObject.AddComponent<RotatableGuiItem>();
        }
        rGui.ScreenpointToAlign = RotatableGuiItem.AlignmentScreenpoint.BottomLeft;
        rGui.angle = 45;
        rGui.enabled = false;
    }

    protected void LoadSounds()
    {
        captureComplete = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.CaptureComplete]);
        captureProgress = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.CaptureProgress]);
    }
}