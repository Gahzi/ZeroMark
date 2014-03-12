using UnityEngine;
using System.Collections;

public class RocketBullet : ProjectileBaseScript
{

    #region CONSTANTS

    public static int ROCKET_DAMAGE = 1;

    #endregion CONSTANTS

    public override void Start()
    {
        base.Start();
        damage = ROCKET_DAMAGE;
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
