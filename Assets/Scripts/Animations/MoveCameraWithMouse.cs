using UnityEngine;

public class MoveCameraWithMouse : MonoBehaviour
{

    public Vector3 moveAmount;
    private Vector3 originalPos;

    private void Start()
    {
        originalPos = transform.position;
    }

    private void Update()
    {
        transform.position = new Vector3(originalPos.x + (moveAmount.x * (Input.mousePosition.x - (Screen.currentResolution.width / 2)) / Screen.currentResolution.width), originalPos.y + (moveAmount.y * (Input.mousePosition.y - (Screen.currentResolution.height / 2)) / Screen.currentResolution.height), transform.position.z);
    }
}