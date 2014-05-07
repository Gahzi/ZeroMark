using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HomingMiniRocket : ProjectileBaseScript
{

    #region CONSTANTS

    public float rocketInitSpeed;
    public int accel;
    public float trackingStrength;

    #endregion CONSTANTS

    private float targetSpeed;

    public override void Awake()
    {
        base.Awake();
        damageLevel = new int[3]
        { 
            KBConstants.AbilityConstants.DAMAGE_VALUES[KBConstants.AbilityConstants.type.HomingRocketLevel0],
            KBConstants.AbilityConstants.DAMAGE_VALUES[KBConstants.AbilityConstants.type.HomingRocketLevel1],
            KBConstants.AbilityConstants.DAMAGE_VALUES[KBConstants.AbilityConstants.type.HomingRocketLevel2]
        };
    }

    public override void Start()
    {
        base.Start();
        targetSpeed = 100;
        projectileSpeed = rocketInitSpeed;
        homingProjectile = true;
        
    }

    protected override void FixedUpdate()
    {
        if (Time.time - spawnTime > 0.5f)
        {
            projectileSpeed = Mathf.Lerp(rocketInitSpeed, targetSpeed, accel * Time.deltaTime);
        }
        base.FixedUpdate();
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

    protected override void DoHomingBehavior()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPlayer.transform.position - owner.transform.position), trackingStrength * Time.deltaTime);
    }

}