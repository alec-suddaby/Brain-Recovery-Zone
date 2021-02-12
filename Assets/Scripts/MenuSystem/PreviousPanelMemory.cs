using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviousPanelMemory : MonoBehaviour
{
    
    public string menuSystemCanvas;
    //public string videoMenuSystemCanvas;

    public MenuManager menuSystemLink;
    //public VideoMenuManager videoMenuSystemLink;

    static public string lastMenuPanel;
    //public List<Panel> savedPanelHistory;

   

    void Start()
    {
      menuSystemLink = GameObject.Find(menuSystemCanvas).GetComponent<MenuManager>();
      
      lastMenuPanel = menuSystemLink.currentPanel.ToString().Substring(0, menuSystemLink.currentPanel.ToString().Length -8);
    }

    void Update()
    {
      //Debug.Log(lastMenuPanel);
      //Debug.Log(menuSystemLink.currentPanel);
    }

    void OnDestroy(){
      Debug.Log("Previous Panel history cleared");
    }
}
