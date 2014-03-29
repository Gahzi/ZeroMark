﻿using System.Collections.Generic;

namespace KBConstants
{
    public enum Team { Red, Blue, None };

    public enum PlayerType { mech, drone, tank, core };

    public enum ItemType { common, uncommon, rare, legendary, undefined };

    public class ObjectConstants
    {
        public enum type
        {
            Player, Gamepad, Item, PlayerCamera,  BasicRigidbodyCube,
            MachinegunBullet, Rocket, PlasmaBullet, LightAutoLaserBullet, HeavyCannonBullet, LightCannonBullet,
            KillTagBlue, KillTagRed, 
            SmallExplosion, RocketExplosion, NoDamageExplosionMedium,
            FloatingText
        };

        private static readonly IDictionary<type, string> prefabNames = new Dictionary<type, string>
        {
            {type.Player,"Characters/PlayerLocal"},
			{type.Gamepad, "Gamepads/Gamepad"},
            {type.Item, "Items/Item"},
            {type.PlayerCamera, "Cameras/Player Camera"},
            {type.PlasmaBullet, "Abilities/bullet/PlasmaBullet"},
            {type.BasicRigidbodyCube, "Environment/BasicRigidbodyCube"},
            {type.MachinegunBullet, "Abilities/bullet/machinegunbullet"},
            {type.Rocket, "abilities/bullet/rocketbullet"},
            {type.KillTagBlue, "items/killtagblue"},
            {type.KillTagRed, "items/killtagred"},
            {type.LightAutoLaserBullet, "abilities/bullet/lightautolaserbullet"},
            {type.SmallExplosion, "abilities/explosion/smallexplosion"},
            {type.RocketExplosion, "abilities/explosion/rocketexplosion"},
            {type.NoDamageExplosionMedium, "abilities/explosion/mediumexplosion"},
            {type.FloatingText, "gui/floatingtext"},
            {type.HeavyCannonBullet, "abilities/bullet/heavycannonbullet"},
            {type.LightCannonBullet, "abilities/bullet/lightcannonbullet"}
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
        public enum clip { ItemPickup01,
            PlasmaGunFire01,
            MachineGunFire01,
            CannonFire01,
            CannonReload01,
            HitConfirm,
            MachineGunReload01, MachineGunReload02,
            RocketFire01,
            PlasmaReload01 };

        private static readonly IDictionary<clip, string> clipNames = new Dictionary<clip, string>
        {
            {clip.PlasmaGunFire01, "sounds/cyberstorm select/gun single shots/artillery-08"},
            {clip.HitConfirm, "sounds/hitmarker"},
            {clip.MachineGunFire01, "sounds/cyberstorm select/gun single shots/gun-01"},
            {clip.RocketFire01, "sounds/cyberstorm select/gun single shots/rocket-07"},
            {clip.MachineGunReload01, "sounds/cyberstorm select/reload/machine-sequence-03"},
            {clip.MachineGunReload02, "sounds/cyberstorm select/reload/machine-sequence-04"},
            {clip.PlasmaReload01, "sounds/cyberstorm select/reload/tr_2-reployservo-01"},
            {clip.ItemPickup01, "sounds/cyberstorm select/item attach/interface-servos-06"},
            {clip.CannonFire01, "sounds/cyberstorm select/gun single shots/gun-02"}
        };

        public static IDictionary<clip, string> CLIP_NAMES { get { return clipNames; } }
    }
}