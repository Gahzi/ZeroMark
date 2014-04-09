using UnityEngine;

public class PlasmaBullet : ProjectileBaseScript
{
    #region CONSTANTS

    public static int damageL0 = 100;
    public static int damageL1 = 120;
    public static int damageL2 = 150;

    #endregion CONSTANTS
    
    public override void Start()
    {
        base.Start();
        collideWithProjectiles = false;
        damageLevel = new int[3] { damageL0, damageL1, damageL2 };
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