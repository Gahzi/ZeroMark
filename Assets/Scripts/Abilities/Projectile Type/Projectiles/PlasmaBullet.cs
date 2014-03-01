using UnityEngine;

public class PlasmaBullet : ProjectileBaseScript
{
    #region CONSTANTS

    public static int PLASMABULLET_DAMAGE = 1;

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