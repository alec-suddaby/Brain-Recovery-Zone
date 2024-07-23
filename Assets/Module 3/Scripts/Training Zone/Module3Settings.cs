using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Module3Settings : MonoBehaviour
{
    public string levelName = "";
    
    public bool defaultToggleEnable = true;

    public ButtonArray speed;
    public Toggle toggle;
    public ButtonArray quantity;
    public ButtonArray trainingSide;
    void Awake(){
        if(!PlayerPrefs.HasKey("Module3" + levelName + "Speed")){
            PlayerPrefs.SetInt("Module3" + levelName + "Speed", 0);
        
        }

        if(!PlayerPrefs.HasKey("Module3" + levelName + "Quantity")){
            PlayerPrefs.SetInt("Module3" + levelName + "Quantity", 0);
        }

        if(!PlayerPrefs.HasKey("Module3" + levelName + "Toggle")){
            PlayerPrefs.SetInt("Module3" + levelName + "Toggle", defaultToggleEnable ? 1 : 0);
        }

        if(!PlayerPrefs.HasKey("Module3" + levelName + "Eye")){
            PlayerPrefs.SetInt("Module3" + levelName + "Eye", (int)EyePatch.Eye.LeftEye);
        }

        PlayerPrefs.Save();

        speed.SelectNewButton(PlayerPrefs.GetInt("Module3" + levelName + "Speed"));
        quantity.SelectNewButton(PlayerPrefs.GetInt("Module3" + levelName + "Quantity"));
        toggle.isOn = PlayerPrefs.GetInt("Module3" + levelName + "Toggle") == 1;

        if(trainingSide != null){
            trainingSide.SelectNewButton(PlayerPrefs.GetInt("Module3" + levelName + "Eye", trainingSide.selectedIndex));
        }
    }

    public void ReadyToStart()
    {
        PlayerPrefs.SetInt("Module3" + levelName + "Speed", speed.selectedIndex);
        PlayerPrefs.SetInt("Module3" + levelName + "Quantity", quantity.selectedIndex);
        PlayerPrefs.SetInt("Module3" + levelName + "Toggle", toggle.isOn ? 1 : 0);
        if(trainingSide != null){
            PlayerPrefs.SetInt("Module3" + levelName + "Eye", trainingSide.selectedIndex);
        }
        PlayerPrefs.Save();
    }
}
