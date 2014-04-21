using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
    public int clipSize;
    #endregion

    public ProjectileBaseScript[] projectileType = new ProjectileBaseScript[3];
    public int level;
    protected int maxRange;
    public int ammo;
    public bool reloading;
    protected AudioClip reloadClip;
    private int lastSetLevel;
    protected float minimumSpreadAngle;
    protected float maximumSpreadAngle;
    protected float[] angleModifierArray;
    protected bool burstFireWeapon;
    public int burstSize;
    protected float burstDelay;
    private ProjectileBaseScript lastProjectile;

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
        burstSize = 1;
        burstDelay = 0;
        angleModifierArray = new float[0];
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (reloading || ammo <= 0)
        {
            available = false;
        }

        if (owner.killTokens < GameConstants.levelOneThreshold)
        {
            level = 0;
        }
        else if (owner.killTokens >= GameConstants.levelOneThreshold && owner.killTokens <= GameConstants.levelTwoThreshold)
        {
            level = 1;
        }
        else if (owner.killTokens > GameConstants.levelTwoThreshold)
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
        if (available)
        {
            available = false;
            cooldown = cooldownStart;
            ammo--;
            StartCoroutine(DoFiringCoroutine(direction, firedBy, _inheritSpeed));
        }
        projectile = lastProjectile;
        return projectile;
    }

    /// <summary>
    /// Fires projectile (does not alter lifetime)
    /// </summary>
    /// <param name="firedBy"></param>
    /// <param name="_inheritSpeed"></param>
    /// <returns></returns>
    protected ProjectileBaseScript Fire(KBPlayer firedBy, float _inheritSpeed = 0)
    {
        return Fire(transform.rotation.eulerAngles, firedBy, _inheritSpeed);
    }

    /// <summary>
    /// Fires and sets max range of projectile at runtime
    /// </summary>
    /// <param name="maxRange"></param>
    /// <param name="firedBy"></param>
    /// <param name="_inheritSpeed"></param>
    /// <returns></returns>
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

    private IEnumerator DoFiringCoroutine(Vector3 direction, KBPlayer firedBy, float _inheritSpeed = 0.0f)
    {
        for (int i = 0; i < burstSize; i++)
        {
            ProjectileBaseScript projectile = null;
            Vector3 modifiedDirection = direction;
            if (burstFireWeapon)
            {
                modifiedDirection.y = (direction.y - maximumSpreadAngle) + (i * 2.0f * maximumSpreadAngle / burstSize);

            }
            //else
            //{
            //    if (Random.Range(0, 2) == 0) // Adjust angle by spread value
            //    {
            //        direction.y += minimumSpreadAngle + Random.Range(0, maximumSpreadAngle - minimumSpreadAngle);
            //    }
            //    else
            //    {
            //        direction.y -= minimumSpreadAngle + Random.Range(0, maximumSpreadAngle - minimumSpreadAngle);
            //    }
            //}


            // Spawn projectile
            projectile = ObjectPool.Spawn(projectileType[level], transform.position, Quaternion.Euler(modifiedDirection));
            projectile.inheritSpeed = _inheritSpeed;
            projectile.Team = firedBy.Team;
            projectile.Init(firedBy);
            projectile.damage = projectile.damageLevel[level];

            Collider[] collider = transform.parent.GetComponentsInChildren<Collider>();
            foreach (Collider c in collider)
            {
                if (c.enabled)
                {
                    Physics.IgnoreCollision(c, projectile.collider, true);
                }
            }

            // Alter behavior if special projectile type
            if (projectile.homingProjectile)
            {
                projectile.targetPlayer = GameManager.Instance.FindClosestPlayer(owner, 10, true);
            }
            if (projectile.aimedProjectile)
            {
                projectile.targetPosition = new Vector3(-owner.mousePlayerDiff.x, owner.transform.position.y, -owner.mousePlayerDiff.y);
            }

            // Play effects
            if (audio.clip != null)
            {
                audio.PlayOneShot(audio.clip);
            }
            if (particleSystem != null)
            {
                StartCoroutine(ParticleBurstStaged());
            }

            lastProjectile = projectile;

            if (i < burstSize)
            {
                if (burstDelay > 0.0f)
                {
                    yield return new WaitForSeconds(burstDelay);
                }
            }
            else if (burstDelay == 0)
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }

    /// <summary>
    /// Sets level and returns level set at
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public abstract int SetLevel(int level);

}
