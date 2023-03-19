using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModalGameOver : MonoBehaviour
{
    [SerializeField] private Text sessionScoreText;
    [SerializeField] private Text bestScoreText;

    [SerializeField] private string baseScoreText = "SCORE - ";
    [SerializeField] private string baseBestScoreText = "BEST SCORE - ";

    private string score;
    private string scoreBest;


    void Start()
    {
        InGameUIManager.instance.onGameOver += OnGameOver;
    }

    void OnDestroy()
    {
        InGameUIManager.instance.onGameOver -= OnGameOver;
    }

    void OnGameOver(float current, float best)
    {
        score= ScoreToString(current);
        scoreBest= ScoreToString(best);

        transform.GetChild(0).gameObject.SetActive(true);
        ApplyScores();
    }

    string ScoreToString(float timeSecs)
    {
        int minutes = Mathf.FloorToInt(timeSecs / 60F);
        int seconds = Mathf.FloorToInt(timeSecs - minutes * 60);
        string stringTime = string.Format("{0:0}' {1:00}\"", minutes, seconds);
        return stringTime;
    }

    void ApplyScores()
    {
        sessionScoreText.text = "SCORE - " + score;
        bestScoreText.text = "BEST SCORE - " + scoreBest;
    }
}
