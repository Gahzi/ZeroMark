using KBConstants;
using UnityEngine;

public class StartupScript : MonoBehaviour
{
    private void Awake()
    {
        PhotonNetwork.offlineMode = true;
        //GamepadInfoHandler newGamepadManager = (GamepadInfoHandler)Instantiate(Resources.Load(ManagerConstants.PREFAB_NAMES[ManagerConstants.type.GamepadManager], typeof(GamepadInfoHandler)));
        //newGamepadManager.tag = ManagerConstants.PREFAB_TAGS[ManagerConstants.type.GamepadManager];
        GameManager newManager = (GameManager)Instantiate(Resources.Load(ManagerConstants.PREFAB_NAMES[ManagerConstants.type.GameManager], typeof(GameManager)));
        newManager.tag = ManagerConstants.PREFAB_TAGS[ManagerConstants.type.GameManager];
    }

    private void Start()
    {
    }
    private void Update()
    {
    }
}