using UnityEngine;
using System.Collections;

public class KBCamera : MonoBehaviour
{

    private Vector3 CAMERA_FOLLOW_DISTANCE = new Vector3(0, 20, -65);

    public KBGameObject attachedObject;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (attachedObject != null)
        {
            //Set position and rotation
            Vector3 newFollowPosition = attachedObject.transform.position + CAMERA_FOLLOW_DISTANCE;
            Quaternion newFollowRotation = attachedObject.transform.rotation;

            transform.position = newFollowPosition;
            transform.rotation = newFollowRotation;
        }
    }
}
