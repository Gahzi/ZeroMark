using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.RotateAround(transform.parent.transform.position, Vector3.up, 10.0f * Time.deltaTime);
	}
}
