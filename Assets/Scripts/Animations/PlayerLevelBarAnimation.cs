using UnityEngine;

public class PlayerLevelBarAnimation : MonoBehaviour
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
            switch (player.currentLevel)
            {
                case 0:
                    renderer.material.SetFloat("_Cutoff", Mathf.InverseLerp(KBConstants.GameConstants.levelOneThreshold, 0, player.currentPoints));
                    break;

                case 1:
                    renderer.material.SetFloat("_Cutoff", Mathf.InverseLerp(KBConstants.GameConstants.levelTwoThreshold, 0, player.currentPoints));
                    break;

                case 2:
                    renderer.material.SetFloat("_Cutoff", 255.0f);
                    break;

                default:
                    break;
            }
        }
    }
}