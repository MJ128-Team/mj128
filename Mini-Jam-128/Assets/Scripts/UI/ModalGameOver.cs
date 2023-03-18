using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModalGameOver : MonoBehaviour
{
    void Start()
    {
        InGameUIManager.instance.onGameOver += OnGameOver;
    }

    void OnDestroy()
    {
        InGameUIManager.instance.onGameOver -= OnGameOver;
    }

    void OnGameOver()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
