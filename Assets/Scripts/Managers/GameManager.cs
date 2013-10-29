using UnityEngine;
using System.Collections.Generic;
using KBConstants;

public class GameManager : MonoBehaviour {
	
	List<Player> players;
	
	private static GameManager instance;
	
	public static GameManager Instance
    {
        // Here we use the ?? operator, to return 'instance' if 'instance' does not equal null
        // otherwise we assign instance to a new component and return that
        get { return instance ?? (instance = new GameObject("Game Manager").AddComponent<GameManager>()); }
    }
	
	// Use this for initialization
	void Start () 
	{
		players = new List<Player>(1);
		
		instance = this;
		
		GameObject player = (GameObject)Instantiate(Resources.Load("Player"));
		Player playerScript = player.GetComponent<Player>();
		
		if(playerScript != null) 
		{
			players.Add(playerScript);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void createObject(KBConstants.ObjectConstants.type objectType)
	{
		switch (objectType) 
		{
			case ObjectConstants.type.Player:
			{
				Player newPlayer = (Player)Instantiate(Resources.Load(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.Player],typeof(Player)));
                players.Add(newPlayer);
				break;
			}
			
			default:
			break;
		}
	}
}
