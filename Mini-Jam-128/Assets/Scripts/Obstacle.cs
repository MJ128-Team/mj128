using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private bool isDeadly = false;

    public void OnCrash()
    {
        Debug.Log("CRASH");
        if (isDeadly)
        {
            InGameManager.instance.InstaDeath();
            // Trigger Animation/Particle Effect
            // Trigger Sound Effect       
        }
        else
        {
            InGameManager.instance.DecreaseShields();   
            // Trigger Animation/Particle Effect
            // Trigger Sound Effect 
            Destroy(gameObject);
        }
        
        
    }
}
