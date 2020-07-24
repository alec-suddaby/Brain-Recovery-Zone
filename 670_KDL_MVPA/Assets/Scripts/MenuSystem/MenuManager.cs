using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class MenuManager : MonoBehaviour
{
    public Panel currentPanel = null;
    public GameObject carInteriors;

    private List<GameObject> panelPopupBox = new List<GameObject>();
    private Canvas panelPopupBoxCanvas;
    private bool panelPopupBoxCanvasBool;
    // When this turns to true it will trigger the background to be removed
    private bool panelPopupBoxCanvasTriggerBackground = true;

    private List<Panel> panelHistory = new List<Panel>();

    public AudioClip backButtonPress;
    public AudioClip buttonDownSound;

    private AudioSource backAudioSrc;

    private Button[] allButtons;

    private void Start()
    {
        SetupPanels();
        SetupCarInteriors();
        backAudioSrc = GetComponent<AudioSource>();
    }

    private void SetupPanels()
    {
        Panel[] panels = GetComponentsInChildren<Panel>();

        foreach(Panel panel in panels)
            panel.Setup(this);

        currentPanel.Show();
    }

    private void SetupCarInteriors()
    {
        //Debug.Log("Remove Car Interior");
        // Sets each child of CarInteriors GameObject is active to false
        for (int a = 0; a < carInteriors.transform.childCount; a++)
        {
            carInteriors.transform.GetChild(a).gameObject.SetActive(false);
        }
        //Debug.Log("Bool panelPopupBoxCanvasTriggerBackground: " + panelPopupBoxCanvasTriggerBackground);
    }

    private void Update()
    {
        //if (OVRInput.GetDown(OVRInput.Button.Back) || OVRInput.GetDown(OVRInput.Button.Two))
        if (InputDevice. || OVRInput.GetDown(OVRInput.Button.Two))
        {
            backAudioSrc.PlayOneShot(backButtonPress);
            GoToPrevious();
        }

        //trying to make the audio work for all buttons
        /*
        for (int i = 0; i < allButtons.Length; i++)
        {
            allButtons[].onClick;
        }
        */

    }

    /*
    public void TaskOnClick( int buttonIndex )
    {
        backAudioSrc.PlayOneShot(buttonDownSound);
    }
    */

    public void GoToPrevious()
    {
        //Debug.Log("Start Back Press");
        
        if(panelHistory.Count == 0)
        //execute function if there is no panel histroy, for example exiting the app? else..... return;
        {
            OVRManager.PlatformUIConfirmQuit();
            return;
        }
        
        //Finding if any of the Panel_PopupBox canvas are active
        //CURRENTLY ONLY WORKS WITH ONE I THINK
        //panelPopupBox.AddRange(GameObject.FindGameObjectsWithTag("Panel_PopupBox"));
        panelPopupBox.AddRange(GameObject.FindGameObjectsWithTag("Panel_PopupBox"));

        //Debug.Log("Amount of panel boxes in array " + panelPopupBox.Count);

        if(panelPopupBox.Count > 0)
        {  
            for (int p = 0; p < panelPopupBox.Count; p++)
            {
                //Debug.Log("Loop Start");
                panelPopupBoxCanvas = panelPopupBox[p].GetComponent<Canvas>();
                panelPopupBoxCanvasBool = panelPopupBoxCanvas.enabled;
                //Debug.Log("Checking against number: " + p + " = " + panelPopupBoxCanvasBool);

                // If any Panel_PopupBox are active (true) then this script runs
                if(panelPopupBoxCanvasBool == true)
                {
                    // Setting the panelPopupBoxCanvasTriggerBackground to false will stop the SetupCarInteriors script from running
                    panelPopupBoxCanvasTriggerBackground = false;
                }
                //Debug.Log("Loop End"); 
            }
            //Debug.Log("Bool panelPopupBoxCanvasTriggerBackground: " + panelPopupBoxCanvasTriggerBackground);

            // If a Panel_PopupBox is active and the panelPopupBoxCanvasTriggerBackground has been set to true then the car intereior will be removed by running SetupCarInteriors
            if(panelPopupBoxCanvasTriggerBackground == true)
            {
                //Debug.Log("Run SetupCarInteriors();");
                SetupCarInteriors();
            }
        }

        int lastIndex = panelHistory.Count - 1;
        SetCurrent(panelHistory[lastIndex]);
        panelHistory.RemoveAt(lastIndex);
        //Clear list of Panel_PopupBox
        panelPopupBox.Clear();

        // Reset the trigger to true
        panelPopupBoxCanvasTriggerBackground = true;

        //Debug.Log("End Back Press");
    }
 
    public void SetCurrentWithHistory(Panel newPanel)
    {
        ClickSound();
        panelHistory.Add(currentPanel);
        SetCurrent(newPanel);
    }

    public void SetCurrentChildWithHistory(Panel newPanel)
    {
        ClickSound();
        panelHistory.Add(currentPanel);
        SetCurrentChild(newPanel);
    }

    public void SetCurrent(Panel newPanel)
    {
        ClickSound();
        currentPanel.Hide();

        currentPanel = newPanel;
        currentPanel.Show();
    }

    public void SetCurrentChild(Panel newPanel)
    {
        ClickSound();
        currentPanel = newPanel;
        currentPanel.Show();
    }

    // Should no longer need this function as the Previous() function now handles if the interior should be removed
    /*public void SetCurrentRemoveInteriors(Panel newPanel)
    {
        currentPanel.Hide();
        SetupCarInteriors();

        currentPanel = newPanel;
        currentPanel.Show();
    }*/

    public void LoadScene(string level)
    {
        SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive);
    }

    public void ClickSound()
    {
        backAudioSrc.PlayOneShot(buttonDownSound);
    }
}
