using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class AudioClipAndTitle
{
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private AudioClip overrideIntroAudio;
    public AudioClip GetAudioClip{
        get => audioClip;
    }

    public AudioClip GetIntroAudioClip{
        get => overrideIntroAudio == null ? audioClip : overrideIntroAudio;
    }
    
    public string title;

    public AudioClipAndTitle(AudioClip clip, string clipTitle){
        audioClip = clip;
        title = clipTitle;
    }
}