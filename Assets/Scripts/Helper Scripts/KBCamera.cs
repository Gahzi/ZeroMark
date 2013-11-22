using UnityEngine;
using System.Collections;

public class KBCamera : MonoBehaviour
{

    private Vector3 CAMERA_FOLLOW_DISTANCE = new Vector3(0, 20 / 4, -65 / 8);
    private Quaternion targetCameraUpRotation;

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
            //Vector3 newFollowPosition = attachedObject.transform.position + CAMERA_FOLLOW_DISTANCE;
            //Quaternion newFollowRotation = attachedObject.transform.rotation;

            //transform.position = newFollowPosition;
            //transform.rotation = newFollowRotation;
            transform.parent = attachedObject.transform;
            transform.localPosition = CAMERA_FOLLOW_DISTANCE;

            Player player = (Player)attachedObject;
            float targetUpRotation = (Mathf.Rad2Deg * Mathf.Atan2(player.gamepad.rightStick.y, 0) / 90) * 20;
            Debug.Log(targetUpRotation);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetUpRotation, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z), 10.0f * Time.deltaTime);


        }
    }
}
