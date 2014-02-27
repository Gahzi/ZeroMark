using UnityEngine;
using System.Collections;
using KBConstants;

public class MachineGun : ProjectileAbilityBaseScript
{
    #region CONSTANTS
    public static float MACHINEGUN_COOLDOWN = 0.05f;
    public static int MACHINEGUN_RANGE = 35;
    public static float RELOAD_TIME = 5.0f;
    public static int CLIP_SIZE = 60;
    #endregion

    public override void Start()
    {
        base.Start();
        projectileType = (ProjectileBaseScript)Resources.Load(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.MachinegunBullet], typeof(ProjectileBaseScript));
        cooldown = MACHINEGUN_COOLDOWN;
        sound = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.PlasmaGunFire]);
        audio.clip = sound;
        SetMaxRange(MACHINEGUN_RANGE);
        cooldownStart = MACHINEGUN_COOLDOWN;
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