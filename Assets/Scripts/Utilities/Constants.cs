using System.Collections.Generic;

namespace KBConstants
{
    public enum Team { Red, Blue, None };
    public enum ItemType { common, uncommon, rare, legendary, undefined };

    public class ObjectConstants
    {
        public enum type { Player, Gamepad, Item, Camera, PlasmaBullet, Kaiju, BasicRigidbodyCube, GUISelectCube, Factory, FactoryGroup, Tower, ItemZone };

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
            {type.ItemZone, "Items/ItemZone"}
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
        public enum type { ItemCommon, ItemUncommon, ItemRare, ItemLegendary };

        private static readonly IDictionary<type, string> materialNames = new Dictionary<type, string>
        {
            {type.ItemCommon, "Materials/Visual/Items/ItemCommon"},
            {type.ItemUncommon, "Materials/Visual/Items/ItemUncommon"},
            {type.ItemRare, "Materials/Visual/Items/ItemRare"},
            {type.ItemLegendary, "Materials/Visual/Items/ItemLegendary"}
        };

        public static IDictionary<type, string> MATERIAL_NAMES { get { return materialNames; } }
    }

    public class AudioConstants
    {
        public enum clip { ItemGrab, FactoryItemAccept, FactoryItemFinish, TowerAmbient };

        private static readonly IDictionary<clip, string> clipNames = new Dictionary<clip, string>
        {
            {clip.ItemGrab, "Sounds/CyberStorm Select/sling/TR-BodyServos-Move-02"},
            {clip.FactoryItemAccept, "Sounds/CyberStorm Select/item attach/INTERFACE-SERVOS-06"},
            {clip.TowerAmbient, "Sounds/CyberStorm Select/capture progress/MACHINE-ELECTRICARC"},
            {clip.FactoryItemFinish, "Sounds/CyberStorm Select/factory output/T-SERVO-05"}
        };

        public static IDictionary<clip, string> CLIP_NAMES { get { return clipNames; } }
    }
}

