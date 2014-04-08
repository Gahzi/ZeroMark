using UnityEngine;

public class MenuButton : MonoBehaviour
{

    public MainMenuScript menu;
    public string MenuMethodToCall;
    private bool clicked = false;
    
    private void OnMouseDown()
    {
        if (!clicked)
        {
            menu.SendMessage(MenuMethodToCall);
        }
        else
        {
            menu.SendMessage("CloseAllWindows");
        }
        clicked = !clicked;
    }
}