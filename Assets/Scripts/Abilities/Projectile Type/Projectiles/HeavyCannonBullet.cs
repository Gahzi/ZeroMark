using UnityEngine;
using System.Collections;

public class HeavyCannonBullet : ProjectileBaseScript
{

    #region CONSTANTS

    public static int damageL0 = 60;
    public static int damageL1 = 80;
    public static int damageL2 = 100;

    #endregion CONSTANTS

    public override void Start()
    {
        base.Start();
        collideWithProjectiles = false;
        damageLevel = new int[3] { damageL0, damageL1, damageL2 };
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void DoOnHit()
    {
        AreaOfEffectDamageScript a = ObjectPool.Spawn(explosionPrefab, transform.position);
        a.owner = owner;
        a.Init();
        base.DoOnHit();
    }
}
