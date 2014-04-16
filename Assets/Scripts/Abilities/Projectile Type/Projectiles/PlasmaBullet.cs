using UnityEngine;

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

    public override void Start()
    {
        base.Start();
        collideWithProjectiles = false;   
        
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