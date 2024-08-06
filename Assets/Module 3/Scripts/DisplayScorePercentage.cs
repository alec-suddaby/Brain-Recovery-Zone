using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayScorePercentage : MonoBehaviour
{
    public VideoQuestionTask task;
    public TextMeshProUGUI scoreText;
    public string introText = "Score: ";
    public void DisplayScore(){
        scoreText.text = introText + (((float) task.Score) / ((float) task.NumberOfQuestions)).ToString() + "%";
    }
}
