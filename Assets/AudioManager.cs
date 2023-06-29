using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioClip[] clips;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);

        initializeDict();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void PlayBattle()
    {
        PlayTrackWithIndex(0);
    }
    public void PlayShop()
    {
        PlayTrackWithIndex(1);
    }

    public void PlayMenu()
    {
        PlayTrackWithIndex(2);
    }

    public void PlayTrackWithIndex(int index)
    {
        StopTracks();
        gameObject.GetComponents<AudioSource>()[index].Play();
        gameObject.GetComponents<AudioSource>()[index].loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void initializeDict()
    {
        foreach (AudioClip clip in clips)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = clip;
            source.volume = 0.5f;
        }
    }

    void StopTracks()
    {
        foreach (AudioSource source in gameObject.GetComponents<AudioSource>())
        {
            source.Stop();
        }
    }
}
