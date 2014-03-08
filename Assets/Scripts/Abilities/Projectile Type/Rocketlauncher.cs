using UnityEngine;
using System.Collections;
using KBConstants;

public class Rocketlauncher : ProjectileAbilityBaseScript
{
    #region CONSTANTS
    public static float ROCKET_COOLDOWN = 2.0f;
    public static int ROCKET_RANGE = 200;
    public static float RELOAD_TIME = 10.0f;
    public static int CLIP_SIZE = 120;
    #endregion

    public ProjectileBaseScript explosionPrefab;

    public override void Start()
    {

        projectileType = (ProjectileBaseScript)Resources.Load(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.Rocket], typeof(ProjectileBaseScript));
        //sound = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.PlasmaGunFire]);
        //audio.clip = sound;
        SetMaxRange(ROCKET_RANGE);
        cooldownStart = ROCKET_COOLDOWN;
        ammo = CLIP_SIZE;
        reloadTime = RELOAD_TIME;
        clipSize = CLIP_SIZE;
        base.Start();
        ObjectPool.CreatePool(explosionPrefab);
    }
}
