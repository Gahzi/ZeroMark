using UnityEngine;
using KBConstants;

[RequireComponent(typeof(Rigidbody))]

public class HitboxBaseScript : MonoBehaviour
{

    public GameObject owner;
    
    private Team team;
    public Team Team
    {
        get
        {
            return team;
        }
        set
        {
            team = value;
        }
    }

    void Start()
    {
        rigidbody.isKinematic = true;
        collider.isTrigger = true;
        gameObject.tag = "Hitbox";
        //gameObject.layer = LayerMask.NameToLayer("Hitboxes1");
    }

    void Update()
    {

    }
}
