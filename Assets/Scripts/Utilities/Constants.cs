using System.Collections.Generic;

namespace KBConstants
{
    public enum Team { Red = 0, Blue = 1, None = 2 };

    public enum PlayerMode { Player = 0, Spectator = 1 };
    public enum PlayerType { mech, drone, tank, core };

    public class StringConstants
    {
        public static readonly string mechPrefix = "MCH.";
        public static readonly string tankPrefix = "TNK.";
        public static readonly string hunterPrefix = "HTR.";

        public static readonly string mech0Name = "ASLT";
        public static readonly string mech1Name = "PLSR";
        public static readonly string mech2Name = "LONG";

        public static readonly string tank0Name = "HEAT";
        public static readonly string tank1Name = "DRVR";
        public static readonly string tank2Name = "TRAP";

        public static readonly string hunter0Name = "BRST";
        public static readonly string hunter1Name = "RCPX";
        public static readonly string hunter2Name = "PLSR";
    }

    public class GameConstants
    {
        public static readonly float pregameTime = 30.0f;
        public static readonly int maxGameTimeCapturePoint = 60 * 3;
        public static readonly int maxGameTimeDeathmatch = 60 * 3;
        public static readonly int maxScoreDeathmatch = 30;
        public static readonly int masScoreCapturePoint = 100;
        public static readonly int maxGameTimeDataPulse = 60 * 3;
        public static readonly int dataPulsePeriod = 60;
        public static readonly int pointObjectDecayPeriod = 5;
        public static readonly int pointObjectDecayPercentPerPeriod = 0;
        public static readonly float pointPercentDropOnDeath = 1.0f;
        public static readonly int levelOneThreshold = 5;
        public static readonly int levelTwoThreshold = 29;
        public static readonly int decalLifetime = 10;
        public static readonly int explosionChaffAmount = 3; // Amount of chaff is this value ^ 3
        public static readonly int explosionParticleAmount = 5000;
        public static readonly int defaultAOElifetime = 1;
    }

    public class AbilityConstants
    {
        public enum type
        {
            MachinegunLevel0, MachinegunLevel1, MachinegunLevel2,
            RocketLevel0, RocketLevel1, RocketLevel2,
            PlasmaLevel0, PlasmaLevel1, PlasmaLevel2,
            LightAutoLaser,
            HeavyCannonLevel0, HeavyCannonLevel1, HeavyCannonLevel2,
            LightCannonLevel0, LightCannonLevel1, LightCannonLevel2,
            HomingRocketLevel0, HomingRocketLevel1, HomingRocketLevel2,
            ShotgunLevel0, ShotgunLevel1, ShotgunLevel2,
            SniperLevel0, SniperLevel1, SniperLevel2,
            MineLevel0, MineLevel1, MineLevel2,
            RailgunLevel0, RailgunLevel1, RailgunLevel2
        };

        private static readonly IDictionary<type, int> damageValues = new Dictionary<type, int>
        {
            {type.MachinegunLevel0, 100},
            {type.MachinegunLevel1, 120},
            {type.MachinegunLevel2, 140},

            {type.HeavyCannonLevel0, 30},
            {type.HeavyCannonLevel1, 40},
            {type.HeavyCannonLevel2, 50},

            {type.ShotgunLevel0, 40},
            {type.ShotgunLevel1, 40},
            {type.ShotgunLevel2, 40},

            {type.PlasmaLevel0, 100},
            {type.PlasmaLevel1, 50},
            {type.PlasmaLevel2, 40},

            {type.RocketLevel0, 200},
            {type.RocketLevel1, 200},
            {type.RocketLevel2, 200},

            {type.LightCannonLevel0, 60},
            {type.LightCannonLevel1, 60},
            {type.LightCannonLevel2, 70},

            {type.HomingRocketLevel0, 35},
            {type.HomingRocketLevel1, 150},
            {type.HomingRocketLevel2, 200},

            {type.SniperLevel0, 445},
            {type.SniperLevel1, 450},
            {type.SniperLevel2, 450},
            
            {type.MineLevel0, 200},
            {type.MineLevel1, 200},
            {type.MineLevel2, 200},

            {type.RailgunLevel0, 200},
            {type.RailgunLevel1, 200},
            {type.RailgunLevel2, 200}
        };

        public static IDictionary<type, int> DAMAGE_VALUES { get { return damageValues; } }

        private static readonly IDictionary<type, float> speedValues = new Dictionary<type, float>
        {
            {type.MachinegunLevel0, 50.0f},
            {type.MachinegunLevel1, 50.0f},
            {type.MachinegunLevel2, 50.0f},
        };

        public static IDictionary<type, float> SPEED_VALUES { get { return speedValues; } }
    }

