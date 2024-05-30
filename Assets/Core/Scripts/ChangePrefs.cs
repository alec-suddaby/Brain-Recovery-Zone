using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChangePrefs : MonoBehaviour
{
    public string prefName;
    public Toggle toggle;
    public bool defaultState = true;
    public UnityEvent<bool> isEnabled;
    public bool callOnStart = true;

    void Start(){
        if(callOnStart){
            LoadPrefs();
        }
    }
    
    public void LoadPrefs(){
        if(PlayerPrefs.HasKey(prefName)){
            isEnabled.Invoke(GetBool());
            if(toggle != null)
                toggle.isOn = GetBool();
            return;
        }

        isEnabled.Invoke(defaultState);
        SetBool(defaultState);
        if(toggle != null)
            toggle.isOn = defaultState;
    }

    public void SetBool(bool isTrue){
        isEnabled.Invoke(isTrue);
        PlayerPrefs.SetInt(prefName, isTrue ? 1 : 0);
        PlayerPrefs.Save();
    }

    public bool GetBool(){
        return PlayerPrefs.GetInt(prefName) == 1;
    }
}
