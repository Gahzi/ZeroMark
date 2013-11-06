using UnityEngine;
using System.Collections;
using KBConstants;

public class StartupScript : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
        GamepadInfoHandler newGamepadManager = (GamepadInfoHandler)Instantiate(Resources.Load(ManagerConstants.PREFAB_NAMES[ManagerConstants.type.GamepadManager], typeof(GamepadInfoHandler)));
        newGamepadManager.tag = ManagerConstants.PREFAB_TAGS[ManagerConstants.type.GamepadManager];
        GameManager newManager = (GameManager)Instantiate(Resources.Load(ManagerConstants.PREFAB_NAMES[ManagerConstants.type.GameManager], typeof(GameManager)));
        newManager.tag = ManagerConstants.PREFAB_TAGS[ManagerConstants.type.GameManager];

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
