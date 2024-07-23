using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Question", menuName = "Questions/MultipleChoiceQuestion", order = 1)]
public class MultipleChoiceQuestion : ScriptableObject
{
    public string question;

    public string correctAnswer;

    public string[] incorrectAnswerOptions;

    public int playTimeSeconds;
}
