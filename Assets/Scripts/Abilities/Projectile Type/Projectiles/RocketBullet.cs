using UnityEngine;
using System.Collections;

public class RocketBullet : ProjectileBaseScript
{

    #region CONSTANTS

    public float rocketInitSpeed;
    public int accel;

    #endregion CONSTANTS

    public float targetSpeed;

    public override void Awake()
    {
        base.Awake();
        damageLevel = new int[3]
        { 
            KBConstants.AbilityConstants.DAMAGE_VALUES[KBConstants.AbilityConstants.type.RocketLevel0],
            KBConstants.AbilityConstants.DAMAGE_VALUES[KBConstants.AbilityConstants.type.RocketLevel1],
            KBConstants.AbilityConstants.DAMAGE_VALUES[KBConstants.AbilityConstants.type.RocketLevel2]
        };
    }

    public override void Start()
    {
        base.Start();
        projectileSpeed = rocketInitSpeed;
    }

    protected override void Update()
    {
        if (Time.time - spawnTime > 0.5f)
        {
            projectileSpeed = Mathf.Lerp(rocketInitSpeed, targetSpeed, accel * Time.deltaTime);
        }
        base.Update();
    }

    public override void Reset()
    {
        projectileSpeed = rocketInitSpeed;
        base.Reset();
    }
}
