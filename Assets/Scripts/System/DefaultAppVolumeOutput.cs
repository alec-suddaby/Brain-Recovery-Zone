using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefaultAppVolumeOutput : MonoBehaviour
{
    private Slider defaultVolumeSlider;

    [Range(0,1)]
    public float defaultAppVolume = 0.6f;
    
    void Start()
    {
        defaultVolumeSlider = gameObject.GetComponent<Slider>();

        // Set the value on the first load with no player prefs
        if(PlayerPrefs.GetInt("SetDefaultAppVolume") == 0)
        {
            PlayerPrefs.SetFloat("DefaultAppVolume", defaultAppVolume);
            PlayerPrefs.SetInt("SetDefaultAppVolume", 1);
        }
        
        defaultVolumeSlider.value = PlayerPrefs.GetFloat("DefaultAppVolume");
    }

    public void UpdateAppVolume()
    {
        PlayerPrefs.SetFloat("DefaultAppVolume", defaultVolumeSlider.value);
        Debug.Log("App Volume Player Prefs: " + PlayerPrefs.GetFloat("DefaultAppVolume"));
    }
}
