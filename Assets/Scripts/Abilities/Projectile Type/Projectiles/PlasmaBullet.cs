using UnityEngine;

public class PlasmaBullet : ProjectileBaseScript
{
    #region CONSTANTS

    public static int PLASMABULLET_DAMAGE = 1;

    #endregion CONSTANTS
    
    public override void Start()
    {
        base.Start();
        collideWithProjectiles = false;
        damage = PLASMABULLET_DAMAGE;
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