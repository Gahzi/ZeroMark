using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Collections;
using KBConstants;

[RequireComponent(typeof(Team))]

public class KBGameObject : Photon.MonoBehaviour {

    public int health;
    public Vector3 targetPosition;
    public TeamScript teamScript;

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

        if (!GetComponentInChildren<TeamScript>())
        {
            gameObject.AddComponent<TeamScript>();
        }

        teamScript = GetComponentInChildren<TeamScript>();

	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

    public virtual void takeDamage(int amount) { }
    

}
