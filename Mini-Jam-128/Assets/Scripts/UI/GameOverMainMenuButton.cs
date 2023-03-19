using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMainMenuButton : MonoBehaviour
{
    public void ToMainMenu()
    {
        SceneManager.instance.LoadMainMenu();
    }
}
