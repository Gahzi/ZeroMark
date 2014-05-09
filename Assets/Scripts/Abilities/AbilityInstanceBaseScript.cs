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
    private TrailRenderer trailRenderer;
    private float originalTrailTime;
    public bool canHitMultiplePlayers;

    public virtual void Awake()
    {

    }

    public virtual void Start()
    {
        spawnTime = Time.time;
        rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        rigidbody.isKinematic = true;
        collider.isTrigger = true;
        //damage = 0;
        //lifetime = 10;
        trailRenderer = GetComponent<TrailRenderer>();
        if (trailRenderer != null)
        {
            originalTrailTime = trailRenderer.time;
        }
    }

    /// <summary>
    /// Call this after spawning an instance of an ability out of ObjectPool to reinitialize the instance.
    /// </summary>
    public virtual void Init(KBPlayer _owner)
    {
        spawnTime = Time.time;
        if (trailRenderer != null)
        {
            Invoke("ResetTrail", 0.01f);
        }
        team = _owner.team;
        owner = _owner;
    }

    //public virtual void Init()
    //{
    //    Init(null);
    //}

    protected virtual void FixedUpdate()
    {
        if (Time.time - spawnTime > lifetime)
        {
            //DoOnHit(); // Uncomment this line to have projecticles "hit" on timeout
            Reset();
        }
    }

    public virtual void DoOnHit()
    {
        Reset();
    }

    public void ResetTrail()
    {
        trailRenderer.time = originalTrailTime;
    }

    public virtual void Reset()
    {
        if (trailRenderer != null)
        {
            trailRenderer.time = -1;
        }
        if (canHitMultiplePlayers && Time.time - spawnTime > lifetime)
        {
            ObjectPool.Recycle(this);

        }
        else
        {
            ObjectPool.Recycle(this);
        }
        if (gameObject.particleSystem != null)
        {
            gameObject.particleSystem.Clear();
        }
    }

    public abstract void OnTriggerEnter(Collider other);

}
