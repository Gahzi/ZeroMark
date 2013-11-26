using UnityEngine;
using System.Collections;

public class FactoryGroup : MonoBehaviour {

    private Factory[] factory;

	// Use this for initialization
	void Start () {
       factory = gameObject.GetComponentsInChildren<Factory>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
