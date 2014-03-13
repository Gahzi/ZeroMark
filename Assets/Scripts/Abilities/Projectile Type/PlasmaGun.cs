using UnityEngine;
using System.Collections;
using KBConstants;

/// <summary>
/// PlasmaGuns are a ProjectileAbility that shoots PlasmaBullet prefabs
/// </summary>
public class PlasmaGun : ProjectileAbilityBaseScript
{

    #region CONSTANTS
    public static float PLASMAGUN_COOLDOWN = 0.5f;
    public static int PLASMAGUN_RANGE = 65;
    public static float RELOAD_TIME = 1.5f;
    public static int CLIP_SIZE = 12;
    #endregion

    public override void Start()
    {
        //explosionPrefab = Resources.Load<NoDamageExplosionMedium>(KBConstants.ObjectConstants.PREFAB_NAMES[KBConstants.ObjectConstants.type.NoDamageExplosionMedium]);
        //ObjectPool.CreatePool(Resources.Load<NoDamageExplosionMedium>(KBConstants.ObjectConstants.PREFAB_NAMES[KBConstants.ObjectConstants.type.NoDamageExplosionMedium]));
        
        projectileType = (ProjectileBaseScript)Resources.Load(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.PlasmaBullet], typeof(ProjectileBaseScript));
        sound = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.PlasmaGunFire]);
        audio.clip = sound;
        SetMaxRange(PLASMAGUN_RANGE);
        cooldownStart = PLASMAGUN_COOLDOWN;
        ammo = CLIP_SIZE;
        reloadTime = RELOAD_TIME;
        clipSize = CLIP_SIZE;
        base.Start();
    }
}
