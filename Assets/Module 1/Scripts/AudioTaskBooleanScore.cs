using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class AudioTaskBooleanScore : TaskScore
{
    private AudioCountTask audioCountTaskLevel;

    void Start(){
        audioCountTaskLevel = (AudioCountTask)taskLevel;
    }

    protected override void SaveScore(){
        List<AudioCountTask.AudioTaskSound> countTaskSounds = audioCountTaskLevel.CompletedSounds;
        int numberOfTriggerSounds = audioCountTaskLevel.GetNumberOfTriggerSounds;
        score = (int)FindObjectOfType<SelfEvaluation>().score.value;
        if(score > 1){
            score = 0;
        }

        if(!File.Exists(Application.persistentDataPath + "/" + fileName.Replace(' ', '-') + ".csv")){
            File.WriteAllLines(Application.persistentDataPath + "/" +  fileName.Replace(' ', '-') + ".csv", new List<string>(){ ("Level Name,Date Completed,Did Sound Play Response,Did Sound Actually Play") });
        }

        File.AppendAllLines(Application.persistentDataPath + "/" +  fileName.Replace(' ', '-') + ".csv", new List<string>(){ ($"{audioCountTaskLevel.levelName.GetLevelName},{DateTime.Now.ToString()},{(score > 0).ToString()},{(audioCountTaskLevel.GetNumberOfTriggerSounds > 0).ToString()}")});
    }
}
