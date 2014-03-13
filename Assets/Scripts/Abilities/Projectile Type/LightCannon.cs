using UnityEngine;
using System.Collections;
using KBConstants;

public class LightCannon : ProjectileAbilityBaseScript
{
    #region CONSTANTS
    public static float LCANNON_COOLDOWN = 0.30f;
    public static int LCANNON_RANGE = 60;
    public static float RELOAD_TIME = 3.0f;
    public static int CLIP_SIZE = 10;
    #endregion

    public override void Start()
    {
        projectileType = (ProjectileBaseScript)Resources.Load(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.LightCannonBullet], typeof(ProjectileBaseScript));
        cooldown = LCANNON_COOLDOWN;
        sound = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.CannonFire01]);
        reloadClip = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.MachineGunReload02]);
        audio.clip = sound;
        SetMaxRange(LCANNON_RANGE);
        cooldownStart = LCANNON_COOLDOWN;
        ammo = CLIP_SIZE;
        reloadTime = RELOAD_TIME;
        clipSize = CLIP_SIZE;
        base.Start();
    }
}