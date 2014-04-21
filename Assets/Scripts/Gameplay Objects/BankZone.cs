using KBConstants;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class BankZone : Zone
{
    public int pointsToControl;
    public int redPoints;
    public int bluePoints;
    private bool captured;

    public bool Captured
    {
        get
        {
            return captured;
        }
    }

    public FloatingText textPrefab;

    public override void Start()
    {
        base.Start();
        team = KBConstants.Team.None;
        redPoints = 0;
        bluePoints = 0;
        rigidbody.isKinematic = true;
        collider.isTrigger = true;
        //textPrefab = Resources.Load("gui/floatingtext") as FloatingText;
        ObjectPool.CreatePool(textPrefab);
    }

    private void OnDrawGizmos()
    {
    }

    [RPC]
    public void AddPoints(int points, int teamNum, PhotonMessageInfo msg)
    {
        if (!captured)
        {
            Team bankingPlayerTeam = (Team)teamNum;
            switch (bankingPlayerTeam)
            {
                case Team.Red:
                    redPoints += points;
                    RunPointAddFeedback(points, Team.Red);
                    if (redPoints >= pointsToControl)
                    {
                        team = Team.Red;
                        captured = true;
                        RunCapturedFeedback(Team.Red);
                    }
                    break;

                case Team.Blue:
                    bluePoints += points;
                    RunPointAddFeedback(points, Team.Blue);
                    if (bluePoints >= pointsToControl)
                    {
                        team = Team.Blue;
                        captured = true;
                        RunCapturedFeedback(Team.Blue);
                    }
                    break;

                case Team.None:
                    Debug.LogError("You're trying to add points to a zone, but the player has no team.");
                    break;

                default:
                    break;
            }
        }
    }

    private void RunPointAddFeedback(int points, KBConstants.Team team)
    {
        FloatingText t = ObjectPool.Spawn(textPrefab, transform.position);
        t.Init();
        t.GetComponent<TextMesh>().text = "+" + points.ToString();
    }

    private void RunCapturedFeedback(KBConstants.Team team)
    {
        FloatingText obj = ObjectPool.Spawn(textPrefab, transform.position);
        obj.Init(10000f);
        obj.GetComponent<FloatUp>().upSpeed = 0;
        TextMesh t = obj.GetComponent<TextMesh>();
        t.text = "CAPTURED";
        switch (team)
        {
            case KBConstants.Team.Red:
                t.color = Color.red;
                break;

            case KBConstants.Team.Blue:
                t.color = Color.blue;
                break;

            case KBConstants.Team.None:
                break;

            default:
                break;
        }
    }
}