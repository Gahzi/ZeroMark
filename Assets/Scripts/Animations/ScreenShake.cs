using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    Vector3 target;
    Vector3 originalPosition;
    Transform camPos;
    Camera camera;
    public bool shake;
    private float startTime;
    private float activeTime;
    private float intensity;
    
    private void Start()
    {
        camera = FindObjectOfType<Camera>();
        originalPosition = camera.gameObject.transform.position;
        camPos = camera.gameObject.transform;
        target = originalPosition;
        shake = false;
        StopShake();
    }

    private void Update()
    {
        if (shake)
        {
            if (Time.time > startTime + activeTime)
            {
                StopShake();
            }
            else
            {
                Shake();
            }
        }
    }

    public void Shake()
    {
        target = new Vector3(camera.transform.position.x + Random.Range(-intensity, intensity), camera.transform.position.y + Random.Range(-intensity, intensity), camera.transform.position.z);
        camPos.position = Vector3.Lerp(camera.transform.position, target, 10 * Time.deltaTime);
    }

    public void StartShake(float time, float _intensity)
    {
        shake = true;
        activeTime = time;
        startTime = Time.time;
        intensity = _intensity;
    }

    public void StopShake()
    {
        //ResetPosition();
        shake = false;
    }

    public void ResetPosition()
    {
        target = originalPosition;
    }
}