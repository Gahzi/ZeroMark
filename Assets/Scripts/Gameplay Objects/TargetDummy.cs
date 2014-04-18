public class TargetDummy : KBGameObject
{

    public int startingHealth;

    public override void Start()
    {
        base.Start();
        team = KBConstants.Team.None;
        health = startingHealth;
    }

    public override int TakeDamage(int amount)
    {
        health -= amount;
        return health;
    }

    public new void FixedUpdate()
    {
        if (health <= 0)
        {
            health = startingHealth;
        }
    }
}