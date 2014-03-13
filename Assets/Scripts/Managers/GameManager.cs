using KBConstants;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Photon.MonoBehaviour
{
    public enum GameState { PreGame, InGame, RedWins, BlueWins, Tie, EndGame };

    public KBPlayer localPlayer;
    public List<KBPlayer> players;
    public List<KillTag> killTags;

    public List<CaptureZone> captureZones;
    private List<Item> items;
    private List<PlayerSpawnPoint> playerSpawnZones;
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
    public float gameTime;
    public float gameTimeMax;

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

    private void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        //SHERVIN: SEND A MESSAGE SAYING A NEW PLAYER HAS JOINED
    }

    private void OnPhotonPlayerDisconneced(PhotonPlayer player)
    {
        foreach (KBPlayer currentPlayer in players)
        {
            if (currentPlayer.networkPlayer == player)
            {
                players.Remove(currentPlayer);
            }
        }
    }

    #endregion PHOTON CONNECTION HANDLING

    private void Awake()
    {
        instance = this;
        players = new List<KBPlayer>();
        killTags = new List<KillTag>();
        captureZones = new List<CaptureZone>();
        playerSpawnZones = new List<PlayerSpawnPoint>();
        ReadPlayerStatData();
    }

    private void Start()
    {        
        PhotonNetwork.isMessageQueueRunning = true;
        startTime = Time.time;
        lastTick = Time.time;
        state = GameState.PreGame;

        KillTag[] loadedKillTags = FindObjectsOfType<KillTag>();
        CaptureZone[] loadedCaptureZones = FindObjectsOfType<CaptureZone>();
        ItemSpawn[] loadedItemZones = FindObjectsOfType<ItemSpawn>();
        PlayerSpawnPoint[] loadedPSpawns = FindObjectsOfType<PlayerSpawnPoint>();

        //TODO:Remove 6 value and replace with constant representing max player size;
        //players = new List<KBPlayer>();
        //killTags = new List<KillTag>(loadedKillTags.Length);
        //captureZones = new List<CaptureZone>(loadedCaptureZones.Length);
        //playerSpawnZones = new List<PlayerSpawnPoint>(loadedPSpawns.Length);

        foreach (CaptureZone c in loadedCaptureZones)
        {
            captureZones.Add(c);
        }

        for (int i = 0; i < loadedKillTags.Length; i++)
        {
            killTags.Add(loadedKillTags[i]);
        }

        foreach (PlayerSpawnPoint p in loadedPSpawns)
        {
            playerSpawnZones.Add(p);
        }

        if (PhotonNetwork.connected)
        {
            Team nextTeam = (Team)(PhotonNetwork.otherPlayers.Length % 2);
            GameManager.Instance.CreateObject((int)ObjectConstants.type.Player, Vector3.zero, Quaternion.identity, (int)nextTeam);
            //photonView.RPC("CreateObject", PhotonTargets.MasterClient, , , , );
            
        }
        
    }

    private void FixedUpdate()
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
                    gameTimeMax = 250.0f;
                    state = GameState.InGame;
                    break;

                case GameState.InGame:
                    gameTime += Time.deltaTime;
                    
                    if (IsGameTimeOver())
                    {
                        state = GameState.EndGame;
                    }
                      
                    //CheckPlayerUpgradePoints();
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
                    // check winners here;
                    if (redTeamScore > blueTeamScore)
                    {
                        state = GameState.RedWins;
                    }
                    else if (blueTeamScore > redTeamScore)
                    {
                        state = GameState.BlueWins;
                    }
                    else
                    {
                        state = GameState.Tie;
                    }
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
    
    void OnGUI()
    {
        GUI.Box(new Rect(Screen.width / 2 - 50, 0, 100, 40), "Blue" + System.Environment.NewLine +  blueTeamScore.ToString());
        GUI.Box(new Rect(Screen.width / 2 + 50, 0, 100, 40), "Red" + System.Environment.NewLine + redTeamScore.ToString());
    }
    

     //<summary>
     //While script is observed (in a PhotonView), this is called by PUN with a stream to write or read.
     //</summary>
     //<remarks>
     //The property stream.isWriting is true for the owner of a PhotonView. This is the only client that
     //should write into the stream. Others will receive the content written by the owner and can read it.
    
     //Note: Send only what you actually want to consume/use, too!
     //Note: If the owner doesn't write something into the stream, PUN won't send anything.
     //</remarks>
     //<param name="stream">Read or write stream to pass state of this GameObject (or whatever else).</param>
     //<param name="info">Some info about the sender of this stream, who is the owner of this PhotonView (and GameObject).</param>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            int gmState = (int)state;
            int rdTmScre = redTeamScore;
            int blTmScre = blueTeamScore;
            float lstTick = lastTick;
            int tk = tick;
            int rdCptrs = redCaptures;
            int blCptrs = blueCaptures;
            float strtTime = startTime;
            float gmTime = gameTime;
            float gmTimeMax = gameTimeMax;

            stream.Serialize(ref gmState);
            stream.Serialize(ref rdTmScre);
            stream.Serialize(ref blTmScre);
            stream.Serialize(ref lstTick);
            stream.Serialize(ref tk);
            stream.Serialize(ref rdCptrs);
            stream.Serialize(ref blCptrs);
            stream.Serialize(ref strtTime);
            stream.Serialize(ref gmTime);
            stream.Serialize(ref gmTimeMax);
        }
        else
        {
            // Receive latest state information
            int gmState = (int)state;
            int rdTmScre = redTeamScore;
            int blTmScre = blueTeamScore;
            float lstTick = lastTick;
            int tk = tick;
            int rdCptrs = redCaptures;
            int blCptrs = blueCaptures;
            float strtTime = startTime;
            float gmTime = gameTime;
            float gmTimeMax = gameTimeMax;

            stream.Serialize(ref gmState);
            stream.Serialize(ref rdTmScre);
            stream.Serialize(ref blTmScre);
            stream.Serialize(ref lstTick);
            stream.Serialize(ref tk);
            stream.Serialize(ref rdCptrs);
            stream.Serialize(ref blCptrs);
            stream.Serialize(ref strtTime);
            stream.Serialize(ref gmTime);
            stream.Serialize(ref gmTimeMax);

            state = (GameState)gmState;
            redTeamScore = rdTmScre;
            blueTeamScore = blTmScre;
            lastTick = lstTick;
            tick = tk;
            redCaptures = rdCptrs;
            blueCaptures = blCptrs;
            startTime = strtTime;
            gameTime = gmTime;
            gameTimeMax = gmTimeMax;

        }
    }

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

    public void CreateObject(int type, Vector3 position, Quaternion rotation, int newTeam)
    {
        KBConstants.ObjectConstants.type newType = (KBConstants.ObjectConstants.type)type;

        switch (newType)
        {
            case ObjectConstants.type.Player:
                {
                    GameObject newPlayerObject = PhotonNetwork.Instantiate(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.Player], position, rotation, 0);
                    KBPlayer newPlayer = newPlayerObject.GetComponent<KBPlayer>();
                    newPlayer.photonView.RPC("Setup", PhotonTargets.AllBuffered, PhotonNetwork.player, (int)newTeam);
                    photonView.RPC("SetPlayerStats", PhotonTargets.AllBuffered, PhotonNetwork.player);
                    break;
                }

            case ObjectConstants.type.KillTagBlue:
                {
                    GameObject newKillTakeBlueObject = PhotonNetwork.Instantiate(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.KillTagBlue], position, rotation, 0);
                    KillTag newKillTagBlue = newKillTakeBlueObject.GetComponent<KillTag>();
                    newKillTagBlue.team = (Team)newTeam;
                    killTags.Add(newKillTagBlue);
                    break;
                }

            case ObjectConstants.type.KillTagRed:
                {
                    GameObject newKillTakeRedObject = PhotonNetwork.Instantiate(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.KillTagRed], position, rotation, 0);
                    KillTag newKillTagRed = newKillTakeRedObject.GetComponent<KillTag>();
                    newKillTagRed.team = (Team)newTeam;
                    killTags.Add(newKillTagRed);
                    break;
                }
        }
    }

    [RPC]
    public void DestroyObject(PhotonView phView)
    {
        if (PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.Destroy(phView);
        }
    }

    private void SendGlobalMessageToPlayers(string msg)
    {
        // TODO
    }

    private void ReadPlayerStatData()
    {
        playerStatData = CSVReader.ReadFile(KBConstants.ManagerConstants.PREFAB_NAMES[ManagerConstants.type.PlayerStats]);
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
    public void SetPlayerStats(PhotonPlayer phPlayer)
    {
        GameManager gm = GameManager.instance;
        int numberOfStats = 4;
        int initStatColumn = 2;
        PlayerStats stats = new PlayerStats();
        stats.statArray = new float[numberOfStats];

        KBPlayer player = null;

        if (phPlayer.isLocal)
        {
            player = localPlayer;
        }
        else
        {
            KBPlayer currentPlayer;
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
            stats.statArray[i] = Convert.ToInt32(gm.playerStatData[i + ((int)player.type * numberOfStats)][initStatColumn]);
        }

        stats.health = (int)stats.statArray[(int)PlayerStats.PlayerStatNames.Health];
        stats.lowerbodyRotationSpeed = (int)stats.statArray[(int)PlayerStats.PlayerStatNames.LBRotationSpeed];
        stats.upperbodyRotationSpeed = (int)stats.statArray[(int)PlayerStats.PlayerStatNames.UBRotationSpeed];
        stats.speed = (int)stats.statArray[(int)PlayerStats.PlayerStatNames.MovementSpeed];
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

    public int AddPointsToScore(Team team, int points)
    {
        switch (team)
        {
            case Team.Red:
                redTeamScore += points;
                break;
            case Team.Blue:
                blueTeamScore += points;
                break;
            case Team.None:
                break;
            default:
                break;
        }
        return 0;
    }

    /// <summary>
    /// Checks if time spent in InGame state is greater than max alloted game time. True if time is up.
    /// </summary>
    /// <returns>True is game time is up</returns>
    private bool IsGameTimeOver()
    {
        if (gameTime > gameTimeMax) // game over
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}