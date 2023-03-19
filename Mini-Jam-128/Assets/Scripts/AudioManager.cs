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

    [SerializeField] private List<AudioSource> sfxSources;
    [SerializeField] private AudioMixerGroup sfxBus;

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

    void OnDestroy() 
    {
        if(instance == this)
            instance = null;
    }

    void FixedUpdate() 
    {
        HandleSfxSources();
    }

    void HandleSfxSources()
    {
        if (sfxSources.Count > 1)
        {
          for (int i = 0; i < sfxSources.Count; i++)
          {
              if ( i > 0 && !sfxSources[i].isPlaying)
              { 
                  // Danger
                  Destroy(sfxSources[i]);
                  sfxSources.RemoveAt(i); 
              }
          }
        }
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

    public void TriggerSfx(AudioClip clip)
    {
        bool allBussy = true;
        foreach (AudioSource source in sfxSources)
        {
            if (!source.isPlaying)
            {
                source.PlayOneShot(clip);
                allBussy = false;
                return;
            }
        }
        if (allBussy)
        {
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            newSource.outputAudioMixerGroup = sfxBus;
            newSource.PlayOneShot(clip);
        }
    }

    
}
