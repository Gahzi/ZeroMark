public abstract class AreaOfEffectDamageScript : ProjectileBaseScript
{
    public override void Start()
    {
        base.Start();
        collideWithProjectiles = false;
    }
}