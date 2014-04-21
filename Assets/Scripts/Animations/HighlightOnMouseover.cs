using UnityEngine;

public class HighlightOnMouseover : MonoBehaviour
{
    public Color highlightColor;
    private Color originalColor;
    public Color pressColor;
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

    void OnMouseDown()
    {
        text.color = pressColor;
    }

    void OnMouseUp()
    {
        text.color = highlightColor;
    }



}