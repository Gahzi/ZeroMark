public class PlasmaBullet : ProjectileBaseScript
{
    public override void Awake()
    {
        base.Awake();
        damageLevel = new int[3]
        {
            KBConstants.AbilityConstants.DAMAGE_VALUES[KBConstants.AbilityConstants.type.PlasmaLevel0],
            KBConstants.AbilityConstants.DAMAGE_VALUES[KBConstants.AbilityConstants.type.PlasmaLevel1],
            KBConstants.AbilityConstants.DAMAGE_VALUES[KBConstants.AbilityConstants.type.PlasmaLevel2]
        };
    }
}