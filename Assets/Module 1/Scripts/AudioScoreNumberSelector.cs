using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;
using System;

public class AudioScoreNumberSelector : TaskScore
{
    private AudioCountTask audioTask;

    public UnityEvent correctGuess;
    public UnityEvent incorrectGuess;

    void Start(){
        audioTask = (AudioCountTask)taskLevel;
    }

    protected override void SaveScore()
    {
        int guess = FindObjectOfType<NumberSelector>().GetGuess;
        int actualSounds = audioTask.GetNumberOfTriggerSounds;

         if(!File.Exists(Application.persistentDataPath + "/" + fileName.Replace(' ', '-') + ".csv")){
            File.WriteAllLines(Application.persistentDataPath + "/" +  fileName.Replace(' ', '-') + ".csv", new List<string>(){ ("Level Name,Date Completed,Number Selected,Correct Value") });
        }

        File.AppendAllLines(Application.persistentDataPath + "/" +  fileName.Replace(' ', '-') + ".csv", new List<string>(){ ($"{audioTask.levelName.GetLevelName},{DateTime.Now.ToString()},{guess.ToString()},{actualSounds.ToString()}") });
        
        if(guess == actualSounds){
            correctGuess.Invoke();
            return;
        }

        incorrectGuess.Invoke();
    }
}
