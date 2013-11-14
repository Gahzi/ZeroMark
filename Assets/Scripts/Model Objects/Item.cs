using UnityEngine;
using System.Collections;
using KBConstants;

public class Item : KBGameObject {

    public enum ItemState { isDown, isPickedUp };

    ItemState state;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collision)
    {
        state = ItemState.isPickedUp;
        Debug.Log("GameItem was Picked Up");
    }
}
