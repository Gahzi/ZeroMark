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

    protected Vector3 direction;
    public bool physicsProjectile;
    public bool collideWithProjectiles;
    public bool collideWithEnvironment;

    public override void Start()
    {
        base.Start();
        gameObject.tag = "Projectile";
        //collideWithProjectiles = true;

        if (physicsProjectile)
        {
            rigidbody.isKinematic = false;
            direction.Normalize();
            direction.z = 0;
            rigidbody.AddForce(projectileSpeed * direction, ForceMode.VelocityChange);
        }
        damage = DAMAGE; ;
    }

    protected override void Update()
    {
        base.Update();

        if (physicsProjectile)
        {
            // Physics movement occurs @ init
        }
        else
        {
            transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        // Bullet will collide with anything that is a projectile, environment, or hitbox tagged.
        // If it hits a hitbox, it needs to inform the owner of the bullet that it has hit a player.
        if (other.gameObject.CompareTag("Hitbox"))
        {
            KBGameObject o = other.gameObject.transform.parent.GetComponent<KBGameObject>();
            if (o.gameObject.GetComponent<KBPlayer>().photonView.isMine)
            {
                if (o.Team != Team)
                {
                    int victimHealth = o.TakeDamage(DAMAGE);
                    if (victimHealth <= 0)
                    {
                        o.gameObject.GetComponent<KBPlayer>().Die(owner.gameObject);
                    }
                    owner.ConfirmHit(o.gameObject.GetComponent<KBPlayer>());
                    Destroy(gameObject);
                }
            }
        }
        if (collideWithEnvironment)
        {
            if (other.gameObject.CompareTag("Environment"))
            {
                Destroy(gameObject);
            }
        }
        
        if (collideWithProjectiles)
        {
            if (other.gameObject.CompareTag("Projectile"))
            {
                Destroy(gameObject);
            }
        }
    }

    public void setLifetimeForMaxRange(int maxRange)
    {
        lifetime = maxRange / projectileSpeed;
    }
}