using UnityEngine;

public class PlasmaBullet : ProjectileBaseScript
{
    #region CONSTANTS

    public static int PLASMABULLET_DAMAGE = 1;

    #endregion CONSTANTS

    public override void Start()
    {
        base.Start();
        collideWithProjectiles = false;
    }

    private void Update()
    {
        base.Update();
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    base.OnTriggerEnter();

    //    //// Bullet will collide with anything that is a projectile, environment, or hitbox tagged.
    //    //// If it hits a hitbox, it needs to inform the owner of the bullet that it has hit a player.
    //    //if (other.gameObject.CompareTag("Hitbox"))
    //    //{
    //    //    KBGameObject o = other.gameObject.transform.parent.GetComponent<KBGameObject>();
    //    //    if(o.gameObject.GetComponent<PlayerLocal>().networkPlayer.isLocal)
    //    //    {
    //    //        if (o.Team != Team)
    //    //        {
    //    //            int victimHealth = o.takeDamage(PLASMABULLET_DAMAGE);
    //    //            if (victimHealth <= 0)
    //    //            {
    //    //                o.gameObject.GetComponent<PlayerLocal>().Die(owner.gameObject);
    //    //            }
    //    //            owner.ConfirmHit();
    //    //            Destroy(gameObject);
    //    //        }
    //    //    }
    //    //}
    //    //if (other.gameObject.CompareTag("Environment"))
    //    //{
    //    //    Destroy(gameObject);
    //    //}
    //}

}