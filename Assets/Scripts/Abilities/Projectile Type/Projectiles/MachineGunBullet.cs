using UnityEngine;
using System.Collections;

public class MachineGunBullet : ProjectileBaseScript {

    #region CONSTANTS

    public static int _damage = 1;

    #endregion CONSTANTS

    public override void Start()
    {
        base.Start();
        //explosionPrefab = Resources.Load<SmallExplosion>(KBConstants.ObjectConstants.PREFAB_NAMES[KBConstants.ObjectConstants.type.SmallExplosion]);
        collideWithProjectiles = false;
        damage = _damage;
        //ObjectPool.CreatePool(explosionPrefab);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void DoOnHit()
    {
        AreaOfEffectDamageScript a = ObjectPool.Spawn(explosionPrefab, transform.position);
        a.Init();
        base.DoOnHit();
    }
}
