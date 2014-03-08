﻿using UnityEngine;
using System.Collections;

public class RocketBullet : ProjectileBaseScript
{

    #region CONSTANTS

    public static int ROCKET_DAMAGE = 1;

    #endregion CONSTANTS

    public ProjectileBaseScript explosionPrefab;

    public override void Start()
    {
        base.Start();
        damage = ROCKET_DAMAGE;
    }

    protected override void Update()
    {
        base.Update();
    }
}
