using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private string kind; // TODO: Enum
    [SerializeField] private float value = 10.0f; // in seconds

    public void OnCollect()
    {
        if(kind == "fuel")
        {
          InGameManager.instance.IncreasePower(value);
          // Efecto de sonido
          // Efecto de particulas
          Destroy(gameObject);
        }
    }
}
