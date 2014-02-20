using KBConstants;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : Photon.MonoBehaviour
{
    public enum GameState { PreGame, InGame, RedWins, BlueWins, Tie, EndGame };

    public PlayerLocal localPlayer;
    public List<PlayerLocal> players;

    public List<CaptureZone> captureZones;
    private List<Item> items;
    private List<PlayerSpawnPoint> playerSpawnZones;
    public CaptureZone victoryZone;
    private GameState state = GameState.PreGame;
    public GameState State
    {
        get
        {
            return state;
        }
    }
    public int redTeamScore;
    public int blueTeamScore;
    public float lastTick;
    public int secondsPerTick;
    public int ticksPerGame;
    public int tick = 0;
    public int redCaptures = 0;
    public int blueCaptures = 0;
    public int redBonus = 0;
    public int blueBonus = 0;
    private List<List<String>> playerStatData;
    private List<List<String>> upgradePointReqData;
    private float startTime;

    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
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
        //photonView.RPC("AddPlayer", PhotonTargets.AllBuffered);
        localPlayer = createObject(ObjectConstants.type.Player, new Vector3(0, 0, 0), Quaternion.identity, Team.Blue).GetComponent<PlayerLocal>(); ;
        photonView.RPC("SetPlayerLevel", PhotonTargets.AllBuffered, PhotonNetwork.player, 1);
    }

    // This is one of the callback/event methods called by PUN (read more in PhotonNetworkingMessage enumeration)
    public void OnCreatedRoom()
    {
        Debug.Log("Created Room Succesfully");

    }

    void OnPhotonPlayerDisconneced(PhotonPlayer player)
    {
        foreach (PlayerLocal currentPlayer in players)
        {
            if (currentPlayer.networkPlayer == player)
            {
                players.Remove(currentPlayer);
            }
        }
    }

    void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        //SHERVIN: SEND A MESSAGE SAYING A NEW PLAYER HAS JOINED
    }

    #endregion PHOTON CONNECTION HANDLING

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ReadPlayerStatData();
        ReadUpgradePointData();
        startTime = Time.time;
        lastTick = Time.time;
        state = GameState.PreGame;

        if (!PhotonNetwork.connected)
        {
            PhotonNetwork.autoJoinLobby = false;
            PhotonNetwork.ConnectUsingSettings("1");
        }

        CaptureZone[] loadedCaptureZones = FindObjectsOfType<CaptureZone>();
        GameObject[] loadedItems = GameObject.FindGameObjectsWithTag("Item");
        ItemSpawn[] loadedItemZones = FindObjectsOfType<ItemSpawn>();
        PlayerSpawnPoint[] loadedPSpawns = FindObjectsOfType<PlayerSpawnPoint>();

        //TODO:Remove 6 value and replace with constant representing max player size;
        players = new List<PlayerLocal>(6);
        captureZones = new List<CaptureZone>(loadedCaptureZones.Length);
        items = new List<Item>(loadedItems.Length);
        playerSpawnZones = new List<PlayerSpawnPoint>(loadedPSpawns.Length);

        foreach (CaptureZone c in loadedCaptureZones)
        {
            captureZones.Add(c);
            if (c.tier == CaptureZone.ZoneTier.C)
            {
                victoryZone = c;
            }
        }

        foreach (PlayerSpawnPoint p in loadedPSpawns)
        {
            playerSpawnZones.Add(p);
        }
    }

    private void Update()
    {
        if (Time.time > lastTick + secondsPerTick)
        {
            NextTick();
        }

        if (localPlayer != null)
        {
            switch (state)
            {
                case GameState.PreGame:
                    state = GameState.InGame;
                    break;

                case GameState.InGame:
                    CheckWinConditions();
                    CheckPlayerUpgradePoints();
                    RunGui();
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

        if (Input.GetButtonDown("Screenshot"))
        {
            TakeScreenshot();
        }

    }

    /// <summary>
    /// While script is observed (in a PhotonView), this is called by PUN with a stream to write or read.
    /// </summary>
    /// <remarks>
    /// The property stream.isWriting is true for the owner of a PhotonView. This is the only client that
    /// should write into the stream. Others will receive the content written by the owner and can read it.
    ///
    /// Note: Send only what you actually want to consume/use, too!
    /// Note: If the owner doesn't write something into the stream, PUN won't send anything.
    /// </remarks>
    /// <param name="stream">Read or write stream to pass state of this GameObject (or whatever else).</param>
    /// <param name="info">Some info about the sender of this stream, who is the owner of this PhotonView (and GameObject).</param>
    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.isWriting)
    //    {
    //        int gmState = (int)state;
    //        int rdTmScre = redTeamScore;
    //        int blTmScre = blueTeamScore;
    //        float lstTick = lastTick;
    //        int tk = tick;
    //        int rdCptrs = redCaptures;
    //        int blCptrs = blueCaptures;
    //        float strtTime = startTime;
    //        int lstTeam = (int)lastJoinedTeam;

    //        stream.Serialize(ref gmState);
    //        stream.Serialize(ref rdTmScre);
    //        stream.Serialize(ref blTmScre);
    //        stream.Serialize(ref lstTick);
    //        stream.Serialize(ref tk);
    //        stream.Serialize(ref rdCptrs);
    //        stream.Serialize(ref blCptrs);
    //        stream.Serialize(ref strtTime);
    //        stream.Serialize(ref lstTeam);
    //    }
    //    else
    //    {
    //        // Receive latest state information
    //        int gmState = (int)state;
    //        int rdTmScre = redTeamScore;
    //        int blTmScre = blueTeamScore;
    //        float lstTick = lastTick;
    //        int tk = tick;
    //        int rdCptrs = redCaptures;
    //        int blCptrs = blueCaptures;
    //        float strtTime = startTime;
    //        int lstTeam = (int)lastJoinedTeam;

    //        stream.Serialize(ref gmState);
    //        stream.Serialize(ref rdTmScre);
    //        stream.Serialize(ref blTmScre);
    //        stream.Serialize(ref lstTick);
    //        stream.Serialize(ref tk);
    //        stream.Serialize(ref rdCptrs);
    //        stream.Serialize(ref blCptrs);
    //        stream.Serialize(ref strtTime);
    //        stream.Serialize(ref lstTeam);

    //        state = (GameState)gmState;
    //        redTeamScore = rdTmScre;
    //        blueTeamScore = blTmScre;
    //        lastTick = lstTick;
    //        tick = tk;
    //        redCaptures = rdCptrs;
    //        blueCaptures = blCptrs;
    //        startTime = strtTime;
    //        lastJoinedTeam = (Team)lstTeam;

    //    }
    //}

    private void StartGame()
    {
        state = GameState.InGame;
    }

    private void NextTick()
    {
        lastTick = Time.time;
        tick++;
        redTeamScore += redCaptures;
        blueTeamScore += blueCaptures;
    }

    /// <summary>
    /// Returns the spawnpoints of the specific team.
    /// </summary>
    /// <param name="team">The team of your desired spawnpoints</param>
    public List<PlayerSpawnPoint> GetSpawnPointsWithTeam(Team tm)
    {
        List<PlayerSpawnPoint> spawnpoints = new List<PlayerSpawnPoint>();

        foreach (PlayerSpawnPoint zone in playerSpawnZones)
        {
            if (zone.team == tm)
            {
                spawnpoints.Add(zone);
            }
        }

        return spawnpoints;
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

        redCaptures = 0;
        blueCaptures = 0;
        redBonus = 0;
        blueBonus = 0;

        foreach (var c in captureZones)
        {
            if (c.tier == CaptureZone.ZoneTier.A)
            {
                switch (c.state)
                {
                    case CaptureZone.ZoneState.Unoccupied:
                        break;
                    case CaptureZone.ZoneState.Red:
                        redBonus++;
                        break;
                    case CaptureZone.ZoneState.Blue:
                        blueBonus++;
                        break;
                    default:
                        break;
                }
            }
            
            switch (c.state)
            {
                case CaptureZone.ZoneState.Unoccupied:
                    break;

                case CaptureZone.ZoneState.Red:
                    redCaptures++;
                    break;

                case CaptureZone.ZoneState.Blue:
                    blueCaptures++;
                    break;

                default:
                    break;
            }
        }
        if (redCaptures >= captureZones.Count - 1)
        {
            state = GameState.RedWins;
        }
        else if (blueCaptures >= captureZones.Count - 1)
        {
            state = GameState.BlueWins;
        }

        if (tick >= ticksPerGame)
        {
            if (redTeamScore > blueTeamScore)
            {
                state = GameState.RedWins;
            }
            else if (blueTeamScore > redTeamScore)
            {
                state = GameState.BlueWins;
            }
            else if (blueTeamScore == redTeamScore)
            {
                state = GameState.Tie;
            }
        }
    }

    private void RunGui()
    {
        foreach (var c in captureZones)
        {
            if ((localPlayer.team == Team.Blue && c.state != CaptureZone.ZoneState.Blue && c.blueUnlocked) || (localPlayer.team == Team.Red && c.state != CaptureZone.ZoneState.Red && c.redUnlocked))
            {
                c.rGui.enabled = true;
                Vector3 sPos = Camera.main.WorldToScreenPoint(c.transform.position);
                c.rGui.relativePosition = new Vector2(sPos.x, -sPos.y);
                c.rGui.size = new Vector2((Mathf.Sin(Time.time * 4) + 1) * 32 + 64, (Mathf.Sin(Time.time * 4) + 1) * 32 + 64);
            }
            else
            {
                c.rGui.enabled = false;
            }
        }
    }

    public void CheckPlayerUpgradePoints()
    {
        //TODO: Send level up notification to other clients.
        if (localPlayer != null)
        {
            if (localPlayer.upgradePoints >= Convert.ToInt32(upgradePointReqData[localPlayer.stats.level][0]))
            {
                photonView.RPC("SetPlayerLevel", PhotonTargets.AllBuffered, PhotonNetwork.player, localPlayer.stats.level + 1);
                Debug.Log("Level up!");
            }
        }

    }

    public GameObject createObject(KBConstants.ObjectConstants.type objectType, Vector3 position, Quaternion rotation, Team newTeam)
    {
        switch (objectType)
        {
            case ObjectConstants.type.Player:
                {
                    GameObject newPlayerObject = PhotonNetwork.Instantiate(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.Player], position, rotation, 0);
                    PlayerLocal newPlayer = newPlayerObject.GetComponent<PlayerLocal>();
                    newPlayer.networkPlayer = PhotonNetwork.player;
                    newPlayer.team = newTeam;

                    return newPlayerObject;
                }

            case ObjectConstants.type.Item:
                {
                    GameObject newItemObject = PhotonNetwork.Instantiate(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.Item], position, rotation, 0);
                    Item newItem = newItemObject.GetComponent<Item>();
                    newItem.team = newTeam;
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

    public static string GetTeamScoreString()
    {
        string tab = "     ";
        string spacer = " : ";
        GameManager gm = GameManager.instance;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("Tick " + gm.tick + "/" + gm.ticksPerGame + System.Environment.NewLine);
        sb.Append("Red" + spacer + gm.redTeamScore + System.Environment.NewLine);
        sb.Append("Blue" + spacer + gm.blueTeamScore + System.Environment.NewLine);
        return sb.ToString();
    }

    private void ReadPlayerStatData()
    {
        playerStatData = CSVReader.ReadFile(KBConstants.ManagerConstants.PREFAB_NAMES[ManagerConstants.type.PlayerStats]);
    }

    private void ReadUpgradePointData()
    {
        upgradePointReqData = CSVReader.ReadFile(KBConstants.ManagerConstants.PREFAB_NAMES[ManagerConstants.type.UpgradePointReqs]);
    }

    //[RPC]
    //void SpawnOnNetwork(Vector3 pos, Quaternion rot, PhotonViewID id1, PhotonPlayer np)
    //{

    //    Transform newPlayer = Instantiate(playerPrefab, pos, rot) as Transform;
    //    //Set transform
    //    PlayerInfo4 pNode = GetPlayer(np);
    //    pNode.transform = newPlayer;
    //    //Set photonviewID everywhere!
    //    SetPhotonViewIDs(newPlayer.gameObject, id1);

    //    if (pNode.IsLocal())
    //    {
    //        localPlayerInfo = pNode;
    //    }

    //    //Maybe call some specific action on the instantiated object?
    //    //PLAYERSCRIPT tmp = newPlayer.GetComponent<PLAYERSCRIPT>();
    //    //tmp.SetPlayer(pNode.networkPlayer);
    //}

    [RPC]
    public void SetPlayerLevel(PhotonPlayer phPlayer, int playerLevel)
    {
        GameManager gm = GameManager.instance;
        int numberOfStats = 8;
        int initStatColumn = 2;
        int perLevelStatColumn = 3;
        PlayerStats stats = new PlayerStats();
        stats.statArray = new float[numberOfStats];

        PlayerLocal player = null;

        if (phPlayer.isLocal)
        {
            player = localPlayer;
        }
        else
        {
            PlayerLocal currentPlayer;
            for (int i = 0; i < players.Count; i++)
            {
                currentPlayer = players[i];
                if (currentPlayer.networkPlayer == phPlayer)
                {
                    player = currentPlayer;
                }
            }
        }


        if (player == null)
        {
            Debug.Log("Warning! Couldn't find player to set level");
            return;
        }


        for (int i = 0; i < stats.statArray.Length; i++) // This loads the raw stats into a float[]
        {
            // finalStatValue = init + (level - 1)*(perLevelMultiplier)
            stats.statArray[i] = Convert.ToInt32(gm.playerStatData[i + ((int)player.type * numberOfStats)][initStatColumn]) + ((playerLevel - 1) * Convert.ToInt32(gm.playerStatData[i + ((int)player.type * numberOfStats)][perLevelStatColumn]));
        }

        stats.level = playerLevel;
        stats.health = (int)stats.statArray[(int)PlayerStats.PlayerStatNames.Health];
        stats.attack = (int)stats.statArray[(int)PlayerStats.PlayerStatNames.Attack];
        stats.attackRange = (int)stats.statArray[(int)PlayerStats.PlayerStatNames.AttackRange];
        stats.captureSpeed = (int)stats.statArray[(int)PlayerStats.PlayerStatNames.CaptureSpeed];
        stats.lowerbodyRotationSpeed = (int)stats.statArray[(int)PlayerStats.PlayerStatNames.LBRotationSpeed];
        stats.upperbodyRotationSpeed = (int)stats.statArray[(int)PlayerStats.PlayerStatNames.UBRotationSpeed];
        stats.speed = (int)stats.statArray[(int)PlayerStats.PlayerStatNames.MovementSpeed];
        stats.visionRange = (int)stats.statArray[(int)PlayerStats.PlayerStatNames.VisionRange];
        player.stats = stats;
    }

    private void TakeScreenshot()
    {
        string path = Application.persistentDataPath + "/" + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "_" + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + System.DateTime.Now.Second.ToString() + "_kaiju_scr.png";
        //string path = System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + System.DateTime.Now.Second.ToString() + "_kaiju_scr";
        Application.CaptureScreenshot(path, 2);
        path = Application.persistentDataPath + "/" + path;
        Debug.Log(path);
    }
}