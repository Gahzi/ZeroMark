using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HomingMiniRocket : ProjectileBaseScript
{

    #region CONSTANTS

    public static int _damage = 10;
    public float rocketInitSpeed;
    public int accel;

    #endregion CONSTANTS

    private float targetSpeed;

    public override void Start()
    {
        base.Start();
        damage = _damage;
        targetSpeed = projectileSpeed;
        projectileSpeed = rocketInitSpeed;
        homingProjectile = true;
    }

    protected override void Update()
    {
        if (Time.time - spawnTime > 0.5f)
        {
            projectileSpeed = Mathf.Lerp(rocketInitSpeed, targetSpeed, accel * Time.deltaTime);
        }
        base.Update();
    }

    public override void DoOnHit()
    {
        AreaOfEffectDamageScript a = ObjectPool.Spawn(explosionPrefab, transform.position);
        a.Init();
        base.DoOnHit();
    }

    public override void Reset()
    {
        projectileSpeed = rocketInitSpeed;
        base.Reset();
    }

}