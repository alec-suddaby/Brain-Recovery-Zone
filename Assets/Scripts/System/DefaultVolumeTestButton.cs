using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultVolumeTestButton : MonoBehaviour
{
    private AudioSource testVolumeAudioSource;

    //private DefaultAppVolume defaultAppVolumeComponent;
    
    // Start is called before the first frame update
    void Start()
    {
        testVolumeAudioSource = gameObject.GetComponent<AudioSource>();

        //defaultAppVolumeComponent = FindObjectOfType<DefaultAppVolume>();
    }

    // Update is called once per frame
    void Update()
    {
        testVolumeAudioSource.volume = PlayerPrefs.GetFloat("DefaultAppVolume");
    }
}
