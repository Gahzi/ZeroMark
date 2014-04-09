using UnityEngine;
using System.Collections;

public class MachineGunBullet : ProjectileBaseScript
{

    #region CONSTANTS

    public static int damageL0 = 30;
    public static int damageL1 = 40;
    public static int damageL2 = 60;

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
        a.Init();
        base.DoOnHit();
    }
}
