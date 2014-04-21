using UnityEngine;

public class LightAutoLaserBullet : ProjectileBaseScript
{
    #region CONSTANTS

    public static int damageL0 = 30;
    public static int damageL1 = 30;
    public static int damageL2 = 45;

    

    #endregion CONSTANTS

    public override void Awake()
    {
        base.Awake();
        damageLevel = new int[3] { damageL0, damageL1, damageL2 };
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
}