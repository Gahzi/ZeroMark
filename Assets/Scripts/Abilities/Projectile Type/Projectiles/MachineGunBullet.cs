public class MachineGunBullet : ProjectileBaseScript
{
    public override void Awake()
    {
        base.Awake();
        damageLevel = new int[3]
        {
            KBConstants.AbilityConstants.DAMAGE_VALUES[KBConstants.AbilityConstants.type.MachinegunLevel0],
            KBConstants.AbilityConstants.DAMAGE_VALUES[KBConstants.AbilityConstants.type.MachinegunLevel1],
            KBConstants.AbilityConstants.DAMAGE_VALUES[KBConstants.AbilityConstants.type.MachinegunLevel2]
        };
    }
}