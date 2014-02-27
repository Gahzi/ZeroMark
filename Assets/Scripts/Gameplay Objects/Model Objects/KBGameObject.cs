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
    protected GameManager gm;
    protected List<KBGameObject> collisionObjects;

    private void Awake()
    {
        /*
        if (photonView.isMine)
        {
            this.enabled = true;
        }
         */
        collisionObjects = new List<KBGameObject>();
    }

    public virtual void Start()
    {
        gm = GameManager.Instance;
    }

    private void Update()
    {
    }

    public virtual int takeDamage(int amount) { return 0; }

    protected void OnTriggerEnter(Collider other)
    {
        KBGameObject o = (KBGameObject)other.gameObject.GetComponentInChildren<KBGameObject>();
        if (o != null)
        {
            collisionObjects.Add(o);
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        KBGameObject o = (KBGameObject)other.gameObject.GetComponentInChildren<KBGameObject>();
        if (o != null)
        {
            collisionObjects.Remove(o);
        }
    }

    public void hasDied(KBGameObject deadObj)
    {
        collisionObjects.Remove(deadObj);
    }

    protected void KBDestroy()
    {
        foreach (KBGameObject o in collisionObjects)
        {
            o.hasDied(this);
        }
    }
}