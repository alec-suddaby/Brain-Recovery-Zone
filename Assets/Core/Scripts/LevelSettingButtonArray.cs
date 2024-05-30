using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSettingButtonArray : MonoBehaviour
{
    public int moduleNumber;
    public string levelName = "";
    public string settingName="";

    public ButtonArray value;

    void Awake(){
        if(!PlayerPrefs.HasKey("Module"  + moduleNumber + levelName + settingName)){
            PlayerPrefs.SetInt("Module" + moduleNumber + levelName + settingName, 0);
            PlayerPrefs.Save();
        }        

        value.SelectNewButton(PlayerPrefs.GetInt("Module" + moduleNumber + levelName + settingName));
    }

    public void Save()
    {
        PlayerPrefs.SetInt("Module" + moduleNumber + levelName + settingName, value.selectedIndex);
        PlayerPrefs.Save();
    }
}
