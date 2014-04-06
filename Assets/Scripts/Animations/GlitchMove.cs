using UnityEngine;
using System.Collections;

public class GlitchMove : MonoBehaviour
{

    public Vector3 magnitude;
    public Vector3 magnitudeMin;
    private Vector3 originalPos;
    public float delayMin;
    public float delayMax;
    public int glitchIterationsMin;
    public int glitchIterationsMax;
    private int glitchAmount;
    private float lastGlitch;
    private float nextGlitch;
    public float iterationGapMin;
    public float iterationGapMax;
    
    private void Start()
    {
        originalPos = transform.position;
        lastGlitch = Time.time;
        CalculateNextGlitch();
    }

    private void Update()
    {
        if (Time.time > nextGlitch)
        {
            StartCoroutine(Glitch());
        }
    }

    private IEnumerator Glitch()
    {
        for (int i = 0; i < glitchAmount; i++)
        {
            transform.Translate(Random.Range(-magnitude.x - magnitudeMin.x, magnitude.x + magnitudeMin.x), Random.Range(-magnitude.y - magnitudeMin.y, magnitude.y + magnitudeMin.y), Random.Range(-magnitude.z - magnitudeMin.z, magnitude.z + magnitudeMin.z));
            yield return new WaitForSeconds(Random.Range(iterationGapMin, iterationGapMax));
            transform.position = originalPos;
        }
        lastGlitch = Time.time;
        transform.position = originalPos;
        CalculateNextGlitch();
    }

    private void CalculateNextGlitch()
    {
        glitchAmount = Random.Range(glitchIterationsMin, glitchIterationsMax);
        nextGlitch = lastGlitch + Random.Range(delayMin, delayMax);
    }
}