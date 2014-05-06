using UnityEngine;

public class PlayerKillGUI : MonoBehaviour
{

    public TextMesh kills;
    private KBPlayer player;
    
    private void Start()
    {
        player = transform.parent.GetComponent<KBPlayer>();
    }

    private void Update()
    {
        //transform.rotation = player.upperBody.transform.rotation;
        kills.text = player.currentPoints.ToString();
    }
}