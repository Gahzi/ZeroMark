using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public AudioClip bgm;
    public AudioClip[] clips;

    private static AudioManager instance;

    public static AudioManager Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }
            else
            {
                instance = new GameObject("Game Manager").AddComponent<AudioManager>();
                return instance;
            }
        }
    }

    void Start()
    {
        //audio.clip = bgm;
        //audio.playOnAwake = true;
        //audio.volume = 0.35f;
    }

    void Update()
    {

    }
}
