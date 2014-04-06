using UnityEngine;

public class Screenshot : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetButtonDown("Screenshot"))
        {
            GameManager.TakeScreenshot();
        }
    }

}