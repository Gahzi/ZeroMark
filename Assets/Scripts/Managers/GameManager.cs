using UnityEngine;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using KBConstants;

public class GameManager : MonoBehaviour {
	
	List<Player> players;
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
                GameObject newPlayerObject = PhotonNetwork.Instantiate(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.Player],Vector3.zero, Quaternion.identity, 0);
                Player newPlayer = newPlayerObject.GetComponent<Player>();
                players.Add(newPlayer);
				break;
			}
			
			default:
			break;
		}
	}
}
