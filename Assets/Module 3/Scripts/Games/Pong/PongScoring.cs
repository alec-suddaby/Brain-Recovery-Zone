using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class PongScoring : MonoBehaviour
{
    public Text opponentScoreText;
    public Text playerScoreText;

    public TextMeshProUGUI gameFinishedScore;
    public TextMeshProUGUI gameOverMessage;
    public string victoryMessage = "Victory";
    public string defeatMessage = "Unlucky";

    private int opponentScore = 0;
    public int OpponentScore => opponentScore;

    private int playerScore = 0;
    public int PlayerScore => playerScore;

    public int scoreToWin = 7;
    public UnityEvent PointScored;
    public UnityEvent gameOver;

    void Start()
    {
        gameOver.AddListener(GameFinished);
    }

    private void GameFinished(){
        gameFinishedScore.text = $"{opponentScore} - {playerScore}";
        gameOverMessage.text = playerScore > opponentScore ? victoryMessage : defeatMessage;
    }

    public void OpponentScored(){
        opponentScore++;
        UpdateText();
        CheckForWin();
    }

    public void PlayerScored(){
        playerScore++;
        UpdateText();
        CheckForWin();
    }

    public void UpdateText(){
        opponentScoreText.text = opponentScore.ToString();
        playerScoreText.text = playerScore.ToString();
    }
    
    public void CheckForWin(){
        if(playerScore < scoreToWin && opponentScore < scoreToWin){
            PointScored.Invoke();
            return;
        }
        
        int difficulty = PlayerPrefs.GetInt("Module3TableTennisAceDifficulty", 2);

        int scoreDifference = playerScore - opponentScore;

        if(scoreDifference <= -2){
            difficulty --;
        }else if(scoreDifference >= 2){
            difficulty ++;
        }

        difficulty = Mathf.Clamp(difficulty, 0, 5);

        PlayerPrefs.SetInt("Module3TableTennisAceDifficulty", difficulty);
        PlayerPrefs.Save();

        gameOver.Invoke();
    }
}
