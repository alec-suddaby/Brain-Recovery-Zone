using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindButtonAudioGameObject : MonoBehaviour
{
    private GameObject buttonAudio;
    private ButtonAudio buttonAudioComponent;
    
    // Start is called before the first frame update
    void Start()
    {
        buttonAudio = GameObject.FindGameObjectWithTag("buttonAudio");

        buttonAudioComponent = buttonAudio.GetComponent<ButtonAudio>();
    }

    public void PlayClickSoundLinked()
    {
        if(buttonAudio != null)
            buttonAudioComponent.PlayClickSound();
        else
            Debug.Log("Can't play click sound");
    }

    public void PlayHoverSoundLinked()
    {
        if(buttonAudio != null)
            buttonAudioComponent.PlayHoverSound();
        else
            Debug.Log("Can't play hover sound");
    }

}
