using UnityEngine;

public class KBCamera : MonoBehaviour
{
    private Vector3 CAMERA_FOLLOW_DISTANCE = new Vector3(0, 3.0f, -2.0f);
    private Quaternion targetCameraUpRotation;

    public KBGameObject attachedObject;

    // Use this for initialization
    private void Start()
    {
        camera.fieldOfView = 90.0f;
        transform.rotation = Quaternion.identity;
        transform.parent = attachedObject.transform;
        transform.localPosition = CAMERA_FOLLOW_DISTANCE;
        transform.Rotate(Vector3.right, 15);
    }

    // Update is called once per frame
    private void Update()
    {
        if (attachedObject != null)
        {
            transform.localPosition = CAMERA_FOLLOW_DISTANCE;
            //Player player = (Player)attachedObject;
            //float targetUpRotation = (Mathf.Rad2Deg * Mathf.Atan2(player.gamepad.rightStick.y, 0) / 90) * 20;
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetUpRotation, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z), 10.0f * Time.deltaTime);
        }
    }
}