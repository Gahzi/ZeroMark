﻿using UnityEngine;
using KBConstants;

public class RailGun : ProjectileAbilityBaseScript
{
    #region CONSTANTS

    public static float COOLDOWN_0 = 0.30f;
    public static int RANGE_0 = 100;
    public static float RELOAD_TIME_0 = 2.2f;
    public static int CLIP_SIZE_0 = 6;
    public static int BURST_SIZE_0 = 1;
    public static float BURST_DELAY_0 = 0.00f;
    public static float SPREADMAX_0 = 0.0f;

    public static float COOLDOWN_1 = 0.25f;
    public static int RANGE_1 = 100;
    public static float RELOAD_TIME_1 = 1.8f;
    public static int CLIP_SIZE_1 = 8;
    public static int BURST_SIZE_1 = 4;
    public static float BURST_DELAY_1 = 0.00f;
    public static float SPREADMIN_1 = 0.0f;
    public static float SPREADMAX_1 = 0.0f;

    public static float COOLDOWN_2 = 0.20f;
    public static int RANGE_2 = 100;
    public static float RELOAD_TIME_2 = 1.6f;
    public static int CLIP_SIZE_2 = 10;
    public static int BURST_SIZE_2 = 6;
    public static float BURST_DELAY_2 = 0.00f;
    public static float SPREADMIN_2 = 0.0f;
    public static float SPREADMAX_2 = 0.0f;

    #endregion CONSTANTS

    public override void Start()
    {
        projectileType = new ProjectileBaseScript[3]
        {
            (ProjectileBaseScript)Resources.Load(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.RailgunBulletLevel0], typeof(ProjectileBaseScript)),
            (ProjectileBaseScript)Resources.Load(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.RailgunBulletLevel1], typeof(ProjectileBaseScript)),
            (ProjectileBaseScript)Resources.Load(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.RailgunBulletLevel2], typeof(ProjectileBaseScript))
        };
        for (int i = 0; i < projectileType.Length; i++)
        {
            ObjectPool.CreatePool(projectileType[i]);
        }
        sound = new AudioClip[2]
        {
            Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.PlasmaGunFire01]),
            Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.PlasmaGunFire02]),
        };
        reloadClip = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.PlasmaReload01]);
        SetLevel(0);
        base.Start();
        burstFireWeapon = true;
    }

    public override int SetLevel(int level)
    {
        switch (level)
        {
            case 0:
                cooldown = COOLDOWN_0;
                SetMaxRange(RANGE_0);
                cooldownStart = COOLDOWN_0;
                ammo = CLIP_SIZE_0;
                reloadTime = RELOAD_TIME_0;
                clipSize = CLIP_SIZE_0;
                burstSize = BURST_SIZE_0;
                burstDelay = BURST_DELAY_0;
                maximumSpreadAngle = SPREADMAX_0;
                particleSystem.startSize = 1.5f;
                particleSystem.startLifetime = 1.5f;
                return level;

            case 1:
                cooldown = COOLDOWN_1;
                SetMaxRange(RANGE_1);
                cooldownStart = COOLDOWN_1;
                ammo = CLIP_SIZE_1;
                reloadTime = RELOAD_TIME_1;
                clipSize = CLIP_SIZE_1;
                burstSize = BURST_SIZE_1;
                burstDelay = BURST_DELAY_1;
                minimumSpreadAngle = SPREADMIN_1;
                maximumSpreadAngle = SPREADMAX_1;
                particleSystem.startSize = 2.75f;
                particleSystem.startLifetime = 2.0f;

                return level;

            case 2:
                cooldown = COOLDOWN_2;
                SetMaxRange(RANGE_2);
                cooldownStart = COOLDOWN_2;
                ammo = CLIP_SIZE_2;
                reloadTime = RELOAD_TIME_2;
                clipSize = CLIP_SIZE_2;
                burstSize = BURST_SIZE_2;
                burstDelay = BURST_DELAY_2;
                minimumSpreadAngle = SPREADMIN_2;
                maximumSpreadAngle = SPREADMAX_2;
                particleSystem.startSize = 3.75f;
                particleSystem.startLifetime = 3.0f;

                return level;

            default:
                return -1;
        }
    }
}