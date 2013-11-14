using UnityEngine;
using System.Collections;

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
}
