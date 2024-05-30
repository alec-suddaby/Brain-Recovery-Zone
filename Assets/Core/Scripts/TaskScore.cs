using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;
using System;
using UnityEngine.UI;

public class TaskScore : MonoBehaviour
{
    public GameObject display;
    public string fileName = "SelfEvaluationScores";
    protected int score = 5;
    public TimedTask taskLevel;
    public Button saveScoreButton;
    public UnityEvent saveComplete;
    void Awake(){
        saveScoreButton.onClick.AddListener(Save);
    }

    public void TaskComplete(){
        display.SetActive(true);
    }

    public void SetSelfEvaluationScore(int selfEvaluation){
        score = selfEvaluation;
    }

    public void Save(){
        Debug.Log("Save Called");
        SaveScore();
        Debug.Log("Saved " + Application.persistentDataPath + "/" + fileName.Replace(' ', '-') + ".csv");
        saveComplete.Invoke();
        Debug.Log("Scene Loaded");
    }

    protected virtual void SaveScore(){
        Debug.Log("Test");
        score = FindObjectOfType<SelfEvaluation>().score;
        if(!File.Exists(Application.persistentDataPath + "/" + fileName.Replace(' ', '-') + ".csv")){
            File.WriteAllLines(Application.persistentDataPath + "/" +  fileName.Replace(' ', '-') + ".csv", new List<string>(){ ("Level Name,Date Completed,Score") });
        }
        File.AppendAllLines(Application.persistentDataPath + "/" +  fileName.Replace(' ', '-') + ".csv", new List<string>(){ ($"{taskLevel.levelName.GetLevelName},{DateTime.Now.ToString()},{score.ToString()}") });
    }
}
