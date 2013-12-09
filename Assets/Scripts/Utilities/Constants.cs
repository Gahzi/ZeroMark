using System.Collections.Generic;

namespace KBConstants
{
    public enum Team { Red, Blue, None };
    public enum ItemType { one, two, three, undefined };

    public class ObjectConstants
    {
        public enum type { Player, Gamepad, Item, Camera, PlasmaBullet, Kaiju, BasicRigidbodyCube, GUISelectCube, Factory, FactoryGroup, Tower };

        private static readonly IDictionary<type, string> prefabNames = new Dictionary<type, string>
        {
            {type.Player,"Player"},
			{type.Gamepad, "Gamepad"},
            {type.Item, "Item"},
            {type.Camera, "Camera"},
            {type.PlasmaBullet, "Abilities/PlasmaBullet"},
            {type.Kaiju, "Kaiju/Kaiju"},
            {type.BasicRigidbodyCube, "Environment/BasicRigidbodyCube"},
            {type.GUISelectCube, "GUI/GUISelectCube"},
            {type.Factory, "Factory"},
            {type.FactoryGroup, "FactoryGroup"},
            {type.Tower, "Tower/Tower"}
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
        public enum type { ItemOneMat, ItemTwoMat, ItemThreeMat };

        private static readonly IDictionary<type, string> materialNames = new Dictionary<type, string>
        {
            {type.ItemOneMat, "Materials/Visual/Items/ItemOne"},
            {type.ItemTwoMat, "Materials/Visual/Items/ItemTwo"},
            {type.ItemThreeMat, "Materials/Visual/Items/ItemThree"}
        };

        public static IDictionary<type, string> MATERIAL_NAMES { get { return materialNames; } }
    }
}

