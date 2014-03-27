using UnityEngine;
using System.Collections;
using KBConstants;

[RequireComponent(typeof(Rigidbody))]

/// <summary>
/// Base class for instances of abilities in the world.
/// Contains timers & rigidbody/collider/layer setup
/// </summary>
abstract public class AbilityInstanceBaseScript : MonoBehaviour
{
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
    public float lifetime;
    protected float spawnTime;
    public int damage;
    public KBPlayer owner;

    public virtual void Start()
    {
        spawnTime = Time.time;
        rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        rigidbody.isKinematic = true;
        collider.isTrigger = true;
        damage = 0;
    }

    /// <summary>
    /// Call this after spawning an instance of an ability out of ObjectPool to reinitialize the instance.
    /// </summary>
    public virtual void Init(KBPlayer _owner)
    {
        spawnTime = Time.time;
        owner = _owner;
    }

    public virtual void Init()
    {
        Init(null);
    }

    protected virtual void Update()
    {
        if (Time.time - spawnTime > lifetime)
        {
            //DoOnHit(); // Uncomment this line to have projecticles "hit" on timeout
            ObjectPool.Recycle(this);
        }
    }

    public virtual void DoOnHit()
    {
        Reset();
    }

    public virtual void Reset()
    {
        ObjectPool.Recycle(this);
    }

    public abstract void OnTriggerEnter(Collider other);

}
