using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Collections;
using KBConstants;



public class KBGameObject : Photon.MonoBehaviour {

    public int health;
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

	protected GameManager gm;

    void Awake()
    {
        /*
        if (photonView.isMine)
        {
            this.enabled = true;
        }
         */
    }

	// Use this for initialization
	void Start() 
	{
		gm = GameManager.Instance;
        
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

    public virtual void takeDamage(int amount) { }
    

}
