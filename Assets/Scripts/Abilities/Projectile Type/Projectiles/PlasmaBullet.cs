using UnityEngine;

public class PlasmaBullet : ProjectileBaseScript
{
    #region CONSTANTS

    public static int PLASMABULLET_DAMAGE = 1;

    #endregion CONSTANTS

    //private NoDamageExplosionMedium explosionPrefab;
    
    public override void Start()
    {
        base.Start();
        //explosionPrefab = Resources.Load<NoDamageExplosionMedium>(KBConstants.ObjectConstants.PREFAB_NAMES[KBConstants.ObjectConstants.type.NoDamageExplosionMedium]);
        collideWithProjectiles = false;
        damage = PLASMABULLET_DAMAGE;
        //ObjectPool.CreatePool(explosionPrefab);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void DoOnHit()
    {
        ObjectPool.Spawn(explosionPrefab, transform.position);
        base.DoOnHit();
    }
}