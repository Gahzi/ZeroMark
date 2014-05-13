using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public AudioClip bgm;
    public AudioClip[] clips;
    public AudioClip dataPulseStage1;
    public AudioClip dataPulseStage2;
    public bool playingDataPulse;

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
                instance = FindObjectOfType<AudioManager>();
                return instance;
            }
        }
    }

    void Start()
    {
        //audio.clip = bgm;
        //audio.playOnAwake = true;
        //audio.volume = 0.35f;
        playingDataPulse = false;
    }

    void Update()
    {

    }

    private IEnumerator PlayDataPulseSFX()
    {
        playingDataPulse = true;
        audio.PlayOneShot(dataPulseStage1);
        yield return new WaitForSeconds(10.0f);
        audio.PlayOneShot(dataPulseStage2);
    }

    public void StartDataPulseSFX()
    {
        if (!playingDataPulse)
        {
            StartCoroutine(PlayDataPulseSFX());
        }
    }


}
