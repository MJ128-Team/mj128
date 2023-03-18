using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InGameUIManager : MonoBehaviour
{
    public static InGameUIManager instance;

    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void OnDestroy() {
        if (instance == this) {
            instance = null;
        }
    }

    public event Action<float> onShieldsChanged;
    public void OnShieldsChanged(float currentShields)
    {
      if (onShieldsChanged != null)
      {
        onShieldsChanged(currentShields);
      }
    }

    public event Action<float> onFuelChanged;
    public void OnFuelChanged(float currentFuel)
    {
      if (onFuelChanged != null)
      {
        onFuelChanged(currentFuel);
      }
    }

    public event Action onGameOver;
    public void OnGameOver()
    {
      if (onGameOver != null)
      {
        onGameOver();
      }
    }
}
