using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TriggerSfx : MonoBehaviour
{
    [SerializeField] private AudioClip sfxClip;

    public void PlaySfx()
    {
        AudioManager.instance.TriggerSfx(sfxClip);
    }
}
