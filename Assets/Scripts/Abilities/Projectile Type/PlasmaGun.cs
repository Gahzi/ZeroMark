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
    #endregion

    public override void Start()
    {
        base.Start();
        projectileType = (ProjectileBaseScript)Resources.Load(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.PlasmaBullet], typeof(ProjectileBaseScript));
        cooldown = PLASMAGUN_COOLDOWN;
        sound = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.PlasmaGunFire]);
        audio.clip = sound;
    }

}
