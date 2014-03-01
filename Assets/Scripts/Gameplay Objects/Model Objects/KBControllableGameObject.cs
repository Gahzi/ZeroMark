using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(Rigidbody))]

/// <summary>
/// This class is meant to be the link between the moveable game objects and the player object that currently exists.
/// </summary>
public class KBControllableGameObject : KBGameObject {

	/// <summary>
    /// Use this for initialization
	/// </summary>
	public override void Start () {
        base.Start();
	}
	
	/// <summary>
    /// Update is called once per frame
	/// </summary>
	void Update () {
	
	}
}
