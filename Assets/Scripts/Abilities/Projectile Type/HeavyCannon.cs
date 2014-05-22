using KBConstants;
using UnityEngine;

public class HeavyCannon : ProjectileAbilityBaseScript
{
    #region CONSTANTS

    public static float COOLDOWN_0 = 0.100f;
    public static int RANGE_0 = 200;
    public static float RELOAD_TIME_0 = 2.300f;
    public static int CLIP_SIZE_0 = 20;
    public static int BURST_SIZE_0 = 2;
    public static float BURST_DELAY_0 = 0.00f;
    public static float SPREAD_0 = 1.000f;

    public static float COOLDOWN_1 = 0.0750f;
    public static int RANGE_1 = 200;
    public static float RELOAD_TIME_1 = 2.000f;
    public static int CLIP_SIZE_1 = 30;
    public static int BURST_SIZE_1 = 3;
    public static float BURST_DELAY_1 = 0.000f;
    public static float SPREAD_1 = 1.000f;

    public static float COOLDOWN_2 = 0.100f;
    public static int RANGE_2 = 200;
    public static float RELOAD_TIME_2 = 1.800f;
    public static int CLIP_SIZE_2 = 45;
    public static int BURST_SIZE_2 = 4;
    public static float BURST_DELAY_2 = 0.000f;
    public static float SPREAD_2 = 1.000f;

    #endregion CONSTANTS

    public override void Start()
    {
        base.Start();
        projectileType = new ProjectileBaseScript[3]
        {
            (ProjectileBaseScript)Resources.Load(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.HeavyCannonBulletLevel0], typeof(ProjectileBaseScript)),
            (ProjectileBaseScript)Resources.Load(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.HeavyCannonBulletLevel1], typeof(ProjectileBaseScript)),
            (ProjectileBaseScript)Resources.Load(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.HeavyCannonBulletLevel2], typeof(ProjectileBaseScript))
        };
        for (int i = 0; i < projectileType.Length; i++)
        {
            ObjectPool.CreatePool(projectileType[i]);
        }
        sound = new AudioClip[5]
        {
            Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.CannonFire01]),
            Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.CannonFire02]),
            Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.CannonFire03]),
            Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.CannonFire04]),
            Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.CannonFire05]),
        };
        reloadClip = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.MachineGunReload02]);
        //audio.clip = sound;
        SetLevel(0);
        burstFireWeapon = true;

        casing = Resources.Load<ShellCasing>(KBConstants.ObjectConstants.PREFAB_NAMES[KBConstants.ObjectConstants.type.HCannonShellCasing]);
        if (casing != null)
        {
            ObjectPool.CreatePool(casing);
        }
    }

    public override int SetLevel(int level)
    {
        switch (level)
        {
            case 0:
                cooldown = COOLDOWN_0;
                SetMaxRange(RANGE_0);
                cooldownStart = COOLDOWN_0;
                ammo = CLIP_SIZE_0;
                reloadTime = RELOAD_TIME_0;
                clipSize = CLIP_SIZE_0;
                burstSize = BURST_SIZE_0;
                burstDelay = BURST_DELAY_0;
                maximumSpreadAngle = SPREAD_0;
                particleSystem.startSize = 1.25f;
                particleSystem.startLifetime = 1.250f;
                return level;

            case 1:
                cooldown = COOLDOWN_1;
                SetMaxRange(RANGE_1);
                cooldownStart = COOLDOWN_1;
                ammo = CLIP_SIZE_1;
                reloadTime = RELOAD_TIME_1;
                clipSize = CLIP_SIZE_1;
                burstSize = BURST_SIZE_1;
                burstDelay = BURST_DELAY_1;
                maximumSpreadAngle = SPREAD_1;
                particleSystem.startSize = 2.0f;
                particleSystem.startLifetime = 2.0f;
                return level;

            case 2:
                cooldown = COOLDOWN_2;
                SetMaxRange(RANGE_2);
                cooldownStart = COOLDOWN_2;
                ammo = CLIP_SIZE_2;
                reloadTime = RELOAD_TIME_2;
                clipSize = CLIP_SIZE_2;
                burstSize = BURST_SIZE_2;
                burstDelay = BURST_DELAY_2;
                maximumSpreadAngle = SPREAD_2;
                particleSystem.startSize = 6.5f;
                particleSystem.startLifetime = 3.0f;

                return level;

            default:
                return -1;
        }
    }
}