using UnityEngine;
using System.Collections;

public class MachineGunBullet : ProjectileBaseScript {

    #region CONSTANTS

    public static int MACHINE_GUN_DAMAGE = 1;

    #endregion CONSTANTS

    public override void Start()
    {
        base.Start();
        collideWithProjectiles = false;
        damage = MACHINE_GUN_DAMAGE;
    }

    protected override void Update()
    {
        base.Update();
    }
}
