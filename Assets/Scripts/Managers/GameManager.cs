using UnityEngine;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using KBConstants;

public class GameManager : MonoBehaviour {

    enum GameState { PreGame, InGame, RedWins, BlueWins, Tie };


	List<Player> players;
    List<Factory> factories;
    List<Tower> towers;
    List<GoalZone> goalZones;
    List<Item> items;

    

    int redTeamScore = 0;
    int blueTeamScore = 0;
    GameState state = GameState.PreGame;


    private GamepadInfoHandler gamepadHandler;

	private static GameManager instance;
	public static GameManager Instance
    {
        // Here we use the ?? operator, to return 'instance' if 'instance' does not equal null
        // otherwise we assign instance to a new component and return that
        get 
        {
            if (instance == null)
            {
                GameObject gamepadManagerObject = (GameObject.FindGameObjectWithTag("GameManager"));
                if (gamepadManagerObject != null)
                {
                    instance = gamepadManagerObject.GetComponent<GameManager>();
                    if (instance != null)
                    {
                        return instance;
                    }
                    else
                    {
                        instance = new GameObject("Game Manager").AddComponent<GameManager>();
                        return instance;
                    }
                }
            }

            return instance;
        
        }
    }

    #region PHOTON CONNECTION HANDLING

    // This is one of the callback/event methods called by PUN (read more in PhotonNetworkingMessage enumeration)
    public void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server");
        PhotonNetwork.JoinRandomRoom();
    }

    // This is one of the callback/event methods called by PUN (read more in PhotonNetworkingMessage enumeration)
    public void OnPhotonRandomJoinFailed()
    {
        Debug.Log("Room Creation Failed");
        PhotonNetwork.CreateRoom(null, true, true, 4);
    }

    // This is one of the callback/event methods called by PUN (read more in PhotonNetworkingMessage enumeration)
    public void OnJoinedRoom()
    {
        Debug.Log("Joined Room Succesfully");
        createObject(ObjectConstants.type.Player);
       
    }

    // This is one of the callback/event methods called by PUN (read more in PhotonNetworkingMessage enumeration)
    public void OnCreatedRoom()
    {
        Debug.Log("Created Room Succesfully");
     
       //Application.LoadLevel(Application.loadedLevel);
    }

    #endregion

    void Awake()
    {
        instance = this;
    }
	
	// Use this for initialization
	void Start () 
	{
        state = GameState.PreGame;

        gamepadHandler = GamepadInfoHandler.Instance;

		players = new List<Player>(gamepadHandler.getNumberOfConnectedControllers());

        if (gamepadHandler.getNumberOfConnectedControllers() > 0)
        {
            if (!PhotonNetwork.connected)
            {
                PhotonNetwork.autoJoinLobby = false;
                PhotonNetwork.ConnectUsingSettings("1");
            }
        }

        redTeamScore = 0;
        blueTeamScore= 0;

        GameObject[] loadedFactories = GameObject.FindGameObjectsWithTag("Factory");
        GameObject[] loadedTowers = GameObject.FindGameObjectsWithTag("Tower");
        GameObject[] loadedGoalZones = GameObject.FindGameObjectsWithTag("GoalZone");
        GameObject[] loadedItems = GameObject.FindGameObjectsWithTag("Item");


        List<Factory> factories = new List<Factory>(loadedFactories.Length);
        List<Tower> towers = new List<Tower>(loadedTowers.Length);
        List<GoalZone> goalZones = new List<GoalZone>(loadedTowers.Length);
        List<Item> items = new List<Item>(loadedItems.Length);


        for (int i = 0; i < loadedFactories.Length; i++)
        {
            factories.Add(loadedFactories[i].GetComponent<Factory>());
        }

        for (int i = 0; i < loadedTowers.Length; i++)
        {
            towers.Add(loadedTowers[i].GetComponent<Tower>());
        }

        for (int i = 0; i < loadedGoalZones.Length; i++)
        {
            goalZones.Add(loadedGoalZones[i].GetComponent<GoalZone>());
        }

        for (int i = 0; i < loadedItems.Length; i++)
        {
            items.Add(loadedItems[i].GetComponent<Item>());
        }
	}

    // Update is called once per frame
    void Update() 
	{
        CheckWinConditions();
	}

    void StartGame()
    {
        // Spawn 
        /*
                     * Factories,
            Goal Zone,
            Items,
            Tower
         * */


    }

    /// <summary>
    /// Checks to see if the game has reached one of it's win conditions and changes state appropriately.
    /// </summary>
    void CheckWinConditions()
    {
        if (redTeamScore >= 50 && blueTeamScore >= 50)
        {
            //Game State is Tie
            state = GameState.Tie;
        }
        else if(redTeamScore >= 50)
        {
            //Game state is Red Wins
            state = GameState.RedWins;
        }
        else if (blueTeamScore >= 50)
        {
            //Game state is Blue Wins
            state = GameState.BlueWins;
        }
    }
	
	void createObject(KBConstants.ObjectConstants.type objectType)
	{
		switch (objectType) 
		{
			case ObjectConstants.type.Player:
			{
                GameObject newPlayerObject = PhotonNetwork.Instantiate(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.Player], new Vector3(0, 2, 0), Quaternion.identity, 0);
                Player newPlayer = newPlayerObject.GetComponent<Player>();
                players.Add(newPlayer);
				break;
			}

            case ObjectConstants.type.Item:
            {
                break;
            }
			
			default:
			break;
		}
	}
}
