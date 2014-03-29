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

    private static int DAMAGE = 0;

    #endregion CONSTANTS

    [Range(1.0f, 500.0f)]
    public float projectileSpeed;
    public float inheritSpeed;
    protected Vector3 direction;
    public bool collideWithProjectiles;
    public bool collideWithEnvironment;

    public AreaOfEffectDamageScript explosionPrefab;

    public override void Start()
    {
        base.Start();
        gameObject.tag = "Projectile";
        damage = DAMAGE;
        if (explosionPrefab != null)
        {
            ObjectPool.CreatePool(explosionPrefab);
        }
    }

    protected override void Update()
    {
        base.Update();
        transform.Translate(Vector3.forward * (projectileSpeed + inheritSpeed) * Time.deltaTime);
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

                    if (victimPlayer.photonView.isMine)
                    {
                        int victimHealth = o.TakeDamage(damage);
                        if (victimHealth <= 0)
                        {
                            o.gameObject.GetComponent<KBPlayer>().Die(owner.gameObject);
                        }
                        owner.ConfirmHit(o.gameObject.GetComponent<KBPlayer>());
                        DoOnHit();
                    }
                    else
                    {
                        owner.ConfirmHitToOthers(victimPlayer.networkPlayer);
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
}