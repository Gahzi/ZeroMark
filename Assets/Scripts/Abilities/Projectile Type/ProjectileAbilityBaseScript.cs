using UnityEngine;
using System.Collections;

/// <summary>
/// Basic projectile ability class. Fires attached ammo type @ firerate
/// in the direction the source obj is "aiming" (Vec3)
/// Plays sound on fire.
/// </summary>
public class ProjectileAbilityBaseScript : AbilitySlotBaseScript
{

    protected ProjectileBaseScript projectileType;
    private int maxRange;

    public override void Start()
    {
        base.Start();
    }
    
    void Update()
    {
        if (abilityActive)
        {
            if (!cooldownTimer.IsTimerActive(cooldownTimerNumber))
            {
                Fire(maxRange);
            }
        }
    }

    public ProjectileBaseScript Fire(Vector3 direction)
    {
        ProjectileBaseScript projectile = (ProjectileBaseScript)Instantiate(projectileType, transform.position, Quaternion.Euler(direction));
        audio.Play();
        cooldownTimerNumber = cooldownTimer.StartTimer(cooldown);

        Collider[] collider = transform.parent.GetComponentsInChildren<Collider>();
        foreach (Collider c in collider)
        {
            if (c.enabled)
            {
                Physics.IgnoreCollision(c, projectile.collider, true); 
            }

        }
        return projectile;
    }

    public ProjectileBaseScript Fire()
    {
        return Fire(transform.rotation.eulerAngles);
    }

    public ProjectileBaseScript Fire(int maxRange)
    {
        ProjectileBaseScript p = Fire();
        p.setLifetimeForMaxRange(maxRange);
        return p;
    }

    public void SetMaxRange(int maxRange)
    {
        this.maxRange = maxRange;
    }

}
