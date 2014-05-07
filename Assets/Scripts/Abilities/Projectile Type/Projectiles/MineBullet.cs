public class MineBullet : ProjectileBaseScript
{
    public override void Awake()
    {
        base.Awake();
        damageLevel = new int[3]
        {
            KBConstants.AbilityConstants.DAMAGE_VALUES[KBConstants.AbilityConstants.type.MineLevel0],
            KBConstants.AbilityConstants.DAMAGE_VALUES[KBConstants.AbilityConstants.type.MineLevel1],
            KBConstants.AbilityConstants.DAMAGE_VALUES[KBConstants.AbilityConstants.type.MineLevel2]
        };
    }

    protected override void FixedUpdate()
    {
        inheritSpeed = 0;
        lifetime = 15.0f;
        base.FixedUpdate();
    }
}