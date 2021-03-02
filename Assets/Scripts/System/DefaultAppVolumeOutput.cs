using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefaultAppVolumeOutput : MonoBehaviour
{
    private DefaultAppVolume defaultAppVolumeComponent;

    private Slider defaultVolumeSlider;
    
    void Start()
    {
        defaultAppVolumeComponent = FindObjectOfType<DefaultAppVolume>();
        defaultVolumeSlider = gameObject.GetComponent<Slider>();

        if(defaultAppVolumeComponent == null)
        {
            Debug.Log("Connection to 'DefaultAppVolume' could not be made");
        }
        else
        {
            defaultVolumeSlider.value = defaultAppVolumeComponent.defaultAppVolume;
        }
        
    }

    void Update()
    {
        defaultAppVolumeComponent.defaultAppVolume = defaultVolumeSlider.value;
    }
}
