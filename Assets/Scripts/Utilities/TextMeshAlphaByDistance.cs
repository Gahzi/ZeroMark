using UnityEngine;

public class TextMeshAlphaByDistance : MonoBehaviour
{

    public TextMesh text;
    public Color originalColor;

    private void Start()
    {
        text = GetComponent<TextMesh>();
        originalColor = text.color;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, Camera.main.transform.position) < 10)
        {
            text.color = originalColor;
        }
        else
        {
            text.color = Color.clear;
        }
    }
}