    public class ObjectConstants
    {
        public enum type
        {
            Player, Spectator, Gamepad, Item, PlayerCamera, BasicRigidbodyCube,
            MachinegunBulletLevel0, MachinegunBulletLevel1, MachinegunBulletLevel2,
            RocketBulletLevel0, RocketBulletLevel1, RocketBulletLevel2,
            PlasmaBulletLevel0, PlasmaBulletLevel1, PlasmaBulletLevel2,
            LightAutoLaserBullet,
            HeavyCannonBulletLevel0, HeavyCannonBulletLevel1, HeavyCannonBulletLevel2,
            LightCannonBulletLevel0, LightCannonBulletLevel1, LightCannonBulletLevel2,
            ShotgunLevel0, ShotgunLevel1, ShotgunLevel2,
            SniperBulletLevel0, SniperBulletLevel1, SniperBulletLevel2,
            RailgunBulletLevel0, RailgunBulletLevel1, RailgunBulletLevel2,
            MineLevel0, MineLevel1, MineLevel2,
            HomingRocketL0,
            KillTagBlue, KillTagRed,
            PlasmaExplosionL0, PlasmaExplosionL1, PlasmaExplosionL2,
            SmallExplosion, RocketExplosion, NoDamageExplosionMedium,
            FloatingText,
            MachineGunShellCasing, HCannonShellCasing, SniperShellCasing
        };

        private static readonly IDictionary<type, string> prefabNames = new Dictionary<type, string>
        {
            {type.Player,"Characters/PlayerLocal"},
            {type.Spectator, "Characters/SpectatorLocal"},
			{type.Gamepad, "Gamepads/Gamepad"},
            {type.Item, "Items/Item"},
            {type.PlayerCamera, "Cameras/Player Camera"},
            {type.PlasmaBulletLevel0, "Abilities/bullet/PlasmaBulletl0"},
            {type.PlasmaBulletLevel1, "Abilities/bullet/PlasmaBulletl1"},
            {type.PlasmaBulletLevel2, "Abilities/bullet/PlasmaBulletl2"},
            {type.BasicRigidbodyCube, "Environment/BasicRigidbodyCube"},
            {type.MachinegunBulletLevel0, "Abilities/bullet/machinegunbulletl0"},
            {type.MachinegunBulletLevel1, "Abilities/bullet/machinegunbulletl1"},
            {type.MachinegunBulletLevel2, "Abilities/bullet/machinegunbulletl2"},
            {type.RocketBulletLevel0, "abilities/bullet/rocketbulletl0"},
            {type.RocketBulletLevel1, "abilities/bullet/rocketbulletl1"},
            {type.RocketBulletLevel2, "abilities/bullet/rocketbulletl2"},
            {type.KillTagBlue, "items/killtagblue"},
            {type.KillTagRed, "items/killtagred"},
            {type.LightAutoLaserBullet, "abilities/bullet/lightautolaserbullet"},
            {type.SmallExplosion, "abilities/explosion/smallexplosion"},
            {type.RocketExplosion, "abilities/explosion/rocketexplosion"},
            {type.NoDamageExplosionMedium, "abilities/explosion/mediumexplosion"},
            {type.PlasmaExplosionL0, "abilities/explosion/plasmaexplosionl0"},
            {type.PlasmaExplosionL1, "abilities/explosion/plasmaexplosionl1"},
            {type.PlasmaExplosionL2, "abilities/explosion/plasmaexplosionl2"},

            {type.FloatingText, "gui/floatingtext"},
            {type.HeavyCannonBulletLevel0, "abilities/bullet/heavycannonbulletl0"},
            {type.HeavyCannonBulletLevel1, "abilities/bullet/heavycannonbulletl1"},
            {type.HeavyCannonBulletLevel2, "abilities/bullet/heavycannonbulletl2"},
            {type.LightCannonBulletLevel0, "abilities/bullet/lightcannonbulletl0"},
            {type.LightCannonBulletLevel1, "abilities/bullet/lightcannonbulletl1"},
            {type.LightCannonBulletLevel2, "abilities/bullet/lightcannonbulletl2"},
            {type.ShotgunLevel0, "abilities/bullet/shotgunbulletl0"},
            {type.ShotgunLevel1, "abilities/bullet/shotgunbulletl1"},
            {type.ShotgunLevel2, "abilities/bullet/shotgunbulletl2"},
            {type.HomingRocketL0, "abilities/bullet/homingrocketbulletl0"},
            {type.MachineGunShellCasing, "abilities/machinegunshellcasing"},
            {type.HCannonShellCasing, "abilities/HCannonShellCasing"},
            {type.SniperShellCasing, "abilities/snipershellcasing"},
            {type.SniperBulletLevel0, "abilities/bullet/sniperbulletl0"},
            {type.SniperBulletLevel1, "abilities/bullet/sniperbulletl1"},
            {type.SniperBulletLevel2, "abilities/bullet/sniperbulletl2"},
            {type.RailgunBulletLevel0, "abilities/bullet/railgunbulletl0"},
            {type.RailgunBulletLevel1, "abilities/bullet/railgunbulletl1"},
            {type.RailgunBulletLevel2, "abilities/bullet/railgunbulletl2"},
            {type.MineLevel0, "abilities/bullet/minebulletl0"},
            {type.MineLevel1, "abilities/bullet/minebulletl1"},
            {type.MineLevel2, "abilities/bullet/minebulletl2"},
        };

