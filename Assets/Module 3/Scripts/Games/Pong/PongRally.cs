using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class PongRally : MonoBehaviour
{
    private int score = 0;
    public int Score => score;

    public Text scoreText;
    public TextMeshProUGUI endOfGameScoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI performanceCommentText;
    public UnityEvent gameOver;
    public UnityEvent pointScored;
    public string highScorePref;

    public void Start(){
        scoreText.text = score.ToString();
    }

    public void IncreaseRallyScore(){
        score++;
        scoreText.text = score.ToString();

        pointScored.Invoke();
    }

    public void GameOver(){
        endOfGameScoreText.text = score.ToString();
        int bestScore = PlayerPrefs.HasKey(highScorePref) ? PlayerPrefs.GetInt(highScorePref) : 0;
        
        if(score > bestScore){
            PlayerPrefs.SetInt(highScorePref, score);
            PlayerPrefs.Save();
            bestScore = score;
            performanceCommentText.text = "New High Score!";
        }

        scoreText.text = score.ToString();
        highScoreText.text = bestScore.ToString();

        gameOver.Invoke();
    }
}
