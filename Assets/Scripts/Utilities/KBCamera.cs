using UnityEngine;

public class KBCamera : MonoBehaviour
{
    private Vector3 CAMERA_FOLLOW_DISTANCE;
    private Quaternion targetCameraUpRotation;
    public KBPlayer attachedPlayer;
    public Camera gameCamera;

    private void Start()
    {
        gameCamera = GetComponent<Camera>();
        gameCamera.fieldOfView = 60.0f;
        transform.rotation = Quaternion.identity;
        transform.parent = attachedPlayer.transform;
        gameCamera.isOrthoGraphic = true;
        CAMERA_FOLLOW_DISTANCE = new Vector3(0, 110, -60);
        transform.Rotate(Vector3.right, 60);
        transform.localPosition = CAMERA_FOLLOW_DISTANCE;
    }

    private void Update()
    {
        if (attachedPlayer != null)
        {
            transform.localPosition = CAMERA_FOLLOW_DISTANCE;
        }
    }
}