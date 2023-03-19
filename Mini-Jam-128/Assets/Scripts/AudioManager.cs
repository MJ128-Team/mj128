using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    // public AudioSource musicSource;
    [SerializeField] private AudioMixer mixer;

    public static float musicVolume = 1f;
    public static float sfxVolume = 1f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void OnDestroy() {
        if(instance == this)
            instance = null;
    }

    public void SetMusicLevel(float newValue)
    {
        musicVolume = Mathf.Clamp(newValue, -80.0f, 0f);
        mixer.SetFloat("MusicVol", musicVolume);
    }

    public void SetSFXLevel(float newValue)
    {
        sfxVolume = Mathf.Clamp(newValue, -80.0f, 0f);
        mixer.SetFloat("SfxVol", sfxVolume);
    }
    
}
