using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddAlerterSound : MonoBehaviour
{
    public FixedGapAudioTask task;
    public FixedGapAudioTask.AlerterSound taskSound;

    public string playerPref = "";

    public void Start(){
        if(!PlayerPrefs.HasKey(playerPref) || PlayerPrefs.GetInt(playerPref) == 1){
            task.alerterSounds.Add(taskSound);
        }
    }
}
