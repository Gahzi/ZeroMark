using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]

abstract public class AbilityInstanceBaseScript : MonoBehaviour
{
    
    /// <summary>
    /// Base class for instances of abilities in the world.
    /// Contains timers & rigidbody/collider/layer setup
    /// </summary>
    
    public float lifetime;
    protected float spawnTime;
    public int damage;

    // Use this for initialization
    public virtual void Start()
    {
        spawnTime = Time.time;
        rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        rigidbody.isKinematic = true;
        collider.isTrigger = true;
        gameObject.layer = LayerMask.NameToLayer("Hitboxes1");
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (Time.time - spawnTime > lifetime)
        {
            Destroy(gameObject);
        }
    }

    public abstract void OnTriggerEnter(Collider other);
}
