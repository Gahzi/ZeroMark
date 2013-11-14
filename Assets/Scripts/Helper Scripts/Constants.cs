using System.Collections.Generic;

namespace KBConstants {
	
	public class ObjectConstants {
		public enum type { Player,Gamepad,Item,Camera };
		
		private static readonly IDictionary<type, string> prefabNames = new Dictionary<type, string>
        {
            {type.Player,"Player"},
			{type.Gamepad, "Gamepad"},
            {type.Item, "Item"},
            {type.Camera, "Camera"}
        };
        public static IDictionary<type, string> PREFAB_NAMES { get { return prefabNames; } }
	}

    public class PlayerConstants {
        public static float PLAYER_MOVEMENT_SPEED = 0.25f;

        public static enum coreNames { red, blue, green, black };

    }

    public class KaijuConstants
    {
        public static int CORE_HEALTH_DEFAULT = 100;

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

