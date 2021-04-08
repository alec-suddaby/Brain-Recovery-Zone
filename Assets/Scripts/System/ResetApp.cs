using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetApp : MonoBehaviour
{
    public MenuManager menuManager;
    
    public void ResetAppFunction()
    {
        PlayerPrefs.DeleteAll();
        //menuManager.LoadScene(MainMenuAddative);
        menuManager.ClearHistory();
        menuManager.BackToMenu();
    }
}
