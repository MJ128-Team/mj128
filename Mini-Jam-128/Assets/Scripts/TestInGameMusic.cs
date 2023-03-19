using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TestInGameMusic : MonoBehaviour
{
    public static TestInGameMusic instance;

    public AudioSource musicSource;
    public AudioClip musicClip;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    void Start()
    {
        musicSource.clip = musicClip;
    }

    public void PlayLevelMusic()
    {
        musicSource.Play();
    }

    public void StopLevelMusic()
    {
        musicSource.Stop();
    }
    
}
