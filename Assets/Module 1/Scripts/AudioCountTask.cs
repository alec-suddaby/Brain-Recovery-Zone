using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCountTask : TaskCountdown
{   
    public List<AudioTaskSound> audioTaskSounds = new List<AudioTaskSound>();
    private List<AudioTaskSound> completedTaskSounds= new List<AudioTaskSound>();
    public List<AudioTaskSound> CompletedSounds{
        get => completedTaskSounds;
    }

    public int GetNumberOfTriggerSounds{
        get{
            int noSounds = 0;
            foreach(AudioTaskSound sound in completedTaskSounds){
                if(sound.isTriggerSound){
                    noSounds++;
                }
            }

            return noSounds;
        }
    }
    public AudioSource audioSource;

    [System.Serializable]
    public class AudioTaskSound{
        public AudioClip taskAudio;
        public bool isTriggerSound;
        public float playAtTimeSeconds;

        public AudioTaskSound(AudioClip audio, bool triggerSound){
            taskAudio = audio;
            isTriggerSound = triggerSound;
        }
    }

    public override void InitTask()
    {
        float latestClipEndTime = 0;
        foreach(AudioTaskSound sound in audioTaskSounds){
            float endTime = sound.playAtTimeSeconds + sound.taskAudio.length;
            if(endTime > latestClipEndTime){
                latestClipEndTime = endTime;
            }
        }

        taskLengthSeconds = latestClipEndTime;
        base.InitTask();

        updateTick.AddListener(CheckAudioToPlay);
    }

    private void CheckAudioToPlay(){
        if(taskComplete || audioTaskSounds.Count == 0){
            return;
        }
        if(timeElapsed > audioTaskSounds[0].playAtTimeSeconds){
            PlayAudioClip(audioTaskSounds[0].taskAudio);
            completedTaskSounds.Add(audioTaskSounds[0]);
            audioTaskSounds.RemoveAt(0);
        }
    }

    protected virtual void PlayAudioClip(AudioClip clip){
        audioSource.PlayOneShot(clip);   
    }
}
