using UnityEngine;

public class KBCamera : MonoBehaviour
{
    private Vector3 CAMERA_FOLLOW_DISTANCE;
    private Quaternion targetCameraUpRotation;
    public KBPlayer attachedPlayer;
    public Camera gameCamera;
    private float zoom;
    public float zoomTarget;
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
        zoomTarget = 1.4f;
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
            CAMERA_FOLLOW_DISTANCE = new Vector3(0, 22 * zoom, -2 * zoom);
            transform.localPosition = Vector3.Lerp(transform.localPosition, CAMERA_FOLLOW_DISTANCE, 5.0f * Time.deltaTime);

            #region CameraShiftingTowardLookDirection

            float x, y;
            if (attachedPlayer.useController)
            {
                x = attachedPlayer.gamepadState.ThumbSticks.Right.X;
                y = attachedPlayer.gamepadState.ThumbSticks.Right.Y;
            }
            else
            {
                x = -attachedPlayer.mousePlayerDiff.x;
                y = -attachedPlayer.mousePlayerDiff.y;
            }

            Vector3 lookDir = new Vector3(x, 0, y).normalized;
            float moveMagnitude = 1.5f;
            if (attachedPlayer.useController && y < 0)
            {
                moveMagnitude = 2.5f;
            }
            else if (y > 0)
            {
                moveMagnitude = 2.5f;
            }
            transform.position = Vector3.Lerp(transform.position, transform.position + lookDir * moveMagnitude, 5.0f * Time.deltaTime);

            #endregion CameraShiftingTowardLookDirection

            #region temporary level text

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

            #endregion temporary level text
        }

        #region DamageSplashVignette

        Color c = Color.white;
        float percentHealth = (float)attachedPlayer.health / (float)attachedPlayer.stats.health;
        c.a = 1.0f - percentHealth;
        damageVignette.color = c;

        #endregion DamageSplashVignette
    }
}