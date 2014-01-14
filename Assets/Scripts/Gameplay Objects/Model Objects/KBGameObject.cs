using KBConstants;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Team))]
public class KBGameObject : Photon.MonoBehaviour
{
    public int health;
    public TeamScript teamScript;
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

        if (!GetComponentInChildren<TeamScript>())
        {
            gameObject.AddComponent<TeamScript>();
        }

        teamScript = GetComponentInChildren<TeamScript>();
    }

    private void Update()
    {
    }

    public virtual void takeDamage(int amount)
    {
    }

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