using UnityEngine;
using System.Collections;

public class SignAnimation : MonoBehaviour
{

    public GameObject signElement;
    private MeshRenderer[] elements;
    public float delayMin;
    public float delayMax;
    public int glitchIterationsMin;
    public int glitchIterationsMax;
    private int glitchAmount;
    private float lastGlitch;
    private float nextGlitch;
    public float iterationGapMin;
    public float iterationGapMax;
    private float[] origRed;
    private float[] origBlue;
    private float[] origGreen;
    
    private void Start()
    {
        elements = signElement.GetComponentsInChildren<MeshRenderer>();
        int size = elements.Length;
        lastGlitch = Time.time;
        CalculateNextGlitch();
        origBlue = new float[size];
        origRed = new float[size];
        origGreen = new float[size];
        for (int i = 0; i < elements.Length; i++)
        {
            origBlue[i] = elements[i].material.color.b;
            origRed[i] = elements[i].material.color.r;
            origGreen[i] = elements[i].material.color.g;
        }
    }

    private void Update()
    {
        if (Time.time > nextGlitch)
        {
            StartCoroutine(GlitchColors());
        }
    }

    private void CalculateNextGlitch()
    {
        glitchAmount = Random.Range(glitchIterationsMin, glitchIterationsMax);
        nextGlitch = lastGlitch + Random.Range(delayMin, delayMax);
    }

    private IEnumerator GlitchColors()
    {
        for (int i = 0; i < glitchAmount; i++)
        {
            for (int j = 0; j < elements.Length; j++) // Change color of all sign elements
            {
                float h, s, v;
                int r, g, b;
                Utilities.ColorToHSV(elements[j].material.color, out h, out s, out v);
                v = 0;
                Utilities.HsvToRgb(h, s, v, out r, out g, out b);
                elements[j].material.color = new Color(r / 255, g / 255, b / 255);
            }
            yield return new WaitForSeconds(Random.Range(iterationGapMin, iterationGapMax)); // wait
            for (int j = 0; j < elements.Length; j++)
            {
                elements[j].material.color = new Color(origRed[j], origGreen[j], origBlue[j]);
            }
        }

        lastGlitch = Time.time;
        CalculateNextGlitch();
    }
}