using UnityEngine;
using System.Collections;

public class LightCannonBullet : ProjectileBaseScript
{

    public override void Start()
    {
        base.Start();
        damageLevel = new int[3] 
        { 
            KBConstants.AbilityConstants.DAMAGE_VALUES[KBConstants.AbilityConstants.type.LightCannonLevel0],
            KBConstants.AbilityConstants.DAMAGE_VALUES[KBConstants.AbilityConstants.type.LightCannonLevel1],
            KBConstants.AbilityConstants.DAMAGE_VALUES[KBConstants.AbilityConstants.type.LightCannonLevel2]
        };
    }

}
