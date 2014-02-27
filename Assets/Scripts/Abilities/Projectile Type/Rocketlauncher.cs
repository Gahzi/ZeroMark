using UnityEngine;
using System.Collections;
using KBConstants;

public class Rocketlauncher : ProjectileAbilityBaseScript
{
    #region CONSTANTS
    public static float MACHINEGUN_COOLDOWN = 2.0f;
    public static int ROCKET_RANGE = 200;
    #endregion

    public override void Start()
    {
        base.Start();
        projectileType = (ProjectileBaseScript)Resources.Load(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.Rocket], typeof(ProjectileBaseScript));
        cooldown = MACHINEGUN_COOLDOWN;
        sound = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.PlasmaGunFire]);
        audio.clip = sound;
        SetMaxRange(ROCKET_RANGE);
    }
}
