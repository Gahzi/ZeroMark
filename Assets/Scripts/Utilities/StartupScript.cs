using UnityEngine;
using System.Collections;
using KBConstants;

public class StartupScript : MonoBehaviour
{
    void Awake()
    {
        PhotonNetwork.offlineMode = true;
        GamepadInfoHandler newGamepadManager = (GamepadInfoHandler)Instantiate(Resources.Load(ManagerConstants.PREFAB_NAMES[ManagerConstants.type.GamepadManager], typeof(GamepadInfoHandler)));
        newGamepadManager.tag = ManagerConstants.PREFAB_TAGS[ManagerConstants.type.GamepadManager];
        GameManager newManager = (GameManager)Instantiate(Resources.Load(ManagerConstants.PREFAB_NAMES[ManagerConstants.type.GameManager], typeof(GameManager)));
        newManager.tag = ManagerConstants.PREFAB_TAGS[ManagerConstants.type.GameManager];

    }


    // Use this for initialization
    void Start()
    {


        //newManager.createObject(ObjectConstants.type.FactoryGroup);
        //Instantiate(Resources.Load<FactoryGroup>(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.FactoryGroup]), new Vector3(-200, 0, -100), Quaternion.identity);

        //Kaiju kaiju = (Kaiju)Instantiate(Resources.Load(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.Kaiju], typeof(Kaiju)), new Vector3(0, 0, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
