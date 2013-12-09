using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]

public class HitboxBaseScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        rigidbody.isKinematic = true;
        collider.isTrigger = true;
        gameObject.tag = "Hitbox";
        gameObject.layer = LayerMask.NameToLayer("Hitboxes1");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
