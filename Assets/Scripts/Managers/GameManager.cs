using KBConstants;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private enum GameState { PreGame, InGame, RedWins, BlueWins, Tie, EndGame };

    private List<PlayerLocal> players;
    private List<CaptureZone> captureZones;
    private List<Item> items;
    private List<PlayerSpawnZone> playerSpawnZones;
    private CaptureZone victoryZone;
    private GameState state = GameState.PreGame;

    //private GamepadInfoHandler gamepadHandler;
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

    #endregion PHOTON CONNECTION HANDLING

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        startTime = Time.time;

        state = GameState.PreGame;
        //startImg = GameObject.Find("StartImage");
        //startImg.SetActive(true);

        // TODO : Code below needs to be refactored to use KBaM with conditional controller support

        //gamepadHandler = GamepadInfoHandler.Instance;

        //players = new List<Player>(gamepadHandler.getNumberOfConnectedControllers());

        //if (gamepadHandler.getNumberOfConnectedControllers() > 0)
        //{
        //    if (!PhotonNetwork.connected)
        //    {
        //        PhotonNetwork.autoJoinLobby = false;
        //        PhotonNetwork.ConnectUsingSettings("1");
        //    }
        //}


        // TODO : Some way of making the list of players
        CaptureZone[] loadedCaptureZones = FindObjectsOfType<CaptureZone>();
        GameObject[] loadedItems = GameObject.FindGameObjectsWithTag("Item");
        ItemSpawn[] loadedItemZones = FindObjectsOfType<ItemSpawn>();
        PlayerSpawnZone[] loadedPSpawns = FindObjectsOfType<PlayerSpawnZone>();

        captureZones = new List<CaptureZone>(loadedCaptureZones.Length);
        items = new List<Item>(loadedItems.Length);
        playerSpawnZones = new List<PlayerSpawnZone>(loadedPSpawns.Length);

        foreach (CaptureZone c in loadedCaptureZones)
        {
            captureZones.Add(c);
            if (c.tier == CaptureZone.ZoneTier.C)
            {
                victoryZone = c;
            }
        }

        foreach (PlayerSpawnZone p in loadedPSpawns)
        {
            playerSpawnZones.Add(p);
        }

        StartGame();
    }

    private void Update()
    {
        switch (state)
        {
            case GameState.PreGame:

                break;

            case GameState.InGame:
                CheckWinConditions();
                break;

            case GameState.RedWins:
                Debug.Log("Red won");
                state = GameState.EndGame;
                break;

            case GameState.BlueWins:
                Debug.Log("Blue won");
                state = GameState.EndGame;
                break;

            case GameState.Tie:
                Debug.Log("Tie");
                state = GameState.EndGame;
                break;

            case GameState.EndGame:

                break;

            default:
                break;
        }
    }

    private void StartGame()
    {
        state = GameState.InGame;
    }

    /// <summary>
    /// Checks to see if the game has reached one of it's win conditions and changes state appropriately.
    /// </summary>
    private void CheckWinConditions()
    {
        if (victoryZone.state != CaptureZone.ZoneState.Unoccupied)
        {
            switch (victoryZone.state)
            {
                case CaptureZone.ZoneState.Unoccupied:
                    break;
                case CaptureZone.ZoneState.Red:
                    state = GameState.RedWins;
                    break;
                case CaptureZone.ZoneState.Blue:
                    state = GameState.BlueWins;
                    break;
                default:
                    break;
            }
        }
    }

    public GameObject createObject(KBConstants.ObjectConstants.type objectType, Vector3 position, Quaternion rotation)
    {
        switch (objectType)
        {
            case ObjectConstants.type.Player:
                {
                    GameObject newPlayerObject = PhotonNetwork.Instantiate(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.Player], position, rotation, 0);
                    PlayerLocal newPlayer = newPlayerObject.GetComponent<PlayerLocal>();
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

            default:
                return null;
        }
    }

    private void SendGlobalMessageToPlayers(string msg)
    {
        // TODO
    }

    public static string GetCaptureZoneStateString()
    {
        string tab = "     ";
        string spacer = " : ";
        GameManager gm = GameManager.instance;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        //sb.Append(gm.victoryZone.tier.ToString() + spacer + gm.victoryZone.state.ToString() + System.Environment.NewLine);
        sb.Append(gm.victoryZone.descriptiveName + spacer + gm.victoryZone.scoreboard.txt + System.Environment.NewLine);
        foreach (CaptureZone c in gm.victoryZone.requiredZones)
        {
            sb.Append(tab + c.descriptiveName + spacer + c.scoreboard.txt + System.Environment.NewLine);

            foreach (CaptureZone d in c.requiredZones)
            {
                sb.Append(tab + tab + d.descriptiveName + spacer + d.scoreboard.txt + System.Environment.NewLine);
            }
        }
        return sb.ToString();
    }
}