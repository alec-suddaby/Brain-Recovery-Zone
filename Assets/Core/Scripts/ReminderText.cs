using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReminderText : MonoBehaviour
{
    public GameObject reminderCanvas;
    public TextMeshProUGUI reminderText;
    public string playerPref = "";
    public string reminder = "";

    public bool callOnStart = true;
    public bool updateTextOnStart = false;

    public void Start(){
        if(callOnStart){
            SetReminder();
        }

        if(updateTextOnStart){
            reminderText.text = reminder;
        }
    }

    public void SetReminder(){
        if(!PlayerPrefs.HasKey(playerPref) || PlayerPrefs.GetInt(playerPref) == 1){
            reminderCanvas.SetActive(true);
            reminderText.text = reminder;
        }
    }
}
