public class SniperBullet : ProjectileBaseScript
{
    public override void Awake()
    {
        base.Awake();
        damageLevel = new int[3]
        {
            KBConstants.AbilityConstants.DAMAGE_VALUES[KBConstants.AbilityConstants.type.SniperLevel0],
            KBConstants.AbilityConstants.DAMAGE_VALUES[KBConstants.AbilityConstants.type.SniperLevel1],
            KBConstants.AbilityConstants.DAMAGE_VALUES[KBConstants.AbilityConstants.type.SniperLevel2]
        };
    }
}