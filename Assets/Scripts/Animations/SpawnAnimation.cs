using UnityEngine;

public class SpawnAnimation : MonoBehaviour
{

    public float speed;
    private Vector3 originalPos;
    private bool animating;
    public TrailRenderer trail;
    
    private void Start()
    {
        animating = false;
        originalPos = transform.localPosition;
    }

    private void Update()
    {
        if (animating)
        {
            transform.position = new Vector3(transform.parent.transform.position.x, transform.position.y, transform.parent.transform.position.z);
            transform.Translate(new Vector3(0, speed, 0));
        }
    }

    public void Activate()
    {
        animating = true;
        trail.time = 1.5f;
        trail.enabled = true;
    }

    public void Reset()
    {
        transform.localPosition = originalPos;
        animating = false;
        trail.time = -1.0f;
        trail.enabled = false;
    }
}