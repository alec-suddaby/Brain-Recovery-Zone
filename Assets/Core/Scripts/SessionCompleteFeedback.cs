using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SessionCompleteFeedback : MonoBehaviour
{
    [System.Serializable]
    public class FeedbackForScore{
        public int minScore = 0;
        public int maxScore = -1;
        public string feebackText;

        public bool IsCorrectFeedback(int currentScore){
            if((minScore == currentScore && maxScore == -1) || (minScore <= currentScore && maxScore >= currentScore)){
                return true;
            }

            return false;
        }
    }

    public TextMeshProUGUI feedbackText;
    public FeedbackForScore[] feedbacks;

    public void SelfEvaluationUpdated(int score){
        foreach(FeedbackForScore feedback in feedbacks){
            
            if(feedback.IsCorrectFeedback(score)){
                feedbackText.text = feedback.feebackText;
            }
        }
    }
}
