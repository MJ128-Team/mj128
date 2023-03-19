using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public static SceneManager instance;

    [SerializeField] private string mainMenuName = "MainMenu";
    [SerializeField] private string inGameName = "InGame";

    public SpriteRenderer blankingScreen;
    [SerializeField] private float fadeBlackDuration = 0.5f;
    [SerializeField] private float fadeStayDuration = 0.2f;
    [SerializeField] private float fadeClearDuration = 0.5f;
    [SerializeField] private float fadeTimer = 0f;
    [SerializeField] private bool isFading = false;

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

    void Update()
    {
        HandleBlankingScreen();
    }

    void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    void OnSceneLoaded()
    {
        FadeToClear();
    }

    void HandleBlankingScreen()
    {
        if(isFading)
        {
            fadeTimer += Time.deltaTime;

            if(fadeTimer <= fadeBlackDuration)
            {
                FadeToBlack();
            }
            else if(fadeTimer <= fadeBlackDuration + fadeStayDuration)
            {
                // Do nothing
            }
            else if(fadeTimer <= fadeBlackDuration + fadeStayDuration + fadeClearDuration)
            {
                FadeToClear();
            }
            else
            {
                isFading = false;
                fadeTimer = 0f;
            }
        }
    }

    void FadeToBlack()
    {    
        float pos = fadeTimer / fadeBlackDuration;
        float alpha = Mathf.Lerp(pos, 0f, 1f);
        Debug.Log("Alpha: " + alpha);
        blankingScreen.color = new Color(0f, 0f, 0f, pos);
    }

    void FadeToClear()
    {
        float pos = (fadeTimer-fadeBlackDuration-fadeStayDuration) / fadeClearDuration;
        float alpha = Mathf.Lerp(pos, 1f, 0f);
        Debug.Log("Alpha: " + alpha);
        blankingScreen.color = new Color(0f, 0f, 0f, pos);
    }

    // Testing -----
    // void Start(){ // Remove this
    //     //AudioManager.instance.PlayMusic();
    //     TestInGameMusic.instance.PlayLevelMusic();
    // }
    // -----

    public void LoadMainMenu()
    {
        TestInGameMusic.instance.StopLevelMusic();

        //isFading = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene(mainMenuName);

    }

    public void LoadGame()
    {

        Debug.Log("LoadGame");
        //isFading = true;
        //AudioManager.instance.PlayMusic();
        TestInGameMusic.instance.PlayLevelMusic();
        UnityEngine.SceneManagement.SceneManager.LoadScene(inGameName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
  
}
