using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HuiShields : MonoBehaviour
{
    private Image[] shields;

    public Sprite activeShieldImage;
    public Sprite inactiveShieldImage;


    void Start()
    {
        InGameUIManager.instance.onShieldsChanged += OnShieldsChanged;

        shields = GetComponentsInChildren<Image>();
    }

    void OnDestroy()
    {
        InGameUIManager.instance.onShieldsChanged -= OnShieldsChanged;
    }

    void OnShieldsChanged(float currentShields)
    {
        for (int i = 0; i < shields.Length; i++)
        {
            if (i < currentShields)
            {
                SetShieldState(true, shields[i]);
            }
            else
            {
                SetShieldState(false, shields[i]);
            }
        }
    }

    void SetShieldState(bool state, Image img)
    {
        if (state)
        {
            img.sprite = activeShieldImage;
        }
        else
        {
            img.sprite = inactiveShieldImage;
        }
    }
}
