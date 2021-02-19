using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviousPanelMemory : MonoBehaviour
{
    [Header("Current Panel")]
    public Panel savedPanel = null;
    public string savedPanelSting;
    public GameObject savedPanelGameObject;

    [Header("Saved History")]
    public List<Panel> savedPanelList;
    public string savedPanelListSingleString;
    public List<string> savedPanelListString = new List<string>();
    public GameObject savedStringPanelHistoryGameObject;

    //private const char panelListSeparator = ',';

   


    public void SavedToString()
    {
      SavedPanelToString();
      SavedHistoryToString();
    }

    public void SavedFromString()
    {
      SavedPanelFromString();
      SavedHistroyFromString();
    }

    void SavedPanelToString()
    {
      savedPanelSting = savedPanel.ToString();
      savedPanelSting = savedPanelSting.Substring(0, savedPanelSting.Length - 8);
    }

    void SavedPanelFromString()
    {
      if (savedPanelSting != "")
      {
        //Debug.Log("Saved Panel Full");
        //Debug.Log("Saved Panel: " + savedPanelSting);
        savedPanelGameObject = GameObject.Find(savedPanelSting);
        //Debug.Log("Saved Panel Game Object: " + savedPanelGameObject);
        savedPanel = savedPanelGameObject.GetComponent<Panel>();
        //Debug.Log(savedPanel);
      }
      else
      {
        //Debug.Log("Saved Panel Empty");
        savedPanel = null;
      }
        
    }

    void SavedHistoryToString()
    {
      // clear list
      savedPanelListString.Clear();

      foreach (var panel in savedPanelList)
      {
        // convert each panel to a string
        savedPanelListSingleString = panel.ToString();
        savedPanelListSingleString = savedPanelListSingleString.Substring(0, savedPanelListSingleString.Length - 8);

        // place each string into a string list
        savedPanelListString.Add(savedPanelListSingleString);
      }
    }

    void SavedHistroyFromString()
    {
      // clear list
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
