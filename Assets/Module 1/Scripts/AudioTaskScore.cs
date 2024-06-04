using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class AudioTaskScore : TaskScore
{

    public TriggerPressCounter triggerPressCounter;
    private AudioCountTask audioCountTaskLevel;

    [Range(0,1)]
    public float triggerPointInAudio = 0f;
    public float correctButtonPressThresholdSeconds = 1f;

    void Start(){
        audioCountTaskLevel = (AudioCountTask)taskLevel;
    }

    protected override void SaveScore(){
        List<AudioCountTask.AudioTaskSound> countTaskSounds = audioCountTaskLevel.CompletedSounds;
        List<float> triggerPresses = triggerPressCounter.GetButtonPresses;
        int actualScore = GetScore(triggerPresses, countTaskSounds);
        int numberOfTriggerSounds = audioCountTaskLevel.GetNumberOfTriggerSounds;
        score = (int)FindObjectOfType<SelfEvaluation>().score.value;

        if(!File.Exists(Application.persistentDataPath + "/" + fileName.Replace(' ', '-') + ".csv")){
            File.WriteAllLines(Application.persistentDataPath + "/" +  fileName.Replace(' ', '-') + ".csv", new List<string>(){ ("Level Name,Date Completed,Self Evaluation Score,Correct Button Presses,Total Button Presses,Trigger Sounds Played,Total Sounds Played") });
        }

        File.AppendAllLines(Application.persistentDataPath + "/" +  fileName.Replace(' ', '-') + ".csv", new List<string>(){ ($"{audioCountTaskLevel.levelName.GetLevelName},{DateTime.Now.ToString()},{score.ToString()},{actualScore.ToString()},{triggerPresses.Count.ToString()},{numberOfTriggerSounds.ToString()},{countTaskSounds.Count.ToString()}") });
    }

    protected int GetScore(List<float> triggerPresses, List<AudioCountTask.AudioTaskSound> audioTasks){
        int s = 0;

        foreach(AudioCountTask.AudioTaskSound audioTask in audioTasks){
            foreach(float triggerPress in triggerPresses){
                if(triggerPress - audioTask.playAtTimeSeconds > 0 && triggerPress - audioTask.playAtTimeSeconds < (audioTask.taskAudio.length * triggerPointInAudio) + correctButtonPressThresholdSeconds){
                    s++;
                    break;
                }
            }
        }

        return s;
    }
}
