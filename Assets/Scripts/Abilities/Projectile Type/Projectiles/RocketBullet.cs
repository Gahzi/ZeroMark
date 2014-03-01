using UnityEngine;
using System.Collections;

public class RocketBullet : ProjectileBaseScript
{

    #region CONSTANTS

    new public static int DAMAGE = 1;

    #endregion CONSTANTS

    public override void Start()
    {
        base.Start();
        collideWithProjectiles = false;
    }

    public override void Update()
    {
        base.Update();
    }
}
