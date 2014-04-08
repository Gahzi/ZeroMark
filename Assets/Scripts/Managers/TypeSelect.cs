using UnityEngine;
using KBConstants;

public class TypeSelect : MonoBehaviour
{

    public PlayerType type;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // trigger player spawn
            // change player type
            // notify other clients
        }
    }
    
}