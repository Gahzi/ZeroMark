using KBConstants;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Photon.MonoBehaviour
{
    public enum GameState { PreGame, InGame, RedWins, BlueWins, Tie, EndGame };

    public enum GameType { CapturePoint, DataPulse, Deathmatch };

    public GameType gameType;

    public KBPlayer localPlayer;
    public List<KBPlayer> players;
    public List<KillTag> killTags;

    public List<BankZone> bankZones;
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
    public int redCaptures = 0;
    public int blueCaptures = 0;
    private List<List<String>> playerStatData;
    private float startTime;
    public float gameTime;
    public float gameTimeMax;
    public int dataPulsePeriod = 60;
    private float lastDataPulse;
    public HitFX dataPulseEffect;

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
        bankZones = new List<BankZone>();
        playerSpawnZones = new List<PlayerSpawnPoint>();
    }

    private void Start()
    {
        PhotonNetwork.isMessageQueueRunning = true;
        startTime = Time.time;
        state = GameState.PreGame;
        gameType = GameType.DataPulse;

        KillTag[] loadedKillTags = FindObjectsOfType<KillTag>();
        BankZone[] loadedBankZones = FindObjectsOfType<BankZone>();
        PlayerSpawnPoint[] loadedPSpawns = FindObjectsOfType<PlayerSpawnPoint>();
        ObjectPool.CreatePool(dataPulseEffect);

        foreach (BankZone b in loadedBankZones)
        {
            bankZones.Add(b);
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
            Team nextTeam = Team.None;
            GameManager.Instance.CreateObject((int)ObjectConstants.type.Player, Vector3.zero, Quaternion.identity, (int)nextTeam);
        }
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

    //<summary>
    //While script is observed (in a PhotonView), this is called by PUN with a stream to write or read.
    //</summary>
    //<remarks>
    //The property stream.isWriting is true for the owner of a PhotonView. This is the only client that
    //should write into the stream. Others will receive the content written by the owner and can read it.
    private void StartGame()
    {
        state = GameState.InGame;
    }

    private void OnGUI()
    {
        //GUI.Box(new Rect(Screen.width / 2 - 50, 0, 100, 40), "Blue" + System.Environment.NewLine +  blueTeamScore.ToString());
        //GUI.Box(new Rect(Screen.width / 2 + 50, 0, 100, 40), "Red" + System.Environment.NewLine + redTeamScore.ToString());
    }

    private void FixedUpdate()
    {
        if (localPlayer != null)
        {
            switch (state)
            {
                case GameState.PreGame:
                    gameTimeMax = 60.0f * 5.0f;
                    lastDataPulse = Time.time;
                    state = GameState.InGame;
                    break;

                case GameState.InGame:
                    gameTime += Time.deltaTime;

                    CheckGameOver();

                    //CheckPlayerUpgradePoints();
                    //RunGui();
                    break;

                case GameState.RedWins:
                    Debug.Log("Red won");
                    localPlayer.acceptingInputs = false;
                    state = GameState.EndGame;
                    GUIManager.Instance.state = GUIManager.GUIManagerState.ShowingEndGameTab;
                    break;

                case GameState.BlueWins:
                    Debug.Log("Blue won");
                    localPlayer.acceptingInputs = false;
                    state = GameState.EndGame;
                    GUIManager.Instance.state = GUIManager.GUIManagerState.ShowingEndGameTab;
                    break;

                case GameState.Tie:
                    Debug.Log("Tie");
                    state = GameState.EndGame;

                    break;

                default:
                    break;
            }
        }

        if (Input.GetButtonDown("Screenshot"))
        {
            GameManager.TakeScreenshot(1);
        }
    }

    public GameObject CreateObject(int type, Vector3 position, Quaternion rotation, int newTeam)
    {
        KBConstants.ObjectConstants.type newType = (KBConstants.ObjectConstants.type)type;

        switch (newType)
        {
            case ObjectConstants.type.Player:
                {
                    GameObject newPlayerObject = PhotonNetwork.Instantiate(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.Player], position, rotation, 0);
                    KBPlayer newPlayer = newPlayerObject.GetComponent<KBPlayer>();
                    newPlayer.photonView.RPC("Setup", PhotonTargets.AllBuffered, PhotonNetwork.player, (int)newTeam);
                    newPlayer.photonView.RPC("SwitchType", PhotonTargets.AllBuffered, "SpawnCore", 0);
                    return newPlayerObject;
                }

            case ObjectConstants.type.KillTagBlue:
                {
                    GameObject newKillTagBlueObject = PhotonNetwork.Instantiate(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.KillTagBlue], position, Quaternion.Euler(33.3f, 330.0f, 48.36f), 0);
                    KillTag newKillTagBlue = newKillTagBlueObject.GetComponent<KillTag>();
                    newKillTagBlue.team = (Team)newTeam;
                    return newKillTagBlueObject;
                }

            case ObjectConstants.type.KillTagRed:
                {
                    GameObject newKillTagRedObject = PhotonNetwork.Instantiate(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.KillTagRed], position, Quaternion.Euler(33.3f, 330.0f, 48.36f), 0);
                    KillTag newKillTagRed = newKillTagRedObject.GetComponent<KillTag>();
                    newKillTagRed.team = (Team)newTeam;
                    return newKillTagRedObject;
                }
            default:
                return null;
        }
    }

    [RPC]
    public void DestroyObject(int viewId)
    {
        PhotonView viewToDestroy = PhotonView.Find(viewId);
        GameObject objectToDestroy = viewToDestroy.gameObject;

        if (viewToDestroy.isMine)
        {
            PhotonNetwork.Destroy(objectToDestroy);
        }
    }

    private void RunDataPulse()
    {
        photonView.RPC("AddPointsToScore", PhotonTargets.All, localPlayer.team, localPlayer.currentPoints);
        localPlayer.ScorePoints(localPlayer.currentPoints);
        lastDataPulse = Time.time;
        HitFX o = ObjectPool.Spawn(dataPulseEffect);
        o.Init();
    }

    [RPC]
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
        string s = string.Format("Banked {0} points for team {1}", points, Enum.GetName(typeof(Team), team));
        Debug.Log(s);
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

    /// <summary>
    /// Checks to see if the 2/3 capture points are taken.
    /// </summary>
    /// <returns>True if a team has more than 2/3rds of the capture points</returns>
    private void CheckGameOver()
    {
        switch (gameType)
        {
            case GameType.CapturePoint:

                int redBankCaptureCount = 0;
                int blueBankCaptureCount = 0;

                for (int i = 0; i < bankZones.Count; i++)
                {
                    BankZone bz = bankZones[i];
                    switch (bz.team)
                    {
                        case Team.Blue:
                            {
                                blueBankCaptureCount++;
                                break;
                            }

                        case Team.Red:
                            {
                                redBankCaptureCount++;
                                break;
                            }

                        case Team.None:
                            {
                                break;
                            }
                    }
                }

                if (redBankCaptureCount > bankZones.Count / 2)
                {
                    state = GameState.RedWins;
                }
                else if (blueBankCaptureCount > bankZones.Count / 2)
                {
                    state = GameState.BlueWins;
                }
                break;

            case GameType.DataPulse:

                if (Time.time > lastDataPulse + dataPulsePeriod)
                {
                    RunDataPulse();
                }

                if (IsGameTimeOver())
                {
                    if (redTeamScore > blueTeamScore)
                    {
                        state = GameState.RedWins;
                    }
                    else if (blueTeamScore > redTeamScore)
                    {
                        state = GameState.BlueWins;
                    }
                }

                break;

            case GameType.Deathmatch:

                // Add primary check for score limit here
                if (IsGameTimeOver())
                {
                    if (redTeamScore > blueTeamScore)
                    {
                        state = GameState.RedWins;
                    }
                    else if (blueTeamScore > redTeamScore)
                    {
                        state = GameState.BlueWins;
                    }
                }

                break;

            default:
                break;
        }
    }

    private void SendGlobalMessageToPlayers(string msg)
    {
        // TODO
    }

    public KBPlayer FindClosestPlayer(KBPlayer fromPlayer, float minimumDist, bool oppositeTeamOnly)
    {
        List<KBPlayer> players = GameManager.Instance.players;
        KBPlayer target = null;
        float closest = 0;
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].GetHashCode() != fromPlayer.GetHashCode())
            {
                float distance = 0;
                if (oppositeTeamOnly && players[i].team != fromPlayer.team)
                {
                    distance = Vector3.Distance(fromPlayer.transform.position, players[i].transform.position);
                }
                else if (!oppositeTeamOnly)
                {
                    distance = Vector3.Distance(fromPlayer.transform.position, players[i].transform.position);
                }
                if (distance > minimumDist)
                {
                    if (closest == 0)
                    {
                        closest = distance;
                        target = players[i];
                    }
                    else if (closest != 0 && distance < closest)
                    {
                        closest = distance;
                        target = players[i];
                    }
                }
            }
        }
        return target;
    }

    public static void TakeScreenshot(int res)
    {
        string path = Application.persistentDataPath + "/" + System.DateTime.Now.Year.ToString() +
            System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "_" +
            System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() +
            System.DateTime.Now.Second.ToString() + "_zm_scr.png";

        Application.CaptureScreenshot(path, res);
        path = Application.persistentDataPath + "/" + path;
        Debug.Log(path);
    }

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
            int rdCptrs = redCaptures;
            int blCptrs = blueCaptures;
            float strtTime = startTime;
            float gmTime = gameTime;
            float gmTimeMax = gameTimeMax;

            stream.Serialize(ref gmState);
            stream.Serialize(ref rdTmScre);
            stream.Serialize(ref blTmScre);
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

            int rdCptrs = redCaptures;
            int blCptrs = blueCaptures;
            float strtTime = startTime;
            float gmTime = gameTime;
            float gmTimeMax = gameTimeMax;

            stream.Serialize(ref gmState);
            stream.Serialize(ref rdTmScre);
            stream.Serialize(ref blTmScre);
            stream.Serialize(ref rdCptrs);
            stream.Serialize(ref blCptrs);
            stream.Serialize(ref strtTime);
            stream.Serialize(ref gmTime);
            stream.Serialize(ref gmTimeMax);

            state = (GameState)gmState;
            redTeamScore = rdTmScre;
            blueTeamScore = blTmScre;

            redCaptures = rdCptrs;
            blueCaptures = blCptrs;
            startTime = strtTime;
            gameTime = gmTime;
            gameTimeMax = gmTimeMax;
        }
    }
}