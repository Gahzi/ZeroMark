using UnityEngine;

public class HighlightOnMouseover : MonoBehaviour
{
    public Color highlightColor;
    private Color originalColor;
    public TextMesh text;

    public void Start()
    {
        //text = GetComponent<TextMesh>();
        originalColor = text.color;
    }

    void OnMouseEnter()
    {        
        text.color = highlightColor;
    }

    void OnMouseExit()
    {
        text.color = originalColor;
    }

}