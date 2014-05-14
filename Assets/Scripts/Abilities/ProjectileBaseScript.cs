using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]

/// <summary>
/// Basic projectile class.
/// Defaults to Kinematic Rigidbody Trigger Collider.
/// Changes to Rigidbody Trigger Collider if flagged as physics projectile (e.g. arrow)
///
/// NOTES: Projectiles collide with colliders tagged "Hitbox"
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
    public bool collideWithEnvironment;
    public bool homingProjectile;
    public bool aimedProjectile;
    private List<GameObject> hitPlayer;

    public AreaOfEffectDamageScript explosionPrefab;

    public KBPlayer targetPlayer;
    public Vector3 targetPosition;
    private BoxCollider boxCollider;
    private Vector3 boxColliderSize;

    public override void Awake()
    {
        base.Awake();
        damageLevel = new int[3] { 0, 0, 0 };
    }

    public override void Start()
    {
        base.Start();
        //hitPlayer = new List<GameObject>();
        gameObject.tag = "Projectile";
        if (GetComponent<BoxCollider>() != null)
        {
            boxCollider = GetComponent<BoxCollider>();
            boxColliderSize = boxCollider.size;
        }

        if (explosionPrefab != null)
        {
            ObjectPool.CreatePool(explosionPrefab);
        }
    }

    protected override void FixedUpdate()
    {
        if (active)
        {
            base.FixedUpdate();
            if (boxCollider != null)
            {
                boxCollider.size = Vector3.Lerp(boxCollider.size, boxColliderSize, 5.0f * Time.deltaTime);
            }
            if (aimedProjectile && homingProjectile)
            {
                Debug.LogWarning("Projectile cannot be both aimed & homing");
            }
            if (homingProjectile && targetPlayer != null)
            {
                DoHomingBehavior();
            }
            if (aimedProjectile)
            {
                DoAimedBehavior();
            }
            transform.Translate(Vector3.forward * (projectileSpeed + inheritSpeed) * Time.deltaTime);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetPosition, 1.0f);

        if (targetPlayer != null)
        {
            Gizmos.DrawWireSphere(targetPlayer.transform.position, 1.0f);
        }
    }

    public override void Init(KBPlayer _owner)
    {
        base.Init(_owner);
        if (hitPlayer == null)
        {
            hitPlayer = new List<GameObject>();
        }
        else
        {
            hitPlayer.Clear();
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        // Bullet will collide with anything that is a projectile, environment, or hitbox tagged.
        // If it hits a hitbox, it needs to inform the owner of the bullet that it has hit a player.
        if (active)
        {
            if (other.gameObject.CompareTag("Hitbox"))
            {
                KBGameObject o = other.gameObject.transform.parent.transform.parent.GetComponent<KBGameObject>();
                KBPlayer victimPlayer = other.gameObject.transform.parent.transform.parent.GetComponent<KBPlayer>();

                if (victimPlayer != null && !hitPlayer.Contains(victimPlayer.gameObject))
                {
                    if (victimPlayer.Team != Team && victimPlayer.health > 0 && victimPlayer.invulnerabilityTime <= 0)
                    {
                        if (victimPlayer.photonView.isMine && owner != null)
                        {
                            int victimHealth = o.TakeDamage(damage);
                            hitPlayer.Add(victimPlayer.gameObject);
                            if (victimHealth <= 0)
                            {
                                o.gameObject.GetComponent<KBPlayer>().Die(owner.gameObject);
                            }
                            owner.ConfirmHit(o.gameObject.GetComponent<KBPlayer>(), damage);
                            DoOnHit();
                        }
                        else if (owner != null)
                        {
                            //owner.ConfirmHitToOthers(victimPlayer.networkPlayer, damage);
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
        }

    }

    public override void DoOnHit()
    {
        if (explosionPrefab != null)
        {
            AreaOfEffectDamageScript a = ObjectPool.Spawn(explosionPrefab, transform.position);
            a.Init(owner);
        }
        if (boxCollider != null)
        {
            boxCollider.size = Vector3.one;
        }
        base.DoOnHit();
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