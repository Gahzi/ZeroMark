using UnityEngine;

public class KBCamera : MonoBehaviour
{
    private Vector3 CAMERA_FOLLOW_DISTANCE;
    private Quaternion targetCameraUpRotation;
    public KBPlayer attachedPlayer;
    public Camera gameCamera;
    private float zoom = 1.0f;
    public float zoomTarget = 1.0f;
    public float rotation;
    public SpriteRenderer damageVignette;

    private void Start()
    {
        gameCamera = GetComponent<Camera>();
        gameCamera.fieldOfView = 90.0f;
        transform.parent = attachedPlayer.transform;
        transform.rotation = Quaternion.Euler(new Vector3(rotation, 0, 0));
        transform.localPosition = CAMERA_FOLLOW_DISTANCE;
    }

    private void Update()
    {
        if (attachedPlayer != null)
        {
            if (zoom < 1.0f)
            {
                //rotation = 65.0f * zoom;
            }
            else
            {
                rotation = 65.0f;
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(rotation, 0, 0)), 1.0f * Time.deltaTime);
            zoom = Mathf.Lerp(zoom, zoomTarget, 1.0f * Time.deltaTime);
            CAMERA_FOLLOW_DISTANCE = new Vector3(0, 22 * zoom, -12 * zoom);
            transform.localPosition = Vector3.Lerp(transform.localPosition, CAMERA_FOLLOW_DISTANCE, 5.0f * Time.deltaTime);
        }
        Color c = Color.white;
        float percentHealth = (float)attachedPlayer.health / (float)attachedPlayer.stats.health;
        c.a = 1.0f - percentHealth;
        damageVignette.color = c;
    }
}