        public static IDictionary<type, string> PREFAB_NAMES { get { return prefabNames; } }
    }

    public class ManagerConstants
    {
        public enum type { GameManager, GamepadManager, UpgradePointReqs };

        private static readonly IDictionary<type, string> prefabNames = new Dictionary<type, string>
        {
            {type.GameManager,"Managers/GameManager"},
			{type.GamepadManager, "Managers/GamepadManager"},
            {type.UpgradePointReqs, "UpgradePointReqs.csv"}
        };

        public static IDictionary<type, string> PREFAB_NAMES { get { return prefabNames; } }

        private static readonly IDictionary<type, string> prefabTags = new Dictionary<type, string>
        {
            {type.GameManager,"GameManager"},
			{type.GamepadManager, "GamepadManager"},
            {type.UpgradePointReqs, "UpgradePointReqs"}
        };

        public static IDictionary<type, string> PREFAB_TAGS { get { return prefabTags; } }
    }

    public class MaterialConstants
    {
        public enum type { ItemCommon, ItemUncommon, ItemRare, ItemLegendary, CoreBlue, CoreRed };

        private static readonly IDictionary<type, string> materialNames = new Dictionary<type, string>
        {
            {type.ItemCommon, "Materials/Visual/Items/ItemCommon"},
            {type.ItemUncommon, "Materials/Visual/Items/ItemUncommon"},
            {type.ItemRare, "Materials/Visual/Items/ItemRare"},
            {type.ItemLegendary, "Materials/Visual/Items/ItemLegendary"},
            {type.CoreBlue, "Materials/Visual/Core/CoreBlueMaterial"},
            {type.CoreRed, "Materials/Visual/Core/CoreRedMaterial"}
        };

        public static IDictionary<type, string> MATERIAL_NAMES { get { return materialNames; } }
    }

    public class AudioConstants
    {
        public enum clip
        {
            ItemPickup01,
            PlasmaGunFire01,
            PlasmaGunFire02,
            MachineGunFire01,
            MachineGunFire02,
            LightCannonFire01,
            LightCannonFire02,
            LightCannonFire03,
            CannonFire01,
            CannonFire02,
            CannonFire03,
            CannonFire04,
            CannonFire05,
            CannonReload01,
            HitConfirm,
            MachineGunReload01, MachineGunReload02,
            RocketFire01,
            PlasmaReload01,
            SniperFire01,
            MineFire01, MineFire02
        };

        private static readonly IDictionary<clip, string> clipNames = new Dictionary<clip, string>
        {
            {clip.PlasmaGunFire01, "sounds/cyberstorm select/gun single shots/subgun-04"},
            {clip.PlasmaGunFire02, "sounds/cyberstorm select/gun single shots/subgun-05"},            
            {clip.HitConfirm, "sounds/hitmarker"},
            {clip.MachineGunFire01, "sounds/cyberstorm select/cuts/machinegun_double_01"},
            {clip.MachineGunFire02, "sounds/cyberstorm select/cuts/machinegun_triple_01"},
            {clip.RocketFire01, "sounds/cyberstorm select/gun single shots/rocket-07"},
            {clip.MachineGunReload01, "sounds/cyberstorm select/reload/machine-sequence-03"},
            {clip.MachineGunReload02, "sounds/cyberstorm select/reload/zm_audio_reload_01"},
            {clip.PlasmaReload01, "sounds/cyberstorm select/reload/tr_2-reployservo-01"},
            {clip.ItemPickup01, "sounds/cyberstorm select/item attach/interface-servos-06"},
            {clip.LightCannonFire01, "sounds/cyberstorm select/cuts/laserfire_01"},
            {clip.CannonFire01, "sounds/cyberstorm select/gun single shots/subgun-16"},
            {clip.CannonFire02, "sounds/cyberstorm select/gun single shots/subgun-17"},
            {clip.CannonFire03, "sounds/cyberstorm select/gun single shots/subgun-18"},
            {clip.CannonFire04, "sounds/cyberstorm select/gun single shots/subgun-19"},
            {clip.CannonFire05, "sounds/cyberstorm select/gun single shots/subgun-20"},
            {clip.SniperFire01, "sounds/cyberstorm select/gun single shots/artillery-21"},
            {clip.MineFire01, "sounds/cyberstorm select/cuts/mine_single_01"},
            {clip.MineFire02, "sounds/cyberstorm select/cuts/mine_single_02"}
        };

        public static IDictionary<clip, string> CLIP_NAMES { get { return clipNames; } }
    }
}