using System.Collections.Generic;

namespace KBConstants
{
    public enum Team { Red, Blue, None };
    public enum ItemType { common, uncommon, rare, legendary, undefined };

    public class ObjectConstants
    {
        public enum type { Core, Player, Gamepad, Item, Camera, PlasmaBullet, Kaiju, BasicRigidbodyCube, GUISelectCube, Factory, FactoryGroup, Tower, ItemZone };

        private static readonly IDictionary<type, string> prefabNames = new Dictionary<type, string>
        {
            {type.Player,"Player"},
			{type.Gamepad, "Gamepad"},
            {type.Item, "Items/Item"},
            {type.Camera, "Camera"},
            {type.PlasmaBullet, "Abilities/PlasmaBullet"},
            {type.Kaiju, "Kaiju/Kaiju"},
            {type.BasicRigidbodyCube, "Environment/BasicRigidbodyCube"},
            {type.GUISelectCube, "GUI/GUISelectCube"},
            {type.Factory, "Factory"},
            {type.FactoryGroup, "FactoryGroup"},
            {type.Tower, "Tower/Tower"},
            {type.ItemZone, "Items/ItemZone"},
            {type.Core, "Core"}
        };
        public static IDictionary<type, string> PREFAB_NAMES { get { return prefabNames; } }
    }

    public class ManagerConstants
    {
        public enum type { GameManager, GamepadManager };

        private static readonly IDictionary<type, string> prefabNames = new Dictionary<type, string>
        {
            {type.GameManager,"GameManager"},
			{type.GamepadManager, "GamepadManager"}
        };
        public static IDictionary<type, string> PREFAB_NAMES { get { return prefabNames; } }

        private static readonly IDictionary<type, string> prefabTags = new Dictionary<type, string>
        {
            {type.GameManager,"GameManager"},
			{type.GamepadManager, "GamepadManager"}
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
        public enum clip { ItemGrab, FactoryItemAccept, FactoryItemFinish, TowerAmbient, CaptureProgress, CaptureComplete, TowerSpawn, TowerDie, PlasmaGunFire };

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
            {clip.PlasmaGunFire, "sounds/cyberstorm select/tower fire/artillery-21"}
        };

        public static IDictionary<clip, string> CLIP_NAMES { get { return clipNames; } }
    }
}

