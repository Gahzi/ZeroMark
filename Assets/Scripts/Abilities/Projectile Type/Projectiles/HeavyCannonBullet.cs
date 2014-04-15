using UnityEngine;
using System.Collections;

public class HeavyCannonBullet : ProjectileBaseScript
{

    public override void Start()
    {
        base.Start();
        collideWithProjectiles = false;
        damageLevel = new int[3] 
        { 
            KBConstants.AbilityConstants.DAMAGE_VALUES[KBConstants.AbilityConstants.type.HeavyCannonLevel0],
            KBConstants.AbilityConstants.DAMAGE_VALUES[KBConstants.AbilityConstants.type.HeavyCannonLevel1],
            KBConstants.AbilityConstants.DAMAGE_VALUES[KBConstants.AbilityConstants.type.HeavyCannonLevel2]
        };
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
