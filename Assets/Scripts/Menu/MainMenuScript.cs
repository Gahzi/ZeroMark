﻿using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MainMenuScript : Photon.MonoBehaviour
{
    //GUI vars

    private string currentGUIMethod = "join";
    private string currentGUIWindow = "none";

    private Vector2 JoinScrollPosition;

    private string joinRoomName;

    private string failConnectMesage = "";
    private bool isConnectingToRoom = false;

    public Font menuFont;
    public GUISkin skin;

    public GameObject playButton;
    public AudioClip pressClip;

    private void Awake()
    {
        //Default join values
        joinRoomName = "";

        //Default host values
        hostTitle = PlayerPrefs.GetString("hostTitle", "Guests server");
        hostMaxPlayers = PlayerPrefs.GetInt("hostPlayers", 8);

        if (!PhotonNetwork.connected)
            PhotonNetwork.ConnectUsingSettings("1.0");
    }

    private void OnConnectedToPhoton()
    {
        Debug.Log("This client has connected to a server");
        failConnectMesage = "";
    }

    private void OnDisconnectedFromPhoton()
    {
        Debug.Log("This client has disconnected from the server");
        failConnectMesage = "Disconnected from Photon";
    }

    private void OnFailedToConnectToPhoton(ExitGames.Client.Photon.StatusCode status)
    {
        Debug.Log("Failed to connect to Photon: " + status);
        failConnectMesage = "Failed to connect to Photon: " + status;
    }

    private void OnGUI()
    {
        GUI.skin = skin;
        if (!PhotonNetwork.connected)
        {
            GUILayout.Label("Connecting..");
            if (failConnectMesage != "")
            {
                GUILayout.Label("Error message: " + failConnectMesage);
                if (GUILayout.Button("Retry"))
                {
                    failConnectMesage = "";
                    PhotonNetwork.ConnectUsingSettings("1.0");
                    audio.PlayOneShot(pressClip);
                }
            }
        }
        else if (currentGUIWindow == "serverMenu")
        {
            GUILayout.Window(2, new Rect(Screen.width / 2 - 600 / 2, Screen.height / 2 - 550 / 2, 600, 550), ServerMenu, "");
        }
    }

    private void ServerMenu(int wID)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Multiplayer menu", "menuText");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (currentGUIMethod == "join")
        {
            GUILayout.Label("Join", "menuText");
        }
        else
        {
            if (GUILayout.Button("Join"))
            {
                currentGUIMethod = "join";
                audio.PlayOneShot(pressClip);
            }
        }
        if (currentGUIMethod == "create")
        {
            GUILayout.Label("Create", "menuText");
        }
        else
        {
            if (GUILayout.Button("Create"))
            {
                currentGUIMethod = "create";
                audio.PlayOneShot(pressClip);
            }
        }

        GUILayout.EndHorizontal();
        GUILayout.Space(25);

        if (currentGUIMethod == "join")
            JoinMenu();
        else
            HostMenu();
    }

    private void JoinMenu()
    {
        if (isConnectingToRoom)
        {
            GUILayout.Label("Trying to connect to a room.", "menuText");
        }
        else if (failConnectMesage != "")
        {
            GUILayout.Label("The game failed to connect:\n" + failConnectMesage, "menuText");
            GUILayout.Space(10);
            if (GUILayout.Button("Cancel"))
            {
                failConnectMesage = "";
                audio.PlayOneShot(pressClip);
            }
        }
        else
        {
            //Masterlist
            GUILayout.BeginHorizontal();
            GUILayout.Label("Game list:", "menuText");

            GUILayout.FlexibleSpace();
            if (PhotonNetwork.GetRoomList().Length > 0 &&
                GUILayout.Button("Join random game"))
            {
                PhotonNetwork.JoinRandomRoom();
                audio.PlayOneShot(pressClip);
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(2);
            GUILayout.BeginHorizontal();
            GUILayout.Space(24);

            GUILayout.Label("Title", GUILayout.Width(200));
            GUILayout.Label("Players", GUILayout.Width(55));
            GUILayout.EndHorizontal();

            JoinScrollPosition = GUILayout.BeginScrollView(JoinScrollPosition);
            foreach (RoomInfo room in PhotonNetwork.GetRoomList())
            {
                GUILayout.BeginHorizontal();

                if ((room.playerCount < room.maxPlayers || room.maxPlayers <= 0) &&
                    GUILayout.Button("" + room.name, GUILayout.Width(200)))
                {
                    PhotonNetwork.JoinRoom(room.name);
                    audio.PlayOneShot(pressClip);
                }
                GUILayout.Label(room.playerCount + "/" + room.maxPlayers, GUILayout.Width(55));

                GUILayout.EndHorizontal();
            }
            if (PhotonNetwork.GetRoomList().Length == 0)
            {
                GUILayout.Label("No games are running right now", "menuText");
            }
            GUILayout.EndScrollView();

            string text = PhotonNetwork.GetRoomList().Length + " total rooms";
            GUILayout.Label(text);

            //DIRECT JOIN

            GUILayout.BeginHorizontal();
            GUILayout.Label("Join by name:", "menuText");
            GUILayout.Space(5);
            GUILayout.Label("Room name", "menuText");
            joinRoomName = (GUILayout.TextField(joinRoomName + "", GUILayout.Width(50)) + "");

            if (GUILayout.Button("Connect"))
            {
                PhotonNetwork.JoinRoom(joinRoomName);
                audio.PlayOneShot(pressClip);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(4);
        }
    }

    private void OnPhotonCreateRoomFailed()
    {
        Debug.Log("A CreateRoom call failed, most likely the room name is already in use.");
        failConnectMesage = "Could not create new room, the name is already in use.";
    }

    private void OnPhotonJoinRoomFailed()
    {
        Debug.Log("A JoinRoom call failed, most likely the room name does not exist or is full.");
        failConnectMesage = "Could not connect to the desired room, this room does no longer exist or all slots are full.";
    }

    private void OnPhotonRandomJoinFailed()
    {
        Debug.Log("A JoinRandom room call failed, most likely there are no rooms available.");
        failConnectMesage = "Could not connect to random room; no rooms were available.";
    }

    private void OnJoinedRoom()
    {
        //Stop communication until in the game
        PhotonNetwork.isMessageQueueRunning = false;
        Application.LoadLevel(Application.loadedLevel + 1);
    }

    private string hostTitle;

    //private string hostDescription;
    private int hostMaxPlayers;

    private void HostMenu()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Host a new game:");
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Title:");
        GUILayout.FlexibleSpace();
        hostTitle = GUILayout.TextField(hostTitle, GUILayout.Width(200));
        GUILayout.EndHorizontal();

        /*GUILayout.BeginHorizontal();
        GUILayout.Label("Server description");
        GUILayout.FlexibleSpace();
        hostDescription = GUILayout.TextField(hostDescription, GUILayout.Width(200));
        GUILayout.EndHorizontal();
        */

        GUILayout.BeginHorizontal();
        GUILayout.Label("Max players (1-32)");
        GUILayout.FlexibleSpace();
        hostMaxPlayers = int.Parse(GUILayout.TextField(hostMaxPlayers + "", GUILayout.Width(50)) + "");
        GUILayout.EndHorizontal();

        CheckHostVars();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Start server", GUILayout.Width(150)))
        {
            StartHostingGame(hostTitle, hostMaxPlayers);
            audio.PlayOneShot(pressClip);
        }
        GUILayout.EndHorizontal();
    }

    private void CheckHostVars()
    {
        hostMaxPlayers = Mathf.Clamp(hostMaxPlayers, 1, 64);
    }

    private void StartHostingGame(string hostSettingTitle, int hostPlayers)
    {
        if (hostSettingTitle == "")
        {
            hostSettingTitle = "NoTitle";
        }

        hostPlayers = Mathf.Clamp(hostPlayers, 0, 64);

        PhotonNetwork.CreateRoom(hostSettingTitle, true, true, hostPlayers);
    }

    public void CloseAllWindows()
    {
        currentGUIWindow = "none";
    }

    public void OpenServerBrowser()
    {
        currentGUIWindow = "serverMenu";
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    //
    // CUSTOM HOST LIST
    //
    // You could use this to implement custom sorting, or adding custom fields.
    //

    /*
    private List<MyRoomData> hostDataList = new List<MyRoomData>();

    void OnReceivedRoomList()
    {
        Debug.Log("We received a new room list, total rooms: " + PhotonNetwork.GetRoomList().Length);
        ReloadHostList();
    }

    void OnReceivedRoomListUpdate()
    {
        Debug.Log("We received a room list update, total rooms now: " + PhotonNetwork.GetRoomList().Length);
        ReloadHostList();
    }

    void ReloadHostList()
    {
        hostDataList =new List<MyRoomData>();
        foreach(Room room in PhotonNetwork.GetRoomList())
        {
            MyRoomData cHost= new MyRoomData();
            cHost.room = room;

            hostDataList.Add(cHost);
        }
    }

    public class MyRoomData
    {
        public Room room;

        public string title
        {
            get { return room.name; }
        }
        public int connectedPlayers
        {
            get { return room.playerCount; }
        }
        public int maxPlayers
        {
            get { return room.maxPlayers; }
        }

        //Example custom fields
        public int gameVersion; // You could
    }
     */
}