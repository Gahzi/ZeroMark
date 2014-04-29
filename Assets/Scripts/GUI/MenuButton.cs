using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MenuButton : MonoBehaviour
{

    public MainMenuScript menu;
    public bool callMethod;
    public string MenuMethodToCall;
    private bool clicked = false;
    public bool moveCamera;
    public Transform cameraTarget;
    public float fovTarget;
    public AudioClip pressClip;
    
    private void OnMouseDown()
    {
        if (!clicked)
        {
            audio.PlayOneShot(pressClip);
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