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
    private bool isPlayingMusic = false;
    private bool isSwitchingMusic = false;
    private bool isStoppingMusic = false;
    public static float musicVolume = 1f;
    public static float sfxVolume = 1f;

    [SerializeField] private List<AudioSource> musicSources;
    [SerializeField] private AudioMixerGroup musicBus;
    [SerializeField]private AudioSource loopCurrent;
    [SerializeField]private AudioSource loopIn;
    [SerializeField]private AudioSource loopOut;
    private double nextMusicTime = 0;

    [SerializeField] private List<AudioSource> sfxSources;
    [SerializeField] private AudioMixerGroup sfxBus;

    [SerializeField] private List<AudioClip> musicMenuClips;
    private int gameMusicLevel = 0;
    public List<AudioClip> musicGameClips;

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

    void HandleMusicSources()
    {
        if (musicSources.Count > 1)
        {
          for (int i = 0; i < musicSources.Count; i++)
          {
              if ( i > 0 && !musicSources[i].isPlaying)
              { 
                  // Danger
                  Destroy(musicSources[i]);
                  musicSources.RemoveAt(i); 
              }
          }
        }
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

    void HandleMusicLoop()
    {
        if(!isStoppingMusic)
        {
            if(isPlayingMusic)
            {
                if(AudioSettings.dspTime > nextMusicTime - 0.5)
                {
                    // Update the next start time if loop
                    double duration = (double)loopCurrent.clip.samples / loopCurrent.clip.frequency;
                    nextMusicTime = nextMusicTime + duration;
                }
            } 
            else if (isSwitchingMusic)
            {   
                // Crossfade
                if (loopOut != null)
                {
                    if(loopIn.isPlaying)
                        FadeSourceOut(loopOut);
                }

                if (loopIn != null)
                {
                    FadeSourceIn(loopIn);
                }
                else
                {
                    isSwitchingMusic = false;
                    isPlayingMusic = true;
                }

                if(AudioSettings.dspTime > nextMusicTime - 0.5)
                {
                    // Update the next start time if loop
                    double duration = (double)loopCurrent.clip.samples / loopCurrent.clip.frequency;
                    nextMusicTime = nextMusicTime + duration;
                }
                // Checks how long the Clip will last and updates the Next Start Time with a new value
                // double duration = (double)clipToPlay.samples / clipToPlay.frequency;
                // nextMusicTime = nextMusicTime + duration;
            }
        }
        else
        {
          if(isPlayingMusic)
          {
            if (loopCurrent != null)
            {
                FadeSourceOut(loopCurrent);
            } 
            else
            {
                isStoppingMusic = false;
                isPlayingMusic = false;
                nextMusicTime = 0; // Cant be null
            }
          }
          else if (isSwitchingMusic)
          {
            if (loopOut != null)
            {
                FadeSourceOut(loopOut);
            }

            if (loopIn != null)
            {
                FadeSourceOut(loopIn);
            }

            if (loopOut == null && loopIn == null)
            {
                isStoppingMusic = false;
                isSwitchingMusic = false;
                isPlayingMusic = false;
                nextMusicTime = 0; // Cant be null
            }
          }
        }
    }

    void FadeSourceOut( AudioSource source )
    {
        source.volume = Mathf.Lerp(source.volume, 0f, 0.1f);
        if (source.volume <= 0.01f)
        {
            source.Stop();
            source = null;
        }
    }

    void FadeSourceIn( AudioSource source )
    {
        source.volume = Mathf.Lerp(source.volume, 1f, 0.1f);
        if (source.volume >= 0.99f)
        {
            source.volume = 1f;
            loopCurrent = source;
            source = null;
            // isSwitchingMusic = false;
            // isPlayingMusic = true;
        }
    }

    // =======================

    public void ChangeMusicLoop(AudioClip clip)
    {
        Debug.Log("ChangeMusicLoop: " + clip.name);
        loopOut = loopCurrent;
        isPlayingMusic = false;
        isSwitchingMusic = true;

        bool allBussy = true;        

        foreach (AudioSource source in musicSources)
        {
            if (!source.isPlaying)
            {
                loopIn = source;
                allBussy = false;
                return;
            }
        }
        if (allBussy)
        {
            loopIn = gameObject.AddComponent<AudioSource>();
            loopIn.outputAudioMixerGroup = musicBus;
        }

        loopIn.clip = clip;
        loopIn.loop = true;

        if(nextMusicTime == 0) // First time
        {
            nextMusicTime = AudioSettings.dspTime + 0.2f;
        }
        loopIn.PlayScheduled(nextMusicTime);

        // Checks how long the Clip will last and updates the Next Start Time with a new value
        double duration = (double)clip.samples / clip.frequency;
        nextMusicTime = nextMusicTime + duration;

        // Crossfade

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

    public void PlayMusic()
    {
        isStoppingMusic = false;
        if (!isPlayingMusic)
        {
            isPlayingMusic = true;
            loopCurrent = musicSources[0];
            ChangeMusicLoop(musicGameClips[gameMusicLevel]);
        }
    }

    public void StopMusic()
    {
        isStoppingMusic = true;
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

    public void NextGameMusic()
    {
        if (gameMusicLevel < musicGameClips.Count)
        {
            gameMusicLevel++;
        }
        ChangeMusicLoop(musicGameClips[gameMusicLevel]);
    }

    void ResetGameMusic()
    {
        gameMusicLevel = 0;
        ChangeMusicLoop(musicGameClips[gameMusicLevel]);
    }

    
}
