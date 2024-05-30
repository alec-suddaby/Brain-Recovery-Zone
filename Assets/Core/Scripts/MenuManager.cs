using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject[] menus;
    public bool isMainMenu = false;

    void Start(){
        if(LoadLevelSettings.loadLevelSettings.currentMenu == -1 || !isMainMenu){
            return;
        }
        ChangeMenu(menus[LoadLevelSettings.loadLevelSettings.currentMenu]);
    }

    public void ChangeMenu(GameObject menu){
        for(int i = 0; i < menus.Length; i++){
            menus[i].SetActive(false);
            if(menus[i] == menu && isMainMenu){
                LoadLevelSettings.loadLevelSettings.currentMenu = i;
            }
        }

        menu.SetActive(true);
    }

    public void OverrideLastMenu(GameObject menu){
        for(int i = 0; i < menus.Length; i++){
            menus[i].SetActive(false);
            if(menus[i] == menu && isMainMenu){
                LoadLevelSettings.loadLevelSettings.currentMenu = i;
            }
        }
    }
}
