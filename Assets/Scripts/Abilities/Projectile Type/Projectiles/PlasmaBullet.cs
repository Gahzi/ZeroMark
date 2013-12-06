using UnityEngine;
using System.Collections;

public class PlasmaBullet : ProjectileBaseScript
{

    #region CONSTANTS
    //public static float PLASMABULLET_LIFETIME = 0.0f;
    public static int PLASMABULLET_DAMAGE = 10;
    #endregion

    // Use this for initialization
    void Start()
    {
        base.Start();
        //lifetime = PLASMABULLET_LIFETIME;
        collideWithProjectiles = false;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tower"))
        {
            Debug.Log("Hit tower");
        }

        if (other.gameObject.CompareTag("Hitbox"))
        {
            Debug.Log("Hit hitbox");
            KBGameObject o = other.gameObject.transform.parent.GetComponent<KBGameObject>();
            o.takeDamage(PLASMABULLET_DAMAGE);
        }

    }
    

}
