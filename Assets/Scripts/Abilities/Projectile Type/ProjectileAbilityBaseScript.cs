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

    public ProjectileBaseScript[] projectileType = new ProjectileBaseScript[3];
    public int level;
    protected int maxRange;
    public int ammo;
    public bool reloading;
    protected AudioClip reloadClip;
    private int lastSetLevel;
    public float minimumSpreadAngle;
    public float maximumSpreadAngle;

    public override void Start()
    {
        base.Start();
        for (int i = 0; i < projectileType.Length; i++)
        {
            ObjectPool.CreatePool(projectileType[i]);
        }
        level = 0;
        minimumSpreadAngle = 0.0f;
        maximumSpreadAngle = 0.0f;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (owner.killTokens < 1)
        {
            level = 0;
        }
        else if (owner.killTokens > 0 && owner.killTokens <= 50)
        {
            level = 1;
        }
        else if (owner.killTokens > 50)
        {
            level = 2;
        }
        if (lastSetLevel != level)
        {
            lastSetLevel = SetLevel(level);
        }
    }

    protected ProjectileBaseScript Fire(Vector3 direction, KBPlayer firedBy, float _inheritSpeed = 0.0f)
    {
        ProjectileBaseScript projectile = null;
        if (cooldown <= 0 && ammo > 0 && !reloading)
        {
            if (Random.Range(0, 2) == 0)
            {
                direction.y += minimumSpreadAngle + Random.Range(0, maximumSpreadAngle - minimumSpreadAngle);
            }
            else
            {
                direction.y -= minimumSpreadAngle + Random.Range(0, maximumSpreadAngle - minimumSpreadAngle);
            }
            projectile = ObjectPool.Spawn(projectileType[level], transform.position, Quaternion.Euler(direction));
            projectile.inheritSpeed = _inheritSpeed;
            projectile.Team = firedBy.Team;
            projectile.Init(firedBy);
            projectile.damage = projectile.damageLevel[level];
            
            if (projectile.homingProjectile)
            {
                projectile.targetPlayer = GameManager.Instance.FindClosestPlayer(owner, 10, true);
            }
            if (projectile.aimedProjectile)
            {
                projectile.targetPosition = new Vector3(-owner.mousePlayerDiff.x, owner.transform.position.y, -owner.mousePlayerDiff.y);
            }
            if (audio.clip != null)
            {
                audio.PlayOneShot(audio.clip);
            }
            if (particleSystem != null)
            {
                StartCoroutine(ParticleBurstStaged());
            }
            cooldown = cooldownStart;
            available = false;

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
        if (reloadClip != null)
        {
            audio.PlayOneShot(reloadClip);
        }
        yield return new WaitForSeconds(reloadTime);
        ammo = clipSize;
        reloading = false;
    }

    public void PlayerTriggerReload()
    {
        if (!reloading)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator ParticleBurstStaged()
    {
        particleSystem.Emit(30);
        yield return new WaitForSeconds(0.05f);
        particleSystem.Emit(30);
        yield return new WaitForSeconds(0.05f);
        particleSystem.Emit(30);
        yield return new WaitForSeconds(0.05f);
        particleSystem.Emit(30);
    }

    /// <summary>
    /// Sets level and returns level set at
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public abstract int SetLevel(int level);

}
