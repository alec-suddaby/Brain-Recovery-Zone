using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopMenuToolsMenuRelationship : MonoBehaviour
{
    private Canvas thisCanvas = null;

    public GameObject backButton;
    
    private void Awake()
    {
        thisCanvas = GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
       if(thisCanvas.isActiveAndEnabled == true)
       {
            backButton.SetActive(false);
       }
       else if (thisCanvas.isActiveAndEnabled == false)
       {
           backButton.SetActive(true);
       }
       else
       {
           backButton.SetActive(true);
       }

    }
}
