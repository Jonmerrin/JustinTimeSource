using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public static MusicController instance;
    public AudioSource bgm;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
        bgm = GetComponent<AudioSource>();
    }

    public void PlayMusic()
    {
        if(!bgm.isPlaying)
        {
            bgm.Play();
        }
    }

    public void StopMusic()
    {
        bgm.Stop();
    }
}
