using UnityEngine;
using System.Collections;

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

    [Range(1.0f, 100.0f)]
    public float projectileSpeed;
    protected Vector3 direction;
    public bool physicsProjectile;
    protected bool collideWithProjectiles;

    public virtual void Start()
    {
        base.Start();
        gameObject.tag = "Projectile";
        collideWithProjectiles = true;

        if (physicsProjectile)
        {
            rigidbody.isKinematic = false;
            direction.Normalize();
            direction.z = 0;
            rigidbody.AddForce(projectileSpeed * direction, ForceMode.VelocityChange);
        }
    }

    /// <summary>
    /// For 2D directions.
    /// </summary>
    /// <param name="dir"></param>
    //public virtual void SetDirection2D(Vector3 dir)
    //{
    //    direction = dir;
    //    direction.Normalize();
    //    direction.z = 0;
    //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //    transform.eulerAngles = new Vector3(0, 0, angle);
    //}

    public virtual void Update()
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
        if (other.gameObject.CompareTag("Hitbox"))
        {
            //SOMECLASS target = (SOMECLASS)other.transform.parent.GetComponent<SOMECLASS>();
            //target.TakeDamage(damage);
            //Destroy(gameObject);
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
