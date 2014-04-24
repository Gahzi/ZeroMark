using KBConstants;
using System.Collections.Generic;
using UnityEngine;

public abstract class KBGameObject : Photon.MonoBehaviour
{
    public Team team;
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
    public int health;
    //protected List<KBGameObject> collisionObjects;

    protected void Awake()
    {
        //collisionObjects = new List<KBGameObject>();
    }

    public virtual void Start()
    {
    }

    protected virtual void FixedUpdate()
    {
    }

    public virtual int TakeDamage(int amount) { return 0; }

    //protected virtual void OnTriggerEnter(Collider other)
    //{
    //    KBGameObject o = (KBGameObject)other.gameObject.GetComponentInChildren<KBGameObject>();
    //    if (o != null)
    //    {
    //        collisionObjects.Add(o);
    //    }
    //}

    //protected virtual void OnTriggerExit(Collider other)
    //{
    //    KBGameObject o = (KBGameObject)other.gameObject.GetComponentInChildren<KBGameObject>();
    //    if (o != null)
    //    {
    //        collisionObjects.Remove(o);
    //    }
    //}

    //public void hasDied(KBGameObject deadObj)
    //{
    //    collisionObjects.Remove(deadObj);
    //}

    //protected void KBDestroy()
    //{
    //    foreach (KBGameObject o in collisionObjects)
    //    {
    //        o.hasDied(this);
    //    }
    //}
}