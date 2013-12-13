using UnityEngine;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using KBConstants;

public class GameManager : MonoBehaviour
{

    enum GameState { PreGame, InGame, RedWins, BlueWins, Tie };

    List<Player> players;
    List<Factory> factories;
    List<Tower> towers;
    List<GoalZone> goalZones;
    List<Item> items;
    List<ItemZone> itemZones;

    //int redTeamScore = 0;
    //int blueTeamScore = 0;
    GameState state = GameState.PreGame;

    private GamepadInfoHandler gamepadHandler;
    private GameObject startImg;
    private GameObject redWinImg;
    private GameObject blueWinImg;
    private GameObject tieImg;

    private float startImgScreenTime = 3.0f;

    private float startTime;

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
        PhotonNetwork.offlineMode = true;
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
        //StartGame();
        //Application.LoadLevel(Application.loadedLevel);
    }

    #endregion

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        startTime = Time.time;

        state = GameState.PreGame;
        startImg = GameObject.Find("StartImage");
        startImg.SetActive(true);

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

        //redTeamScore = 0;
        //blueTeamScore = 0;

        GameObject[] loadedFactories = GameObject.FindGameObjectsWithTag("Factory");
        GameObject[] loadedTowers = GameObject.FindGameObjectsWithTag("Tower");
        GameObject[] loadedGoalZones = GameObject.FindGameObjectsWithTag("GoalZone");
        GameObject[] loadedItems = GameObject.FindGameObjectsWithTag("Item");
        ItemZone[] loadedItemZones = FindObjectsOfType<ItemZone>();

        factories = new List<Factory>(loadedFactories.Length);
        towers = new List<Tower>(loadedTowers.Length);
        goalZones = new List<GoalZone>(loadedTowers.Length);
        items = new List<Item>(loadedItems.Length);
        itemZones = new List<ItemZone>(loadedItemZones.Length);

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

        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case GameState.PreGame:
                foreach (GamepadInfo g in gamepadHandler.gamepads)
                {
                    if (g.buttonDown[0])
                    {
                        startImg.GetComponent<FadeOut>().startFading(1.0f);
                        state = GameState.InGame;
                    }
                }
                break;
            case GameState.InGame:
                CheckWinConditions();
                break;
            case GameState.RedWins:
                Debug.Log("Red won");
                break;
            case GameState.BlueWins:
                Debug.Log("Blue won");
                break;
            case GameState.Tie:
                break;
            default:
                break;
        }
    }

    void StartGame()
    {
        bool red = true;
        for (int i = 0; i < gamepadHandler.getNumberOfConnectedControllers(); i++)
        {
            GameObject newObj;
            newObj = createObject(ObjectConstants.type.Player, new Vector3(20, 2, 20), Quaternion.identity);
            Player p = newObj.GetComponent<Player>();

            if (red)
            {
                newObj.renderer.material = Resources.Load<Material>(MaterialConstants.MATERIAL_NAMES[MaterialConstants.type.CoreRed]);
                p.teamScript.Team = KBConstants.Team.Red;
            }
            else
            {
                newObj.renderer.material = Resources.Load<Material>(MaterialConstants.MATERIAL_NAMES[MaterialConstants.type.CoreBlue]);
                p.teamScript.Team = KBConstants.Team.Blue;
            }
            red = !red;
        }
    }

    /// <summary>
    /// Checks to see if the game has reached one of it's win conditions and changes state appropriately.
    /// </summary>
    void CheckWinConditions()
    {
        int redCaps = 0;
        int blueCaps = 0;
        foreach (GoalZone g in goalZones)
        {
            switch (g.state)
            {
                case GoalZone.GoalState.NotCaptured:
                    break;
                case GoalZone.GoalState.RedCaptured:
                    redCaps++;
                    break;
                case GoalZone.GoalState.BlueCaptured:
                    blueCaps++;
                    break;
                default:
                    break;
            }
        }
        if (redCaps >= 2)
        {
            state = GameState.RedWins;
        }
        else if (blueCaps >= 2)
        {
            state = GameState.BlueWins;
        }
    }

    void CheckGoalZones()
    {

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
                    //foreach (Factory f in newFactoryGroup.factory)
                    //{
                    //    factories.Add(f);
                    //}
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
        // TODO : Towers have different spawn properties based on towerSpawnInfo.itemType
        GameObject newTower = createObject(ObjectConstants.type.Tower, towerInfo.spawnLocation, towerInfo.spawnRotation);

        switch (towerInfo.team)
        {
            case Team.Red:
                newTower.renderer.material = Resources.Load<Material>(MaterialConstants.MATERIAL_NAMES[MaterialConstants.type.CoreRed]);
                break;
            case Team.Blue:
                newTower.renderer.material = Resources.Load<Material>(MaterialConstants.MATERIAL_NAMES[MaterialConstants.type.CoreBlue]);
                break;
            case Team.None:
                break;
            default:
                break;
        }

        Tower t = newTower.GetComponent<Tower>();
        t.teamScript.Team = towerInfo.team;
        t.TowerInfo = towerInfo;

    }
}
