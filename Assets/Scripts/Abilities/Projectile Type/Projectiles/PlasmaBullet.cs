using UnityEngine;

public class PlasmaBullet : ProjectileBaseScript
{
    #region CONSTANTS

    public static int PLASMABULLET_DAMAGE = 1;

    #endregion CONSTANTS

    private void Start()
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
            o.takeDamage(PLASMABULLET_DAMAGE);
        }
    }
}