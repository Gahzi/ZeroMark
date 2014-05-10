using UnityEngine;
using System.Collections;

public class RailgunBullet : ProjectileBaseScript {

    public override void Awake()
    {
        base.Awake();
        damageLevel = new int[3]
        {
            KBConstants.AbilityConstants.DAMAGE_VALUES[KBConstants.AbilityConstants.type.RailgunLevel0],
            KBConstants.AbilityConstants.DAMAGE_VALUES[KBConstants.AbilityConstants.type.RailgunLevel1],
            KBConstants.AbilityConstants.DAMAGE_VALUES[KBConstants.AbilityConstants.type.RailgunLevel2]
        };
    }
}
