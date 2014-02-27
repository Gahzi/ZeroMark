using KBConstants;
using UnityEngine;

public class StartupScript : MonoBehaviour
{
    private void Awake()
    {
        
    }

    private void Start()
    {
        if (PhotonNetwork.isMasterClient)
        {
           
            //PhotonNetwork.RPC("");
        }
    }
    private void Update()
    {
    }
}