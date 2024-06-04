using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelfEvaluation : MonoBehaviour
{
    public string fileName = "SelfEvaluationScores";
    public Slider score;
    public UnityEvent<int> scoreChanged;

    public void Start(){
        scoreChanged.Invoke((int)score.value);
        score.onValueChanged.AddListener(SetScore);
    }

    public void SetScore(float s){
        scoreChanged.Invoke((int)score.value);
    }

    public void SaveSelfEvaluation(string levelName, bool alertingSound, bool infoText, int correctButtonPresses, int totalButtonPresses, int expectedButtonPresses){
        //try{
            if(!File.Exists(Application.persistentDataPath + "/" + fileName.Replace(' ', '-') + ".csv")){
                File.WriteAllLines(Application.persistentDataPath + "/" +  fileName.Replace(' ', '-') + ".csv", new List<string>(){ ("Level Name, Date Completed,Self Evalutation Score,Alerting Sound Enabled,Task Reminder Enabled,Correct Button Presses,Total Button Presses,Actual Number of Special Announcements") });
            }
            File.AppendAllLines(Application.persistentDataPath + "/" +  fileName.Replace(' ', '-') + ".csv", new List<string>(){ ($"{levelName},{DateTime.Now.ToString()},{score.ToString()},{alertingSound.ToString()}, {infoText.ToString()},{correctButtonPresses.ToString()},{totalButtonPresses.ToString()}, {expectedButtonPresses.ToString()}") });
            //t.text = (Application.persistentDataPath + "/" +  fileName.Replace(' ', '-') + ".csv" + " was updated");
        // }catch(Exception e){
        //     //t.text = e.Message;
        // }
    }
}
