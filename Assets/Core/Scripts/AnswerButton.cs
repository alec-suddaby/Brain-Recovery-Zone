using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class AnswerButton : MonoBehaviour
{
    public Text questionText;
    private bool correct;
    public bool Correct => correct;
    public UnityEvent<bool> clicked;

    public void SetupButton(string text, bool correctAnswer){
        correct = correctAnswer;
        questionText.text = text;
    }

    public void Clicked(){
        clicked.Invoke(correct);
    }
}
