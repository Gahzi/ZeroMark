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

    public void AddPoints(int points, KBConstants.Team team)
    {
        if (!captured)
        {
            switch (team)
            {
                case KBConstants.Team.Red:
                    redPoints += points;
                    RunPointAddFeedback(points, team);
                    if (redPoints >= pointsToControl)
                    {
                        team = KBConstants.Team.Red;
                        captured = true;
                        RunCapturedFeedback(team);
                    }
                    else
                    {
                    }
                    break;
                case KBConstants.Team.Blue:
                    bluePoints += points;
                    RunPointAddFeedback(points, team);
                    if (bluePoints >= pointsToControl)
                    {
                        team = KBConstants.Team.Blue;
                        captured = true;
                        RunCapturedFeedback(team);
                    }
                    else
                    {
                    }
                    break;
                case KBConstants.Team.None:
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