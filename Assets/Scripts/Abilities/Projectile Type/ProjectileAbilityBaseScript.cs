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
        ObjectPool.CreatePool(projectileType);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected ProjectileBaseScript Fire(Vector3 direction, KBPlayer firedBy, float _inheritSpeed = 0.0f)
    {
        ProjectileBaseScript projectile = null;
        if (cooldown <= 0 && ammo > 0 && !reloading)
        {
            projectile = ObjectPool.Spawn(projectileType, transform.position, Quaternion.Euler(direction));
            projectile.inheritSpeed = _inheritSpeed;
            projectile.Team = firedBy.Team;
            projectile.Init(firedBy);
            if (audio.clip != null)
            {
                audio.PlayOneShot(audio.clip);
            }
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

    protected ProjectileBaseScript Fire(KBPlayer firedBy, float _inheritSpeed = 0)
    {
        return Fire(transform.rotation.eulerAngles, firedBy, _inheritSpeed);
    }

    protected ProjectileBaseScript Fire(int maxRange, KBPlayer firedBy, float _inheritSpeed = 0)
    {
        ProjectileBaseScript p = Fire(firedBy, _inheritSpeed);
        if (p != null)
        {
            p.setLifetimeForMaxRange(maxRange);
        }
        return p;
    }

    public void PlayerFire(float _inheritSpeed)
    {
        Fire(maxRange, owner, _inheritSpeed);
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
