using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource hallwayMusic, stairRoomMusic, tvStaticSfx, birdsSfx, radioSfx, metalClunkSfx, orbitalMusic;
    public Dictionary<string, AudioSource> audioSources = new Dictionary<string, AudioSource>();
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        audioSources.Add("hallwayMusic", hallwayMusic);
        audioSources.Add("stairRoomMusic", stairRoomMusic);
        audioSources.Add("tvStaticSfx", tvStaticSfx);
        audioSources.Add("birdsSfx", birdsSfx);
        audioSources.Add("radioSfx", radioSfx);
        audioSources.Add("metalClunkSfx", metalClunkSfx);
        audioSources.Add("orbitalMusic", orbitalMusic);
        StartCoroutine(AudioFadeIn(hallwayMusic, 2f));
        PlaySound(audioSources["hallwayMusic"]);
        PlaySound(audioSources["birdsSfx"]);
        PlaySound(audioSources["radioSfx"]);
        
    }

    public void PlaySound(AudioSource audio)
    {
        //audio.volume = (GameData.sfxVolume / 100);
        audio.Play();
    }

    public IEnumerator AudioFadeIn(AudioSource audio, float duration)
    {
        audio.UnPause();
        audio.volume = 0f;
        while (audio.volume < 1f)
        {
            audio.volume += Time.deltaTime / duration;
            yield return null;
        }
    }   

    public IEnumerator AudioFadeOut(AudioSource audio, float duration)
    {
        while (audio.volume > 0f)
        {
            audio.volume -= Time.deltaTime / duration;
            yield return null;
        }
        audio.volume = 0;
        audio.Pause();
    }
}
