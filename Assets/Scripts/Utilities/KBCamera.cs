using UnityEngine;

public class KBCamera : MonoBehaviour
{
    private Vector3 CAMERA_FOLLOW_DISTANCE;
    private Quaternion targetCameraUpRotation;
    public PlayerLocal attachedPlayer;
    public Camera camera;

    private void Start()
    {
        camera = GetComponent<Camera>();
        camera.fieldOfView = 90.0f;
        transform.rotation = Quaternion.identity;
        transform.parent = attachedPlayer.transform;
        switch (attachedPlayer.controlStyle)
        {
            case PlayerLocal.ControlStyle.ThirdPerson:
                camera.isOrthoGraphic = false;
                CAMERA_FOLLOW_DISTANCE = new Vector3(0, 3.0f, -2.0f);
                transform.localPosition = CAMERA_FOLLOW_DISTANCE;
                transform.Rotate(Vector3.right, 15);
                break;

            case PlayerLocal.ControlStyle.TopDown:
                camera.isOrthoGraphic = true;
                CAMERA_FOLLOW_DISTANCE = new Vector3(0, 20, 0);
                transform.Rotate(Vector3.right, 90);
                transform.localPosition = CAMERA_FOLLOW_DISTANCE;
                break;

            default:
                break;
        }
    }

    private void Update()
    {
        if (attachedPlayer != null)
        {
            transform.localPosition = CAMERA_FOLLOW_DISTANCE;
        }
    }
}