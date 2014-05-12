using UnityEngine;

public class HealthBarAnimation : MonoBehaviour
{
    public KBPlayer player;

    private void Start()
    {
        if (player == null)
        {
            Debug.LogWarning("Warning! Health bar doesn't have a player attached to it!");
        }
    }

    private void Update()
    {
        if (player != null)
        {
            renderer.material.SetFloat("_Cutoff", Mathf.InverseLerp(player.stats.health, 0, player.health - 1));
        }
    }
}