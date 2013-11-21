using UnityEngine;
using System.Collections;
using KBConstants;

/// <summary>
/// PlasmaGuns are a ProjectileAbility that shoots PlasmaBullet prefabs
/// </summary>
public class PlasmaGun : ProjectileAbilityBaseScript
{

    #region CONSTANTS
    public static float PLASMAGUN_COOLDOWN = 1.0f;
    #endregion

    public override void Start()
    {
        base.Start();
        projectileType = (ProjectileBaseScript)Resources.Load(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.PlasmaBullet], typeof(ProjectileBaseScript));
        cooldown = PLASMAGUN_COOLDOWN;
    }

    public override void Use()
    {
        throw new System.NotImplementedException();
    }

    public override void Use(Vector3 direction)
    {
        base.Use(direction);
    }
}
