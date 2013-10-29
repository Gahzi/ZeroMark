﻿using System.Collections.Generic;

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
}

