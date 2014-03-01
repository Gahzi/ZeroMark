using UnityEngine;
using System.Collections;
using KBConstants;

/// <summary>
/// Basic projectile ability class. Fires attached ammo type @ firerate
/// in the direction the source obj is "aiming" (Vec3)
/// Plays sound on fire.
/// </summary>
public abstract class ProjectileAbilityBaseScript : AbilitySlotBaseScript
{

    #region CONSTANTS
    protected float reloadTime;
    protected int clipSize;
    #endregion

    protected ProjectileBaseScript projectileType;
    private int maxRange;
    public int ammo;
    public bool reloading;

    public override void Start()
    {
        base.Start();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected ProjectileBaseScript Fire(Vector3 direction, KBPlayer firedBy)
    {
        ProjectileBaseScript projectile = null;
        if (cooldown <= 0 && ammo > 0 && !reloading)
        {
            projectile = (ProjectileBaseScript)Instantiate(projectileType, transform.position, Quaternion.Euler(direction));
            projectile.Team = firedBy.Team;
            projectile.owner = firedBy;
            audio.Play();
            cooldown = cooldownStart;
            ammo--;

            Collider[] collider = transform.parent.GetComponentsInChildren<Collider>();
            foreach (Collider c in collider)
            {
                if (c.enabled)
                {
                    Physics.IgnoreCollision(c, projectile.collider, true);
                }
            }
        }
        return projectile;
    }

    protected ProjectileBaseScript Fire(KBPlayer firedBy)
    {
        return Fire(transform.rotation.eulerAngles, firedBy);
    }

    protected ProjectileBaseScript Fire(int maxRange, KBPlayer firedBy)
    {
        ProjectileBaseScript p = Fire(firedBy);
        if (p != null)
        {
            p.setLifetimeForMaxRange(maxRange);
        }
        return p;
    }

    public void PlayerFire()
    {
        Fire(maxRange, owner);
    }

    public void SetMaxRange(int maxRange)
    {
        this.maxRange = maxRange;
    }

    protected IEnumerator Reload()
    {
        reloading = true;
        yield return new WaitForSeconds(reloadTime);
        ammo = clipSize;
        reloading = false;
    }

    public void PlayerTriggerReload()
    {
        StartCoroutine(Reload());
    }
}
