using UnityEngine;
using System.Collections;

public class FadeOut : MonoBehaviour
{
    private GUITexture a;
    private float fadeTime = 0;
    private bool triggerFade;

    void Start()
    {
        a = GetComponent<GUITexture>();
        triggerFade = false;
    }

    void Update()
    {
        if (triggerFade)
        {
            StartCoroutine(fadeColor(a.color, Color.clear, fadeTime));
            triggerFade = false;
        }
    }

    public void startFading(float fadeTimeSec)
    {
        triggerFade = true;
        fadeTime = fadeTimeSec;
    }

    public IEnumerator fadeColor(Color startColor, Color endColor, float time)
    {
        float i = 0.0f;
        float rate = 1.0f / time;
        while (i < 1.0)
        {
            i += Time.deltaTime * rate;
            a.color = Color.Lerp(startColor, endColor, i);
            yield break;
        }
    }
}
