using UnityEngine;
using System.Collections;

public class ShotgunBullet : ProjectileBaseScript
{
    public override void Awake()
    {
        base.Awake();
        damageLevel = new int[3] 
        { 
            KBConstants.AbilityConstants.DAMAGE_VALUES[KBConstants.AbilityConstants.type.ShotgunLevel0],
            KBConstants.AbilityConstants.DAMAGE_VALUES[KBConstants.AbilityConstants.type.ShotgunLevel1],
            KBConstants.AbilityConstants.DAMAGE_VALUES[KBConstants.AbilityConstants.type.ShotgunLevel2]
        };
    }
}