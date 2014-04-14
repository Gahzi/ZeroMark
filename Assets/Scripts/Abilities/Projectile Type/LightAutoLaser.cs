﻿using UnityEngine;
using KBConstants;

public class LightAutoLaser : ProjectileAbilityBaseScript
{

    #region CONSTANTS
    public static float COOLDOWN_0 = 0.035f;
    public static int RANGE_0 = 15;
    public static float RELOAD_TIME_0 = 0.25f;
    public static int CLIP_SIZE_0 = 5;

    public static float COOLDOWN_1 = 0.025f;
    public static int RANGE_1 = 25;
    public static float RELOAD_TIME_1 = 0.20f;
    public static int CLIP_SIZE_1 = 7;

    public static float COOLDOWN_2 = 0.025f;
    public static int RANGE_2 = 20;
    public static float RELOAD_TIME_2 = 0.25f;
    public static int CLIP_SIZE_2 = 5;
    #endregion

    public override void Start()
    {
        projectileType = new ProjectileBaseScript[3]
        {
            (ProjectileBaseScript)Resources.Load(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.LightAutoLaserBullet], typeof(ProjectileBaseScript)),
            (ProjectileBaseScript)Resources.Load(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.LightAutoLaserBullet], typeof(ProjectileBaseScript)),
            (ProjectileBaseScript)Resources.Load(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.LightAutoLaserBullet], typeof(ProjectileBaseScript))
        };
        for (int i = 0; i < projectileType.Length; i++)
        {
            ObjectPool.CreatePool(projectileType[i]);
        }
        //sound = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.PlasmaGunFire]);
        audio.clip = sound;
        SetLevel(0);
        base.Start();
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
                return level;

            case 1:
                cooldown = COOLDOWN_1;
                SetMaxRange(RANGE_1);
                cooldownStart = COOLDOWN_1;
                ammo = CLIP_SIZE_1;
                reloadTime = RELOAD_TIME_1;
                clipSize = CLIP_SIZE_1;
                return level;

            case 2:
                cooldown = COOLDOWN_2;
                SetMaxRange(RANGE_2);
                cooldownStart = COOLDOWN_2;
                ammo = CLIP_SIZE_2;
                reloadTime = RELOAD_TIME_2;
                clipSize = CLIP_SIZE_2;
                return level;

            default:
                return -1;
        }
    }
}