using UnityEngine;
using System.Collections.Generic;

public class GUIManager : MonoBehaviour
{
    public Camera camera;
    public List<GameObject> players;
    public Texture objectOverlayTexture;

    private void Start()
    {
        players = new List<GameObject>();
        GameObject[] p = GameObject.FindGameObjectsWithTag("Player");
        players.AddRange(p);
    }
    
    void OnGUI()
    {
        //Vector3 mousePos = camera.WorldToScreenPoint(Input.mousePosition);
        //GUI.Label(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 100, 100), "Mouse");

        GUI.Box(new Rect(0, 0, 100, 50), GameManager.Instance.redBonus.ToString());
        //GUI.Box(new Rect(Screen.width - 200, 0, 200, 200), GameManager.GetCaptureZoneStateString());
        //GUI.Box(new Rect(0, Screen.height - 50, 100, 50), GameManager.GetTeamScoreString());
        //GUI.Box(new Rect(Screen.width - 100, Screen.height - 50, 100, 50), "Bottom-right");

        //Vector3 ppos = this.transform.position;
        //Vector3 p = camera.WorldToScreenPoint(ppos);
        //GUI.Label(new Rect(p.x, p.y, 100, 100), "Player");

        foreach (var i in GameManager.Instance.captureZones)
        {
            #region FOR 3RDPERSON
            //Vector3 t = i.gameObject.transform.position;
            //Vector3 a = camera.WorldToViewportPoint(t);
            //if (a.z > 0)
            //{
            //    if (a.y > 0.35f)
            //    {
            //        a.y = 0.35f;
            //    }
            //    GUI.Label(new Rect(Screen.width * a.x, Screen.height * 0.35f, 100, 50), i.tier.ToString());
            //}
            #endregion
            #region FOR ISOMETRIC

            #region CAPTURE ZONE GUI ELEMENTS
            //Vector3 t = i.gameObject.transform.position;
            //Vector3 a = camera.WorldToScreenPoint(t);
            GUI.Label(new Rect(a.x, Screen.height - a.y - 10, 100, 100), i.tier.ToString());
            #endregion
            #region PLAYERS
            //Vector3 objPos = Vector3.zero;
            //foreach (var p in players)
            //{
            //    objPos = camera.WorldToScreenPoint(p.transform.position);
            //    GUI.DrawTexture(new Rect(objPos.x - 32.0f, objPos.y, 64.0f, 64.0f), objectOverlayTexture);
            //}
            #endregion
            #endregion
        }
    }
    

    private void Update()
    {
    }
}