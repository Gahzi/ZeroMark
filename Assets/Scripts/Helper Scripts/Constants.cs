using System.Collections.Generic;

namespace KBConstants {
	
	public class ObjectConstants {
		public enum type { Player,Gamepad,Item,Camera,PlasmaBullet,Kaiju };
		
		private static readonly IDictionary<type, string> prefabNames = new Dictionary<type, string>
        {
            {type.Player,"Player"},
			{type.Gamepad, "Gamepad"},
            {type.Item, "Item"},
            {type.Camera, "Camera"},
            {type.PlasmaBullet, "Abilities/PlasmaBullet"},
            {type.Kaiju, "Kaiju/Kaiju"}
        };
        public static IDictionary<type, string> PREFAB_NAMES { get { return prefabNames; } }
	}

    public class ManagerConstants {
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
}

