using UnityEngine;

public class Screenshot : MonoBehaviour
{

    public int superSize;

    private void Update()
    {
        if (Input.GetButtonDown("Screenshot"))
        {
            GameManager.TakeScreenshot(superSize);
        }
    }

}