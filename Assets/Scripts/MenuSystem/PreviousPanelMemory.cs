using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviousPanelMemory : MonoBehaviour
{
    /*
    Info:

    This script is designed to save the current panel and the panel history when leaving the menu scene.
    Rather than storing that data to a save file, it is saved as a string on this script
    */
    
    [Header("Current Panel")]
    public Panel savedPanel = null;
    public string savedPanelSting;
    public GameObject savedPanelGameObject;

    [Header("Saved History")]
    public List<Panel> savedPanelList;
    public string savedPanelListSingleString;
    public List<string> savedPanelListString = new List<string>();
    public GameObject savedStringPanelHistoryGameObject;



    // function that save the panels to strings
    public void SavedToString()
    {
      SavedPanelToString();
      SavedHistoryToString();
    }

    // function that converts the saved strings back to panels
    public void SavedFromString()
    {
      SavedPanelFromString();
      SavedHistroyFromString();
    }

    void SavedPanelToString()
    {
      // Setting the panel to a string
      if (savedPanel != null)
        savedPanelSting = savedPanel.ToString();

      // Removing the last 8 characters " (Panel)" from the string to help with finding the correct GameObject when converting back from String to Panel
      if (savedPanel != null)
        savedPanelSting = savedPanelSting.Substring(0, savedPanelSting.Length - 8);
    }

    void SavedPanelFromString()
    {
      if (savedPanelSting != "")
      {
        // Finding the GameObject with the name of the saved panel
        savedPanelGameObject = GameObject.Find(savedPanelSting);

        // Selecting the Panel from the found GameObject
        savedPanel = savedPanelGameObject.GetComponent<Panel>();
      }
      else
      {
        savedPanel = null;
      }
    }

    void SavedHistoryToString()
    {
      // Clear list of strings from any previous save
      savedPanelListString.Clear();

      foreach (var panel in savedPanelList)
      {
        // convert each panel to a string and remove the " (Panel)" from the end
        savedPanelListSingleString = panel.ToString();
        savedPanelListSingleString = savedPanelListSingleString.Substring(0, savedPanelListSingleString.Length - 8);

        // place each string into a string list
        savedPanelListString.Add(savedPanelListSingleString);
      }
    }

    void SavedHistroyFromString()
    {
      // Clear list of strings to remove any objects that can't be found due to leaving the Main Menu scene
      savedPanelList.Clear();
      
      if (savedPanelListString.Count != 0)
      {
        foreach(var stringpanel in savedPanelListString)
        {
          // convert string to panel
          savedStringPanelHistoryGameObject = GameObject.Find(stringpanel);

          savedPanelList.Add(savedStringPanelHistoryGameObject.GetComponent<Panel>());
        }
      }
      else
      {
        savedPanelList = null;
      }
    }
}
