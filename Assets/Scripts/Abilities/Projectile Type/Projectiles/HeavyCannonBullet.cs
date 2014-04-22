using UnityEngine;
using System.Collections;

public class HeavyCannonBullet : ProjectileBaseScript
{

    public override void Awake()
    {
        base.Awake();
        damageLevel = new int[3] 
        { 
            KBConstants.AbilityConstants.DAMAGE_VALUES[KBConstants.AbilityConstants.type.HeavyCannonLevel0],
            KBConstants.AbilityConstants.DAMAGE_VALUES[KBConstants.AbilityConstants.type.HeavyCannonLevel1],
            KBConstants.AbilityConstants.DAMAGE_VALUES[KBConstants.AbilityConstants.type.HeavyCannonLevel2]
        };
    }
}
