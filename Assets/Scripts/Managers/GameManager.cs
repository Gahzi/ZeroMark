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
    List<ItemZone> itemZones;

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
    }

    // This is one of the callback/event methods called by PUN (read more in PhotonNetworkingMessage enumeration)
    public void OnCreatedRoom()
    {
        Debug.Log("Created Room Succesfully");
        StartGame();
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

        GameObject[] loadedFactories        = GameObject.FindGameObjectsWithTag("Factory");
        GameObject[] loadedTowers           = GameObject.FindGameObjectsWithTag("Tower");
        GameObject[] loadedGoalZones        = GameObject.FindGameObjectsWithTag("GoalZone");
        GameObject[] loadedItems            = GameObject.FindGameObjectsWithTag("Item");
        //GameObject[] loadedItemZones      = GameObject.FindGameObjectsWithTag("ItemZone");
        ItemZone[] loadedItemZones          = FindObjectsOfType<ItemZone>();

        factories                           = new List<Factory>(loadedFactories.Length);
        towers                              = new List<Tower>(loadedTowers.Length);
        goalZones                           = new List<GoalZone>(loadedTowers.Length);
        items                               = new List<Item>(loadedItems.Length);
        itemZones                           = new List<ItemZone>(loadedItemZones.Length);

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

        foreach (ItemZone z in loadedItemZones)
        {
            itemZones.Add(z);
        }
	}

    // Update is called once per frame
    void Update() 
	{
        CheckWinConditions();
	}

    void StartGame()
    {
        GameObject newObj;
        
        newObj = createObject(ObjectConstants.type.Player, new Vector3(20, 2, 20), Quaternion.identity);
        newObj = createObject(ObjectConstants.type.ItemZone, new Vector3(200, 0, 0), Quaternion.identity);
        newObj = createObject(ObjectConstants.type.ItemZone, new Vector3(-200, 0, 0), Quaternion.identity);

        for (int i = 0; i < 4; i++)
        {
            Quaternion q = Quaternion.identity;
            float x = 0.0f;
            float z = 0.0f;
            Team t = Team.None;
            switch (i)
            {
                case 0:
                    x = 200.0f;
                    z = 100.0f;
                    t = Team.Red;
                    q = Quaternion.AngleAxis(180.0f, Vector3.up);
                    break;
                case 1:
                    x = 200.0f;
                    z = -100.0f;
                    t = Team.Red;
                    break;

                case 2:
                    x = -200.0f;
                    z = 100.0f;
                    t = Team.Blue;
                    q = Quaternion.AngleAxis(180.0f, Vector3.up);
                    break;
                case 3:
                    x = -200.0f;
                    z = -100.0f;
                    t = Team.Blue;
                    break;
                default:
                    Debug.LogError("Factory generation error");
                    break;
            }
            GameObject g = createObject(ObjectConstants.type.FactoryGroup, new Vector3(x, 0, z), q);
            FactoryGroup f = g.GetComponent<FactoryGroup>();
            f.resetFactories();
            f.setTeam(t);
        }
        
        foreach (ItemZone z in itemZones)
        {
            z.GenerateItems();
        }
        
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
	
	public GameObject createObject(KBConstants.ObjectConstants.type objectType, Vector3 position, Quaternion rotation)
	{
		switch (objectType) 
		{
			case ObjectConstants.type.Player:
			{
                GameObject newPlayerObject = PhotonNetwork.Instantiate(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.Player], position, rotation, 0);
                Player newPlayer = newPlayerObject.GetComponent<Player>();
                players.Add(newPlayer);
				return newPlayerObject;
			}

            case ObjectConstants.type.Item:
            {
                GameObject newItemObject = PhotonNetwork.Instantiate(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.Item], position, rotation, 0);
                Item newItem = newItemObject.GetComponent<Item>();
                items.Add(newItem);
                return newItemObject;
            }

            case ObjectConstants.type.Tower:
            {
                GameObject newTowerObject = PhotonNetwork.Instantiate(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.Tower], position, rotation, 0);
                Tower newTower = newTowerObject.GetComponent<Tower>();
                towers.Add(newTower);
                return newTowerObject;
            }

            case ObjectConstants.type.FactoryGroup:
            {
                GameObject newFactoryGroupObject = PhotonNetwork.Instantiate(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.FactoryGroup], position, rotation, 0);
                FactoryGroup newFactoryGroup = newFactoryGroupObject.GetComponent<FactoryGroup>();
                foreach (Factory f in newFactoryGroup.factory)
                {
                    factories.Add(f);
                }
                return newFactoryGroupObject;
            }

            case ObjectConstants.type.ItemZone:
            {
                GameObject newItemZoneObject = PhotonNetwork.Instantiate(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.ItemZone], position, rotation, 0);
                ItemZone newItemZone = newItemZoneObject.GetComponent<ItemZone>();
                itemZones.Add(newItemZone);
                return newItemZoneObject;
            }
			
			default:
			return null;
		}
	}

    public void SpawnTower(TowerInfo towerInfo)
    {
        Vector3 spawnLocation = new Vector3(0, 50, 0);
        switch (towerInfo.team)
        {
            case Team.Red:
                spawnLocation = new Vector3(275.0f, 0, 0);
                break;
            case Team.Blue:
                spawnLocation = new Vector3(-275.0f, 0, 0);
                break;
            case Team.None:
                break;
            default:
                break;
        }
        // TODO : Towers have different spawn properties based on towerSpawnInfo.itemType
        GameObject newTower = createObject(ObjectConstants.type.Tower, spawnLocation, Quaternion.identity);
        Tower t = newTower.GetComponent<Tower>();
        t.Team = towerInfo.team;
        t.TowerInfo = towerInfo;

    }
}
