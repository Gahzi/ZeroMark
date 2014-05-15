using UnityEngine;
using System;

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
    public TextMesh redScoreText;
    public TextMesh blueScoreText;
    public TextMesh timeRemainingNumberText;
    public TextMesh gameTypeText;
    public TextMesh redHeldPointTotalText;
    public TextMesh blueHeldPointTotalText;
    public TextMesh dataPulseCountdown;
    public TextMesh nameText;
    public TextMesh levelNumText;
    public TextMesh ammoText;
    public TextMesh typeText;
    public TextMesh tieText;
    public TextMesh redWinsText;
    public TextMesh blueWinsText;
    public TextMesh pregameText;
    public GameObject quitButton;
    public GameObject dataPulse;
    public GameObject redScoreBar;
    public GameObject blueScoreBar;
    public TextMesh pointsText;

    private void Start()
    {
        gameCamera = GetComponent<Camera>();
        gameCamera.fieldOfView = 90.0f;
        transform.parent = attachedPlayer.transform;
        transform.rotation = Quaternion.Euler(new Vector3(rotation, 0, 0));
        transform.localPosition = CAMERA_FOLLOW_DISTANCE;
        nameText.text = attachedPlayer.gameObject.name;
    }

    private void Update()
    {
        gameTypeText.text = Enum.GetName(typeof(GameManager.GameType), GameManager.Instance.gameType);

        zoomTarget = 2.0f;
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

            int total = GameManager.Instance.redTeamScore + GameManager.Instance.blueTeamScore + 10;
            redScoreBar.renderer.material.SetFloat("_Cutoff", Mathf.InverseLerp(total, 0, GameManager.Instance.redTeamScore));
            blueScoreBar.renderer.material.SetFloat("_Cutoff", Mathf.InverseLerp(total, 0, GameManager.Instance.blueTeamScore));

            if (GameManager.Instance.gameTime > GameManager.Instance.lastDataPulse + KBConstants.GameConstants.dataPulsePeriod - 10)
            {
                dataPulse.SetActive(true);
                //((Mathf.Sin(Time.time * speed) + 0.5f) * (lightMaxIntensity - lightMinIntensity)) + lightMinIntensity
                float speed = 3.50f;
                float min = 0.1f;
                float max = 1.0f;
                float alpha = ((Mathf.Sin(GameManager.Instance.gameTime * speed) + 0.5f) * (max - min)) + min;
                dataPulse.GetComponent<TextMesh>().color = new Color(1.0f, 0, 1.0f, alpha);
            }
            int level = attachedPlayer.currentLevel + 1;
            levelNumText.text = level.ToString("0");
            pointsText.text = attachedPlayer.currentPoints.ToString();
            if (attachedPlayer.guns.Length > 0)
            {
                ammoText.text = attachedPlayer.guns[0].ammo.ToString("00");
            }

            if (GameManager.Instance.State == GameManager.GameState.PreGame)
            {
                if (pregameText.gameObject.activeInHierarchy != true)
                {
                    pregameText.gameObject.SetActive(true);
                }

                //((Mathf.Sin(Time.time * speed) + 0.5f) * (lightMaxIntensity - lightMinIntensity)) + lightMinIntensity
                float speed = 3.50f;
                float min = 0.1f;
                float max = 1.0f;
                float alpha = ((Mathf.Sin(Time.time * speed) + 0.5f) * (max - min)) + min;
                pregameText.GetComponent<TextMesh>().color = new Color(1.0f, 0, 1.0f, alpha);
            }
            else
            {
                if (pregameText.gameObject.activeInHierarchy == true)
                {
                    pregameText.gameObject.SetActive(false);
                }
            }

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

            //if (attachedPlayer.guns.Length > 0)
            //{
            //    if (attachedPlayer.guns[0] != null)
            //    {
            //        int l = attachedPlayer.guns[0].level + 1;   
            //        levelText.text = "(" + attachedPlayer.currentPoints.ToString() + "pts.)" + "Lvl." + l.ToString();
            //    }
            //    else
            //    {
            //        levelText.text = "";
            //    }
            //}

            #endregion temporary level text
        }

        #region DamageSplashVignette

        Color c = Color.white;
        float percentHealth = (float)attachedPlayer.health / (float)attachedPlayer.stats.health;
        c.a = 1.0f - percentHealth;
        damageVignette.color = c;

        #endregion DamageSplashVignette

        #region WinLoseTextAndButtonDisplay

        //if (GameManager.Instance.State.Equals(GameManager.GameState.RedWins))
        //{
        //    if(redWinsText.gameObject.activeInHierarchy == false)
        //    {
        //        redWinsText.gameObject.SetActive(true);
        //    }
        //}
        //else if (GameManager.Instance.State.Equals(GameManager.GameState.BlueWins))
        //{
        //    if (blueWinsText.gameObject.activeInHierarchy == false)
        //    {
        //        blueWinsText.gameObject.SetActive(true);

        //    }
        //}

        #endregion WinLoseTextAndButtonDisplay
    }

    private void QuitGame()
    {
        PhotonNetwork.LeaveRoom();
        Application.LoadLevel(0);
    }

}