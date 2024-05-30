using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class AnswerDisplay : UiFade
{
    public Text QuestionText;
    public Transform AnswerGrid;
    public AnswerButton AnswerButtonPrefab;
    public UnityEvent<bool> Answered;

    public void SetQuestionAndAnswers(string question, string correctAnswer, string[] incorrectAnswers)
    {
        foreach(Transform child in AnswerGrid){
            Destroy(child.gameObject);
        }

        QuestionText.text = question;

        List<string> answers = new List<string> { correctAnswer };
        answers.AddRange(incorrectAnswers);

        for(int i = 0; i < answers.Count; i++){
            GameObject answerButton = Instantiate(AnswerButtonPrefab.gameObject, AnswerGrid);
            AnswerButton button = answerButton.GetComponent<AnswerButton>();
            button.SetupButton(answers[i], i == 0);
            button.clicked.AddListener(AnswerButtonClicked);
        }

        for(int i = 0; i < AnswerGrid.childCount - 1; i++){
            for(int j = 0; j < AnswerGrid.childCount; j++){
                if(Random.Range(0f,1f) > 0.5f){
                    int iIndex = AnswerGrid.GetChild(i).GetSiblingIndex();
                    int jIndex = AnswerGrid.GetChild(j).GetSiblingIndex();
                    AnswerGrid.GetChild(i).SetSiblingIndex(jIndex);
                    AnswerGrid.GetChild(j).SetSiblingIndex(iIndex);
                }
            }
        }

        StartCoroutine("FadeIn");
    }

    void AnswerButtonClicked(bool correct){
        Answered.Invoke(correct);
        Answered.RemoveAllListeners();
        StartCoroutine("FadeOut");
    }
}
