using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuiFuel : MonoBehaviour
{
    private RectTransform fillBar;

    void Start()
    {
        fillBar = transform.GetChild(0).GetComponent<RectTransform>();
        fillBar.localScale = Vector3.one;

        InGameUIManager.instance.onFuelChanged += OnFuelChanged;
    }

    void OnDestroy()
    {
        InGameUIManager.instance.onFuelChanged -= OnFuelChanged;
    }

    void OnFuelChanged(float currentFuel)
    {
        fillBar.localScale = new Vector3(currentFuel, 1.0f, 1.0f);
    }
}
