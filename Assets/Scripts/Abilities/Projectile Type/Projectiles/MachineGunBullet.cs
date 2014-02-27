using UnityEngine;
using System.Collections;

public class MachineGunBullet : ProjectileBaseScript {

    #region CONSTANTS

    public static int DAMAGE = 1;

    #endregion CONSTANTS

    public override void Start()
    {
        base.Start();
        collideWithProjectiles = false;
    }

    private void Update()
    {
        base.Update();
    }
}
