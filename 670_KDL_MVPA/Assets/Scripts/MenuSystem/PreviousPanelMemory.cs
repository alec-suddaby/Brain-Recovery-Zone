using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviousPanelMemory : MonoBehaviour
{
    
    public string menuSystemCanvas;

    public bool menuStatus = true;

    

    void Start()
    {
      //GameObject.Find(menuSystemCanvas).GetComponent<MenuManager>();
    }

    
    void Update()
    {
        if (menuStatus == true)
      {
          //GameObject.Find(menuSystemCanvas).GetComponent<MenuManager>().ShowMenu();
      }
      else if (menuStatus == false)
      {
         // GameObject.Find(menuSystemCanvas).GetComponent<MenuManager>().HideMenu();
      }
    }
}
