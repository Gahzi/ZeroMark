using UnityEngine;
using System.Collections;

/// <summary>
/// This class is meant to be the link between the moveable game objects and the player object that currently exists.
/// </summary>
public class KBControllableGameObject : KBGameObject {

    public Player attachedPlayer = null;

	/// <summary>
    /// Use this for initialization
	/// </summary>
	void Start () {
	
	}
	
	/// <summary>
    /// Update is called once per frame
	/// </summary>
	void Update () {
	
	}

    /// <summary>
    /// Checks if a player is currently attached to the object
    /// </summary>
    /// <returns> Boolean, true if attached and false otherwise</returns>
    public bool isPlayerAttached()
    {
        if (attachedPlayer != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Unnattach the player from the game object
    /// </summary>
    void unattachedPlayer()
    {

    }
}
