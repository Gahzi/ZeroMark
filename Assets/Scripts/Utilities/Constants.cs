using System.Collections.Generic;

namespace KBConstants
{
    public enum Team { Red = 0, Blue = 1, None = 2 };

    public enum PlayerType { mech, drone, tank, core };

    public enum ItemType { common, uncommon, rare, legendary, undefined };

    public class GameConstants
    {
        public static readonly float pointPercentDropOnDeath = 1.0f;
        public static readonly int levelOneThreshold = 4;
        public static readonly int levelTwoThreshold = 50;
        public static readonly int decalLifetime = 5;
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
            ShotgunLevel0, ShotgunLevel1, ShotgunLevel2
        };

        private static readonly IDictionary<type, int> damageValues = new Dictionary<type, int>
        {
            {type.MachinegunLevel0, 80},
            {type.MachinegunLevel1, 100},
            {type.MachinegunLevel2, 120},

            {type.HeavyCannonLevel0, 50},
            {type.HeavyCannonLevel1, 50},
            {type.HeavyCannonLevel2, 60},

            {type.ShotgunLevel0, 30},
            {type.ShotgunLevel1, 50},
            {type.ShotgunLevel2, 70},

            {type.PlasmaLevel0, 100},
            {type.PlasmaLevel1, 50},
            {type.PlasmaLevel2, 50},

            {type.RocketLevel0, 75},
            {type.RocketLevel1, 100},
            {type.RocketLevel2, 300},

            {type.LightCannonLevel0, 15},
            {type.LightCannonLevel1, 30},
            {type.LightCannonLevel2, 45},

            {type.HomingRocketLevel0, 75},
            {type.HomingRocketLevel1, 150},
            {type.HomingRocketLevel2, 200},
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
            Player, Gamepad, Item, PlayerCamera, BasicRigidbodyCube,
            MachinegunBulletLevel0, MachinegunBulletLevel1, MachinegunBulletLevel2,
            RocketBulletLevel0, RocketBulletLevel1, RocketBulletLevel2,
            PlasmaBulletLevel0, PlasmaBulletLevel1, PlasmaBulletLevel2,
            LightAutoLaserBullet,
            HeavyCannonBulletLevel0, HeavyCannonBulletLevel1, HeavyCannonBulletLevel2,
            LightCannonBulletLevel0, LightCannonBulletLevel1, LightCannonBulletLevel2,
            ShotgunLevel0, ShotgunLevel1, ShotgunLevel2,
            HomingRocketL0,
            KillTagBlue, KillTagRed,
            PlasmaExplosionL0, PlasmaExplosionL1, PlasmaExplosionL2,
            SmallExplosion, RocketExplosion, NoDamageExplosionMedium,
            FloatingText,
            MachineGunShellCasing, HCannonShellCasing
        };

        private static readonly IDictionary<type, string> prefabNames = new Dictionary<type, string>
        {
            {type.Player,"Characters/PlayerLocal"},
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
            {type.HCannonShellCasing, "abilities/HCannonShellCasing"}
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
            PlasmaReload01
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
            {clip.MachineGunReload02, "sounds/cyberstorm select/reload/machine-sequence-04"},
            {clip.PlasmaReload01, "sounds/cyberstorm select/reload/tr_2-reployservo-01"},
            {clip.ItemPickup01, "sounds/cyberstorm select/item attach/interface-servos-06"},
            {clip.LightCannonFire01, "sounds/cyberstorm select/cuts/laserfire_01"},
            {clip.CannonFire01, "sounds/cyberstorm select/gun single shots/subgun-16"},
            {clip.CannonFire02, "sounds/cyberstorm select/gun single shots/subgun-17"},
            {clip.CannonFire03, "sounds/cyberstorm select/gun single shots/subgun-18"},
            {clip.CannonFire04, "sounds/cyberstorm select/gun single shots/subgun-19"},
            {clip.CannonFire05, "sounds/cyberstorm select/gun single shots/subgun-20"},
        };

        public static IDictionary<clip, string> CLIP_NAMES { get { return clipNames; } }
    }
}