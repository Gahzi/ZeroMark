using KBConstants;
using UnityEngine;

public class MachineGun : ProjectileAbilityBaseScript
{
    #region CONSTANTS

    public static float COOLDOWN_0 = 0.550f;
    public static int RANGE_0 = 100;
    public static float RELOAD_TIME_0 = 1.900f;
    public static int CLIP_SIZE_0 = 12;
    public static int BURST_SIZE_0 = 2;
    public static float BURST_DELAY_0 = 0.050f;
    public static float SPREAD_0 = 0.500f;

    public static float COOLDOWN_1 = 0.5235f;
    public static int RANGE_1 = 100;
    public static float RELOAD_TIME_1 = 0.900f;
    public static int CLIP_SIZE_1 = 12;
    public static int BURST_SIZE_1 = 3;
    public static float BURST_DELAY_1 = 0.0450f;
    public static float SPREAD_1 = 0.600f;

    public static float COOLDOWN_2 = 0.520f;
    public static int RANGE_2 = 100;
    public static float RELOAD_TIME_2 = 0.800f;
    public static int CLIP_SIZE_2 = 12;
    public static int BURST_SIZE_2 = 3;
    public static float BURST_DELAY_2 = 0.0350f;
    public static float SPREAD_2 = 0.700f;

    #endregion CONSTANTS

    public override void Start()
    {
        projectileType = new ProjectileBaseScript[3]
        {
            (ProjectileBaseScript)Resources.Load(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.MachinegunBulletLevel0], typeof(ProjectileBaseScript)),
            (ProjectileBaseScript)Resources.Load(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.MachinegunBulletLevel1], typeof(ProjectileBaseScript)),
            (ProjectileBaseScript)Resources.Load(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.MachinegunBulletLevel2], typeof(ProjectileBaseScript))
        };
        for (int i = 0; i < projectileType.Length; i++)
        {
            ObjectPool.CreatePool(projectileType[i]);
        }
        sound = new AudioClip[1]
        {
            Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.MachineGunFire01])
        };
        reloadClip = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.MachineGunReload01]);

        SetLevel(0);

        burstFireWeapon = true;

        casing = Resources.Load<ShellCasing>(KBConstants.ObjectConstants.PREFAB_NAMES[KBConstants.ObjectConstants.type.MachineGunShellCasing]);
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
                particleSystem.startSize = 0.3f;
                sound = new AudioClip[1]
                {
                    Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.MachineGunFire01])
                };
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
                particleSystem.startSize = 1.2f;
                sound = new AudioClip[1]
                {
                    Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.MachineGunFire02])
                };
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
                particleSystem.startSize = 2.5f;
                sound = new AudioClip[1]
                {
                    Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.MachineGunFire02])
                };
                return level;

            default:
                return -1;
        }
    }
}