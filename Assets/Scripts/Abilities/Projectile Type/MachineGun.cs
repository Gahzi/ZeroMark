﻿using UnityEngine;
using System.Collections;
using KBConstants;

public class MachineGun : ProjectileAbilityBaseScript
{
    #region CONSTANTS
    public static float MACHINEGUN_COOLDOWN = 0.15f;
    public static int MACHINEGUN_RANGE = 45;
    public static float RELOAD_TIME = 3.0f;
    public static int CLIP_SIZE = 30;
    #endregion

    public override void Start()
    {
        projectileType[0] = (ProjectileBaseScript)Resources.Load(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.MachinegunBullet], typeof(ProjectileBaseScript));
        projectileType[1] = (ProjectileBaseScript)Resources.Load(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.MachinegunBullet], typeof(ProjectileBaseScript));
        cooldown = 0;;
        sound = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.MachineGunFire01]);
        reloadClip = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.MachineGunReload01]);
        audio.clip = sound;
        SetMaxRange(MACHINEGUN_RANGE);
        cooldownStart = MACHINEGUN_COOLDOWN;
        ammo = CLIP_SIZE;
        reloadTime = RELOAD_TIME;
        clipSize = CLIP_SIZE;
        
    }
}