using UnityEngine;
using System.Collections;
using KBConstants;

/// <summary>
/// Our model object for game items. This should be extended by every game item in the game.
/// </summary>
public class Item : KBGameObject {

    public enum ItemState { isDown, isPickedUp };

    ItemState state;

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
    /// 
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        Digger colDigger = collision.gameObject.GetComponent<Digger>();

        if (colDigger != null)
        {
            colDigger.itemInventory.Add(this);
            Destroy(this);
        }
        
    }
}
