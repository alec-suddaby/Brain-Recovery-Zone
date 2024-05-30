using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.Events;

public class SelfEvaluation : MonoBehaviour
{
    public string fileName = "SelfEvaluationScores";
    public int score = 5;
    public UnityEvent<int> scoreChanged;

    public void Start(){
        scoreChanged.Invoke(score);
    }

    public void SetScore(int s){
        score = s;
        scoreChanged.Invoke(score);
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
