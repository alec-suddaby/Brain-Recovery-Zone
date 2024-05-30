using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TaskIntroduction : TimedTask
{
    public List<AudioClip> clipsToPlay = new List<AudioClip>();
    public AudioSource audioSource;
    private bool complete = false;
    public float startDelay = 3f;
    private float startTime = 0f;

    public override void InitTask()
    {      
        base.InitTask();

        updateTick.AddListener(SetAudioState);
        startTime = Time.time + startDelay;
        taskLengthSeconds = startDelay + 1f;
        foreach(AudioClip clip in clipsToPlay){
            taskLengthSeconds += clip.length;
        }
    }

    void SetAudioState(){
        if(Time.time < startTime){
            return;
        }
        if(clipsToPlay.Count == 0){
            if(!complete && !audioSource.isPlaying){
                complete = true;
            }
            return;
        }
        if(audioSource.isPlaying){
            return;
        }

        audioSource.PlayOneShot(clipsToPlay[0]);
        clipsToPlay.RemoveAt(0);
    }
}
