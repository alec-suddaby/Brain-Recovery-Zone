using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPitch : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] notes;

    public void Start(){
        audioSource.PlayOneShot(notes[Random.Range(0, notes.Length)]);
    }
}
