using UnityEngine;

public class MenuButton : MonoBehaviour
{

    public MainMenuScript menu;
    public bool callMethod;
    public string MenuMethodToCall;
    private bool clicked = false;
    public bool moveCamera;
    public Transform cameraTarget;
    public float fovTarget;
    
    private void OnMouseDown()
    {
        if (!clicked)
        {
            if (callMethod)
            {
                menu.SendMessage(MenuMethodToCall);
            }
            if (moveCamera)
            {
                Camera.main.GetComponent<MoveCameraWithMouse>().SetAnchor(cameraTarget, fovTarget);
            }
        }
        else
        {
            menu.SendMessage("CloseAllWindows");
        }
        clicked = !clicked;
    }
}