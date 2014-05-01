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

    protected void Awake()
    {
    }

    public virtual void Start()
    {
    }

    protected virtual void FixedUpdate()
    {
    }

    public virtual int TakeDamage(int amount) { return 0; }
}