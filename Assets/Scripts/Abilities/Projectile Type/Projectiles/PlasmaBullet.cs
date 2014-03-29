using UnityEngine;

public class PlasmaBullet : ProjectileBaseScript
{
    #region CONSTANTS

    public static int _damage = 50;

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

    public override void DoOnHit()
    {
        AreaOfEffectDamageScript a = ObjectPool.Spawn(explosionPrefab, transform.position);
        a.Init(); 
        base.DoOnHit();
    }
}