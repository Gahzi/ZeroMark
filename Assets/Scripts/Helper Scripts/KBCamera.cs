using UnityEngine;
using System.Collections;

public class KBCamera : MonoBehaviour
{

    private Vector3 CAMERA_FOLLOW_DISTANCE = new Vector3(0, 20, -65);

    public Player attachedPlayer;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (attachedPlayer != null)
        {
            //Set position and rotation
            Vector3 newFollowPosition = attachedPlayer.transform.position + CAMERA_FOLLOW_DISTANCE;
            Quaternion newFollowRotation = attachedPlayer.transform.rotation;

            transform.position = newFollowPosition;
            transform.rotation = newFollowRotation;
        }
    }
}
