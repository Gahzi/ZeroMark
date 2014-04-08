using UnityEngine;

public class MoveCameraWithMouse : MonoBehaviour
{

    public Vector3 moveAmount;
    private Vector3 targetPos;
    private Quaternion targetRotation;
    public Transform anchorTrans;
    public Transform startTransform;
    public float targetFov;
    private float startFov;

    private void Start()
    {
        anchorTrans = startTransform;
        targetFov = Camera.main.fieldOfView;
        startFov = targetFov;
    }

    private void Update()
    {
        targetPos = new Vector3(anchorTrans.position.x + (moveAmount.x * (Input.mousePosition.x - (Screen.currentResolution.width / 2)) / Screen.currentResolution.width), anchorTrans.position.y + (moveAmount.y * (Input.mousePosition.y - (Screen.currentResolution.height / 2)) / Screen.currentResolution.height), anchorTrans.position.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, anchorTrans.rotation, 0.5f * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, targetPos, 0.5f * Time.deltaTime);
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, targetFov, 0.5f * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            transform.rotation = anchorTrans.rotation;
            transform.position = targetPos;
            Camera.main.fieldOfView = targetFov;
        }
    }

    public void ResetCamera()
    {
        anchorTrans = startTransform;
        targetFov = startFov;
    }

    public void SetAnchor(Transform _anchor, float fov)
    {
        anchorTrans = _anchor;
        targetFov = fov;
    }
}
