using KBConstants;
using UnityEngine;

public class StartupScript : MonoBehaviour
{
    private void Awake()
    {
        
    }

    private void Start()
    {
        PhotonNetwork.isMessageQueueRunning = true;
        if (PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.Instantiate(ManagerConstants.PREFAB_NAMES[ManagerConstants.type.GameManager], Vector3.zero, Quaternion.identity, 0);
        }
    }

    private void Update()
    {

    }
}