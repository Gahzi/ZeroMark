using System.Collections.Generic;

namespace KBConstants
{
    public enum Team { Red, Blue, None };

    public enum PlayerType { mech, drone, tank };

    public enum ItemType { common, uncommon, rare, legendary, undefined };

    public class ObjectConstants
    {
        public enum type { Player, Gamepad, Item, PlayerCamera, PlasmaBullet, BasicRigidbodyCube, MachinegunBullet, Rocket, KillTagBlue, KillTagRed };

        private static readonly IDictionary<type, string> prefabNames = new Dictionary<type, string>
        {
            {type.Player,"Characters/PlayerLocal"},
			{type.Gamepad, "Gamepads/Gamepad"},
            {type.Item, "Items/Item"},
            {type.PlayerCamera, "Cameras/Player Camera"},
            {type.PlasmaBullet, "Abilities/PlasmaBullet"},
            {type.BasicRigidbodyCube, "Environment/BasicRigidbodyCube"},
            {type.MachinegunBullet, "Abilities/machinegunbullet"},
            {type.Rocket, "abilities/rocket"},
            {type.KillTagBlue, "items/killtagblue"},
            {type.KillTagRed, "items/killtagred"}
        };

        public static IDictionary<type, string> PREFAB_NAMES { get { return prefabNames; } }
    }

    public class ManagerConstants
    {
        public enum type { GameManager, GamepadManager, PlayerStats, UpgradePointReqs };

        private static readonly IDictionary<type, string> prefabNames = new Dictionary<type, string>
        {
            {type.GameManager,"Managers/GameManager"},
			{type.GamepadManager, "Managers/GamepadManager"},
            {type.PlayerStats, "PlayerTypeData.csv"},
            {type.UpgradePointReqs, "UpgradePointReqs.csv"}
        };

        public static IDictionary<type, string> PREFAB_NAMES { get { return prefabNames; } }

        private static readonly IDictionary<type, string> prefabTags = new Dictionary<type, string>
        {
            {type.GameManager,"GameManager"},
			{type.GamepadManager, "GamepadManager"},
            {type.PlayerStats, "PlayerStats"},
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
        public enum clip { ItemGrab, FactoryItemAccept, FactoryItemFinish, TowerAmbient, CaptureProgress, CaptureComplete, TowerSpawn, TowerDie, PlasmaGunFire, HitConfirm };

        private static readonly IDictionary<clip, string> clipNames = new Dictionary<clip, string>
        {
            {clip.ItemGrab, "Sounds/CyberStorm Select/sling/TR-BodyServos-Move-02"},
            {clip.FactoryItemAccept, "Sounds/CyberStorm Select/item attach/INTERFACE-SERVOS-06"},
            {clip.TowerAmbient, "Sounds/CyberStorm Select/capture progress/MACHINE-ELECTRICARC"},
            {clip.FactoryItemFinish, "Sounds/CyberStorm Select/factory output/T-SERVO-05"},
            {clip.CaptureProgress, "sounds/cyberstorm select/capture progress/t-rumble-idleloop-04"},
            {clip.CaptureComplete, "sounds/cyberstorm select/capture success/system on-off-02"},
            {clip.TowerSpawn, ""},
            {clip.TowerDie, "sounds/cyberstorm select/tower damage/impact-metal-12"},
            {clip.PlasmaGunFire, "sounds/cyberstorm select/tower fire/artillery-21"},
            {clip.HitConfirm, "sounds/hitmarker"}
        };

        public static IDictionary<clip, string> CLIP_NAMES { get { return clipNames; } }
    }
}