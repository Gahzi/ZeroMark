using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

/// <summary>
/// Basic projectile class.
/// Defaults to Kinematic Rigidbody Trigger Collider.
/// Changes to Rigidbody Trigger Collider if flagged as physics projectile (e.g. arrow)
///
/// NOTES: Projectiles collide with colliders tagged "Hitbox", but only affect
/// objects with CompetitorModules (any child of CompetitivePlayerBaseScript)
/// </summary>
abstract public class ProjectileBaseScript : AbilityInstanceBaseScript
{
    #region CONSTANTS

    public int[] damageLevel;

    #endregion CONSTANTS

    [Range(1.0f, 500.0f)]
    public float projectileSpeed;
    public float inheritSpeed;
    protected Vector3 direction;
    public bool collideWithProjectiles;
    public bool collideWithEnvironment;
    public bool homingProjectile;
    public bool aimedProjectile;

    public AreaOfEffectDamageScript explosionPrefab;

    public KBPlayer targetPlayer;
    public Vector3 targetPosition;

    public override void Awake()
    {
        base.Awake();
        damageLevel = new int[3] { 0, 0, 0 };
    }

    public override void Start()
    {
        base.Start();
        gameObject.tag = "Projectile";
        
        if (explosionPrefab != null)
        {
            ObjectPool.CreatePool(explosionPrefab);
        }
    }

    protected override void Update()
    {
        base.Update();
        if (aimedProjectile && homingProjectile)
        {
            Debug.LogWarning("Projectile cannot be both aimed & homing");
        }
        if (homingProjectile && targetPlayer != null)
        {
            DoHomingBehavior();
        }
        if (aimedProjectile && targetPosition != null)
        {
            DoAimedBehavior();
        }
        transform.Translate(Vector3.forward * (projectileSpeed + inheritSpeed) * Time.deltaTime);
    }

    void OnDrawGizmos()
    {
        if (targetPosition != null)
        {
            Gizmos.DrawWireSphere(targetPosition, 1.0f);
        }
        if (targetPlayer != null)
        {
            Gizmos.DrawWireSphere(targetPlayer.transform.position, 1.0f);
        }
    }

    public override void Init()
    {
        base.Init();
    }

    public override void OnTriggerEnter(Collider other)
    {
        // Bullet will collide with anything that is a projectile, environment, or hitbox tagged.
        // If it hits a hitbox, it needs to inform the owner of the bullet that it has hit a player.
        if (other.gameObject.CompareTag("Hitbox"))
        {
            KBGameObject o = other.gameObject.transform.parent.transform.parent.GetComponent<KBGameObject>();
            KBPlayer victimPlayer = other.gameObject.transform.parent.transform.parent.GetComponent<KBPlayer>();

            if (victimPlayer != null)
            {
                if (victimPlayer.Team != Team && victimPlayer.health > 0 && victimPlayer.invulnerabilityTime <= 0)
                {

                    if (victimPlayer.photonView.isMine && owner != null)
                    {
                        int victimHealth = o.TakeDamage(damage);
                        if (victimHealth <= 0)
                        {
                            o.gameObject.GetComponent<KBPlayer>().Die(owner.gameObject);
                        }
                        owner.ConfirmHit(o.gameObject.GetComponent<KBPlayer>(), damage);
                        DoOnHit();
                    }
                    else if (owner != null)
                    {
                        owner.ConfirmHitToOthers(victimPlayer.networkPlayer, damage);
                        DoOnHit();
                    }
                }
            }
        }
        if (collideWithEnvironment)
        {
            if (other.gameObject.CompareTag("Environment"))
            {
                DoOnHit();
            }
        }

        if (collideWithProjectiles)
        {
            if (other.gameObject.CompareTag("Projectile"))
            {
                DoOnHit();
            }
        }
    }

    public void setLifetimeForMaxRange(int maxRange)
    {
        lifetime = maxRange / projectileSpeed;
    }

    protected virtual void DoHomingBehavior()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPlayer.transform.position - owner.transform.position), 5.0f * Time.deltaTime);
    }

    protected virtual void DoAimedBehavior()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPosition - owner.transform.position), 5.0f * Time.deltaTime);
    }
}