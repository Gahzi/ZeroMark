using UnityEngine;
using KBConstants;

public class LightAutoLaser : ProjectileAbilityBaseScript
{

    #region CONSTANTS
    public static float _cooldown = 0.5f;
    public static int _range = 35;
    public static float _reloadTime = 1.0f;
    public static int _clipSize = 12;
    #endregion

    public override void Start()
    {
        projectileType = (ProjectileBaseScript)Resources.Load(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.LightAutoLaserBullet], typeof(ProjectileBaseScript));
        cooldown = _cooldown;
        //sound = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.PlasmaGunFire]);
        audio.clip = sound;
        SetMaxRange(_range);
        cooldownStart = _cooldown;
        ammo = _clipSize;
        reloadTime = _reloadTime;
        clipSize = _clipSize;
        base.Start();
    }
}