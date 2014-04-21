using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public AudioClip bgm;
    public AudioClip[] clips;

    void Start()
    {
        audio.clip = bgm;
        audio.playOnAwake = true;
        //audio.volume = 0.35f;
    }

    void Update()
    {

    }
}
