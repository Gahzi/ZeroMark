using UnityEngine;
using System.Collections;

public class GUISelectCube : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.parent != null)
        {
            transform.position = transform.parent.position;
            transform.rotation = transform.parent.rotation;
            transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        }

	}
}
