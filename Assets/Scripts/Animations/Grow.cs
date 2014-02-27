using UnityEngine;

public class Grow : MonoBehaviour
{
    private Vector3 initScale;
    public Vector3 targetScale;
    public float growSpeed;

    private void Start()
    {
        initScale = gameObject.transform.localScale;
    }

    private void FixedUpdate()
    {
        gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, targetScale, growSpeed * Time.deltaTime);

        if (gameObject.transform.localScale.x > targetScale.x * 0.95)
        {
            Destroy(gameObject);
        }
    }
}