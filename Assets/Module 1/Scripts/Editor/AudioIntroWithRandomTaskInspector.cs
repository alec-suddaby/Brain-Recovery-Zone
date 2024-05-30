using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AudioIntroWithRandomTask))]
public class AudioIntroWithRandomTaskInspector : Editor{
    public override void OnInspectorGUI()
    {
        AudioIntroWithRandomTask task = (AudioIntroWithRandomTask) target;
        
        List<AudioClipAndTitle> tempClipsAndTitles = new List<AudioClipAndTitle>();
        
        CopyList<AudioClipAndTitle>(task.clips, tempClipsAndTitles);
        task.clips.Clear();
        
        List<AudioClip> clipsToUse = task.triggerClipSource == AudioIntroWithRandomTask.TriggerClipSelection.GenericAudio ? task.task.genericAudio : task.task.triggerClips;

        foreach(AudioClip clip in clipsToUse){
            AudioClipAndTitle temp;
            if((temp = AlreadyCreated(tempClipsAndTitles,clip)) == null){
                temp = new AudioClipAndTitle(clip, clip.name);
            }
            task.clips.Add(temp);
        }
        
        base.OnInspectorGUI();
    }

    private AudioClipAndTitle AlreadyCreated(List<AudioClipAndTitle> clipsAndTitles, AudioClip searchClip){
        foreach(AudioClipAndTitle clipAndTitle in clipsAndTitles){
            if(clipAndTitle.GetAudioClip == searchClip){
                return clipAndTitle;
            }
        }
        return null;
    }

    void CopyList<T>(List<T> original, List<T> copy){
        foreach(T t in original){
            copy.Add(t);
        }
    }
}