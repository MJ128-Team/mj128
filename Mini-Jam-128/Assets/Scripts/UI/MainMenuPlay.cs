using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPlay : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.instance.LoadGame();
    }
    
}
