using UnityEngine;
using System.Collections;

public class FadeOut : MonoBehaviour
{

    private GUITexture guiText;
    public bool fading = false;
    private float fadeTime = 0;
    private Color c;

    // Use this for initialization
    void Start()
    {
        guiText = GetComponent<GUITexture>();
        reset();
    }

    // Update is called once per frame
    void Update()
    {
        switch (fading)
        {
            case true:
                //fadeColor(guiText.color, Color.clear, 10.0f);
                guiText.color = Color.Lerp(guiText.color, Color.clear, 0.1f);
                break;
            case false:
                break;

            default:
                break;
        }
    }

    public void startFading(float fadeTimeSec)
    {
        fading = true;
        this.fadeTime = fadeTimeSec;
    }

    public void pauseFading()
    {
        fading = false;
    }

    public void reset()
    {
        c = guiText.color;
    }

    public IEnumerator fadeColor(Color startColor, Color endColor, float time)
    {
        float i = 0.0f;
        float rate = 1.0f / time;
        while (i < 1.0)
        {
            i += Time.deltaTime * rate;
            guiText.color = Color.Lerp(startColor, endColor, i);
            yield break;
        }
    }
}
