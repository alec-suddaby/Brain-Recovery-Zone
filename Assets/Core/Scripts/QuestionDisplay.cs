using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuestionDisplay : UiFade
{
    public Text questionText;
    public Button continueButton;
    public UnityEvent continueClicked;

    void Start() {
        continueButton.onClick.AddListener(Continue);
    }

    public void SetQuestion(string question) {
        questionText.text = question;
        StartCoroutine("FadeIn");
    }

    void Continue() {
        continueClicked.Invoke();
        StartCoroutine("FadeOut");
        continueClicked.RemoveAllListeners();
    }
}
