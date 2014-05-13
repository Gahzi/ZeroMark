using UnityEngine;

public class MaterialFadeOut : MonoBehaviour
{

    private Color initColor;
    private Color transparentColor;
    public float fadeSpeed;
    
    private void Start()
    {
        initColor = gameObject.renderer.material.color;
        transparentColor = new Color(initColor.r, initColor.g, initColor.b, 0);
    }

    private void FixedUpdate()
    {
        gameObject.renderer.material.color = Color.Lerp(gameObject.renderer.material.color, transparentColor, fadeSpeed * Time.deltaTime);
    }

    public void Init()
    {
        renderer.material.color = initColor;
    }

    public void Init(Color color)
    {

    }
}