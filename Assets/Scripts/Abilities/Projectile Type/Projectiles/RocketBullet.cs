using UnityEngine;
using System.Collections;

public class RocketBullet : ProjectileBaseScript
{

    #region CONSTANTS

    public static int ROCKET_DAMAGE = 1;

    #endregion CONSTANTS

    //private RocketExplosion explosionPrefab;

    public override void Start()
    {
        base.Start();
        damage = ROCKET_DAMAGE;
        //explosionPrefab = Resources.Load<RocketExplosion>(KBConstants.ObjectConstants.PREFAB_NAMES[KBConstants.ObjectConstants.type.RocketExplosion]);
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
