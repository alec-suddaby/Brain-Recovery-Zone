using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(ReminderText))]
public class AudioIntroWithRandomTask : TaskIntroduction
{
    public FixedGapAudioTask task;

    public List<AudioClipAndTitle> clips = new List<AudioClipAndTitle>();

    public TriggerClipSelection triggerClipSource = TriggerClipSelection.GenericAudio;

    public enum TriggerClipSelection{
        GenericAudio,
        TriggerAudio
    }

    public string descriptionString = "<SpecialCharacter1/>";
    public int numberOfSpecialCharacters = 1;

    public AudioClip betweenTriggerSound;
    public AudioClip beforeLastTriggerSound;

    [Range(0f, 1f)]
    public float chanceOfTriggerSoundOccuring = 1f;
    public bool addUnusedTriggerSoundsToGeneric = false;

    //[SerializeField] private ReminderStyle reminderStyle = ReminderStyle.PlayerPrefOrToggle;
    private ReminderText reminderHandler;

    public override void InitTask()
    {
        reminderHandler = GetComponent<ReminderText>();

        if(UnityEngine.Random.Range(0f, 1f) > chanceOfTriggerSoundOccuring){
            task.numberOfGenericAudioClips += task.numberOfTriggerAudioClips;
            task.numberOfTriggerAudioClips = 0;
        }
        
        List<int> selectedClips = new List<int>();

        for(int i = 1; i <= numberOfSpecialCharacters; i++){
            int selectedIndex;
            do{
                selectedIndex = UnityEngine.Random.Range(0, clips.Count);
            }while(selectedClips.Contains(selectedIndex));

            selectedClips.Add(selectedIndex);

            string reminderText = descriptionString.Replace("<SpecialCharacter" + i + "/>", clips[selectedIndex].title);
            reminderHandler.SetReminderText(reminderText);

            if(triggerClipSource == TriggerClipSelection.GenericAudio){
                task.genericAudio.Remove(clips[selectedIndex].GetAudioClip);
                task.triggerClips.Add(clips[selectedIndex].GetAudioClip);
            }
            clipsToPlay.Add(clips[selectedIndex].GetIntroAudioClip);
            
            if(i == numberOfSpecialCharacters - 1 && numberOfSpecialCharacters > 1){
                clipsToPlay.Add(beforeLastTriggerSound);
            }

            if(i < numberOfSpecialCharacters - 1 && numberOfSpecialCharacters > 1){
                clipsToPlay.Add(betweenTriggerSound);
            }
        }

        if(triggerClipSource == TriggerClipSelection.TriggerAudio){
            task.triggerClips = new List<AudioClip>();
            foreach(int i in selectedClips){
                task.triggerClips.Add(clips[i].GetAudioClip);
            }
        }

        if(addUnusedTriggerSoundsToGeneric){
            for(int i = 0; i < clips.Count; i++){
                if(selectedClips.Contains(i)){
                    continue;
                }

                task.genericAudio.Add(clips[i].GetAudioClip);
            }
        }
        
        base.InitTask();
    }
}