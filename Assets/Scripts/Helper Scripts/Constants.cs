using System.Collections.Generic;

namespace KBConstants {
	
	public class ObjectConstants {
		public enum type { Player,Gamepad };
		
		private static readonly IDictionary<type, string> prefabNames = new Dictionary<type, string>
        {
            {type.Player,"Player"},
			{type.Gamepad, "Gamepad"}
        };
        public static IDictionary<type, string> PREFAB_NAMES { get { return prefabNames; } }
	}

    public class PlayerConstants {
        public static float PLAYER_MOVEMENT_SPEED = 0.25f;

    }

    public class GameManagerConstants {
        
    }
}

