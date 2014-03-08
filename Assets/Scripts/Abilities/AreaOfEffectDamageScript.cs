public class AreaOfEffectDamageScript : ProjectileBaseScript
{
    #region CONSTANTS

    public static int _damage = 1;

    #endregion CONSTANTS

    public override void Start()
    {
        base.Start();
        collideWithProjectiles = false;
        damage = _damage;
    }

    protected override void Update()
    {
        base.Update();
    }
}