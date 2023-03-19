using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public static SceneManager instance;

    [SerializeField] private string mainMenuName = "MainMenu";
    [SerializeField] private string inGameName = "InGame";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    // Testing -----
    // void Start(){ // Remove this
    //     //AudioManager.instance.PlayMusic();
    //     TestInGameMusic.instance.PlayLevelMusic();
    // }
    // -----

    public void LoadMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(mainMenuName);
    }

    public void LoadGame()
    {
        //AudioManager.instance.PlayMusic();
        TestInGameMusic.instance.PlayLevelMusic();
        UnityEngine.SceneManagement.SceneManager.LoadScene(inGameName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
