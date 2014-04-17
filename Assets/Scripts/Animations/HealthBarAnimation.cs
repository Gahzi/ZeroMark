using UnityEngine;
using System.Collections;

public class HealthBarAnimation : MonoBehaviour {

    public KBPlayer player;

	// Use this for initialization
	void Start () 
    {
        if (player == null)
        {
            Debug.LogWarning("Warning! Health bar doesn't have a player attached to it!");
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (player != null)
        {
            Debug.Log("Lowest Health: 0 Max Health: " + player.stats.health.ToString() + "  CurrentHealth: " + player.health.ToString() + " Inverse Lerp Value: " + Mathf.InverseLerp(0, player.stats.health, player.health).ToString() + " Lerp Value: " + Mathf.Lerp(1, 1, (float)player.health / player.stats.health).ToString());
            renderer.material.SetFloat("_Cutoff", Mathf.InverseLerp(player.stats.health, 0, player.health-1));
        }
	}
}
