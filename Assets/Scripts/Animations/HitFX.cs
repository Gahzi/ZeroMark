using UnityEngine;
using System.Collections;

public class HitFX : MonoBehaviour {

    public Vector3 targetScale;
    private Vector3 initScale;
    public float growSpeed;
    private Color transparentColor;
    public float fadeSpeed;
    public Material originalMaterial;

    void Start()
    {
        initScale = transform.localScale;
        transparentColor = new Color(originalMaterial.color.r, originalMaterial.color.g, originalMaterial.color.b, 0);
    }

    void Update()
    {
        gameObject.transform.localScale = Vector3.Lerp(transform.localScale, targetScale, growSpeed * Time.deltaTime);
        gameObject.renderer.material.color = Color.Lerp(renderer.material.color, transparentColor, fadeSpeed * Time.deltaTime);

        if (gameObject.transform.localScale.x > targetScale.x * 0.95)
        {
            Reset();
        }
    }

    public void DoEffect(int damage)
    {
        Init();
        float scale = damage / 10f;
        targetScale = Vector3.one * Mathf.Clamp(scale, 10f, 50f);
    }

    public void Init()
    {
        transform.localScale = initScale;
        renderer.material.color = originalMaterial.color;
    }

    public void Reset()
    {
        Init();
        ObjectPool.Recycle(this);
    }
}
