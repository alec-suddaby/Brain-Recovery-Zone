using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    public AudioSource audioSource;

    public void PlayAudio(AudioClip audioClip){
        audioSource.Stop();
        audioSource.PlayOneShot(audioClip);
    }
}
