using UnityEngine;
using System.Collections;
using KBConstants;

public class HeavyCannon : ProjectileAbilityBaseScript
{
    #region CONSTANTS
    public static float HCANNON_COOLDOWN = 0.25f;
    public static int HCANNON_RANGE = 80;
    public static float RELOAD_TIME = 10.0f;
    public static int CLIP_SIZE = 200;
    #endregion

    public override void Start()
    {
        projectileType[0] = (ProjectileBaseScript)Resources.Load(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.HeavyCannonBullet], typeof(ProjectileBaseScript));
        projectileType[1] = (ProjectileBaseScript)Resources.Load(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.HeavyCannonBullet], typeof(ProjectileBaseScript));
        cooldown = HCANNON_COOLDOWN;
        sound = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.CannonFire01]);
        reloadClip = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.MachineGunReload02]);
        audio.clip = sound;
        SetMaxRange(HCANNON_RANGE);
        cooldownStart = HCANNON_COOLDOWN;
        ammo = CLIP_SIZE;
        reloadTime = RELOAD_TIME;
        clipSize = CLIP_SIZE;
        base.Start();
    }
}