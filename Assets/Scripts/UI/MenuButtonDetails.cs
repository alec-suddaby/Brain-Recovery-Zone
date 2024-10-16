﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI.ProceduralImage;

[ExecuteInEditMode]
public class MenuButtonDetails : MonoBehaviour
{
    // Edit Menu Button Details
    [Header("Menu Button Details")]
    public string menuTitle = "Title";
    [TextArea]
    public string menuDescription = "Description";

    // Input Linked Button Bits
    [Header("Linked GameObjects")]
    public GameObject titleGameObject;
    public GameObject descriptionGameObject;

    private TextMeshProUGUI titleTMP;

    private TextMeshProUGUI decriptionTMP;

    [Header("Coming Soon")]
    public bool comingSoon;
    public GameObject comingSoonText;

    
    // Start is called before the first frame update
    void Start()
    {
        //Set Title
        if ( titleGameObject != null)
        {   
            titleTMP = titleGameObject.GetComponent<TextMeshProUGUI>();
            titleTMP.text = menuTitle;
        }

        //Set Description
        if ( descriptionGameObject != null)
        {   
            decriptionTMP = descriptionGameObject.GetComponent<TextMeshProUGUI>();
            decriptionTMP.text = menuDescription;
        }

        // Set Coming soon
        if (comingSoon == true && comingSoonText != null)
            comingSoonText.SetActive(true);
        else if (comingSoonText != null)
            comingSoonText.SetActive(false);

    }

    public void DisablePrompts()
    {
        MenuManager menuManager = FindObjectOfType<MenuManager>();

        if(menuManager)
        {
            menuManager.audioPrompt = false;
            menuManager.likertScalePopup = false;
            menuManager.customPopup = false;
        }

    }

}
