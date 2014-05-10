using UnityEngine;

public class CurrentDateTimeText : MonoBehaviour
{

    public TextMesh text;
    
    private void Start()
    {
        text.text =
            "development build v0.9." +
            System.DateTime.Now.Month.ToString("00") + 
            System.DateTime.Now.Day.ToString("00") +
            "_" +
            System.DateTime.Now.Hour.ToString("00") +
            System.DateTime.Now.Minute.ToString("00");
    }



    private void Update()
    {
    }
}