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

    public override void Start()
    {
        base.Start();
        projectileType = (ProjectileBaseScript)Resources.Load(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.Rocket], typeof(ProjectileBaseScript));
        sound = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.PlasmaGunFire]);
        audio.clip = sound;
        SetMaxRange(ROCKET_RANGE);
        cooldownStart = ROCKET_COOLDOWN;
        ammo = CLIP_SIZE;
        reloadTime = RELOAD_TIME;
        clipSize = CLIP_SIZE;
    }

    //protected override IEnumerator Reload()
    //{
    //    yield return new WaitForSeconds(RELOAD_TIME);
    //    ammo = CLIP_SIZE;
    //    triggerReload = false;
    //}
}
