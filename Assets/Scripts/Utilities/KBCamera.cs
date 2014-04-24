using UnityEngine;

public class KBCamera : MonoBehaviour
{
    private Vector3 CAMERA_FOLLOW_DISTANCE;
    private Quaternion targetCameraUpRotation;
    public KBPlayer attachedPlayer;
    public Camera gameCamera;
    private float zoom = 1.8f;
    public float zoomTarget = 1.8f;
    public float rotation;
    public SpriteRenderer damageVignette;
    public TextMesh levelText;

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
        zoomTarget = 1.6f;
        if (attachedPlayer != null)
        {
            if (zoom < 1.0f)
            {
                //rotation = 65.0f * zoom;
            }
            else
            {
                rotation = 80.0f;
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(rotation, 0, 0)), 1.0f * Time.deltaTime);
            zoom = Mathf.Lerp(zoom, zoomTarget, 1.0f * Time.deltaTime);
            //CAMERA_FOLLOW_DISTANCE = new Vector3(0, 22 * zoom, -12 * zoom); old
            Vector3 lookDir = new Vector3(-attachedPlayer.mousePlayerDiff.x, 0, -attachedPlayer.mousePlayerDiff.y).normalized;
            CAMERA_FOLLOW_DISTANCE = new Vector3(0, 22 * zoom, -2 * zoom);
            transform.localPosition = Vector3.Lerp(transform.localPosition, CAMERA_FOLLOW_DISTANCE, 5.0f * Time.deltaTime);
            float moveMagnitude = 3.0f;
            if (attachedPlayer.mousePlayerDiff.y > 0)
            {
                moveMagnitude = 7.0f;
            }
            transform.position = Vector3.Lerp(transform.position, transform.position + lookDir * moveMagnitude, 5.0f * Time.deltaTime);
            if (attachedPlayer.guns.Length > 0)
            {
                if (attachedPlayer.guns[0] != null)
                {
                    levelText.text = "(" + attachedPlayer.killTokens.ToString() + "pts.)" + "Lvl." + attachedPlayer.guns[0].level.ToString();
                }
                else
                {
                    levelText.text = "";
                }
            }

        }
        Color c = Color.white;
        float percentHealth = (float)attachedPlayer.health / (float)attachedPlayer.stats.health;
        c.a = 1.0f - percentHealth;
        damageVignette.color = c;


    }
}