using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderVolume : MonoBehaviour
{
    public void SetMusicLevel(float newValue)
    {
        AudioManager.instance.SetMusicLevel(newValue);
    }

    public void SetSFXLevel(float newValue)
    {
        AudioManager.instance.SetSFXLevel(newValue);
    }
}
