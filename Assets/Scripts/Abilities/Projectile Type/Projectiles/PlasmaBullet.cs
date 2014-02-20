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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hitbox"))
        {
            KBGameObject o = other.gameObject.transform.parent.GetComponent<KBGameObject>();
            if (o.Team != Team)
            {
                o.takeDamage(PLASMABULLET_DAMAGE);
                Destroy(gameObject);
            }
        }
        if (other.gameObject.CompareTag("Environment"))
        {
            Destroy(gameObject);
        }
    }

}