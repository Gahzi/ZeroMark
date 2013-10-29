using UnityEngine;
using System.Collections;

[RequireComponent(typeof(tk2dSprite))]

public class KBGameObject : MonoBehaviour {
	
	tk2dSprite sprite;
	GameManager gm;

	// Use this for initialization
	void Start() 
	{
	 	this.sprite = gameObject.GetComponent<tk2dSprite>();
		gm = GameManager.Instance;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
