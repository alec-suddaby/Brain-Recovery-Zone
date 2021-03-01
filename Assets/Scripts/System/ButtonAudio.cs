using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonAudio : MonoBehaviour
{
    public AudioClip clickSound;
    public AudioClip hoverSound;

    [Range(0.0f,1.0f)]
    public float audioVolume = 0.8f;
    private AudioSource source { get { return GetComponent<AudioSource>(); } }
    
    void Start()
    {  
        gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
    }

    public void PlayClickSound()
    {
        if(clickSound != null)
            source.PlayOneShot(clickSound);
    }

    public void PlayHoverSound()
    {
        if(hoverSound != null)
            source.PlayOneShot(hoverSound);
    }

}
