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
            renderer.material.SetFloat("_Cutoff", Mathf.InverseLerp(player.stats.health, 0, player.health-1));
        }
	}
}
