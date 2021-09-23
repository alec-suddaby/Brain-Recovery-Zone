using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.Events;

public class MenuManager : MonoBehaviour
{
    [Header("Persistent Menu")]
    public GameObject persistentLogo;

    // Hide and show Game Objects
    [Header("Persistent Menu Tools")]
    //public string topPanelName;
    public GameObject menuHomeButton;
    public GameObject menuBackButton;
    public GameObject menuHideButton;
    public GameObject menuShowButton;
    public GameObject menuSettingsButton;
    private bool menuHidden = false;
    private bool overrideHideBackButton = false;

    // Current panel buttonWrap
    private GameObject currentButtonWrap;
    private float crrentButtonWrapWidth;
    
    [Header("Panel Navigation")]
    [Tooltip("Set this as your default panel to show on load")]
    private Panel currentPanel = null;
    public Panel defaultPanel = null;
    private Component[] canvasInPanels;
    private List<Panel> panelHistory = new List<Panel>();
    private bool sceneLoading = false;

    [Header("XR Controller")]
    //WIll aim to remove or update this function
    //XR Toolkit controllers
    private XRNode xRNode = XRNode.LeftHand;
    private List<InputDevice> devices = new List<InputDevice>();
    private InputDevice device;
    private HandSelection masterHandSelection;

    //to avoid repeat readings
    private bool secondaryButtonIsPressed;

    private bool primaryTouchIsPressed;

    [Header("Audio Prompt")]
    public bool audioPrompt;
    public Panel audioPromptBoxPanel;
    private bool audioPromptGiven = false;
    private string savedLevelString;

    [Header("Likert Scale Popup")]
    public LikertScaleInteractionManager likertScaleInteractionManager;
    public bool likertScalePopup; // Is set true or faulse when hovering over a video button, set by the VideoButtonDetails script
    private bool likertScaleGiven = false;
    private bool showPostLikert;
    private bool likertLock = false;

    [Header("Custom Popup")]
    public bool customPopup;
    private bool customPopupGiven = false;
    public Panel customPopupPanelLink;
    private CustomPopupManager customPopupManager;

    void GetDevice()
    {
        InputDevices.GetDevicesAtXRNode(xRNode, devices);
        device = devices.FirstOrDefault();
    }

    void OnEnable()
    {
        if(!device.isValid)
        {
            GetDevice();
        }    
    }

    void Start()
    {       
        // Reset Loop function
        PlayerPrefs.SetInt("LoopVideo", 0);

        // Reset Mute function
        PlayerPrefs.SetInt("PlayMute", 0);
        savedLevelString = "";

        // Setting up if the post video Likert sacle should show
        float showPostLikertFloat = PlayerPrefs.GetInt("ShowPostLikert");
        if (showPostLikertFloat == 1) {showPostLikert = true;}
        else {showPostLikert = false;}
        PlayerPrefs.SetInt("ShowPostLikert", 0);
        
        SetupPanels();
        ShowMenu();

        //Get Hand Selection
        masterHandSelection = FindObjectOfType<HandSelection>();
        xRNode = masterHandSelection.masterXRNode;
    }

    void SetupPanels()
    {
        Panel[] panels = GetComponentsInChildren<Panel>();
        
        foreach(Panel panel in panels)
            panel.Setup(this);

        GetPanelHistoryMemory(); // Pulls the current panel and panel history from the player prefs
    
        if (showPostLikert == true && likertScaleInteractionManager.postVideoLikertScalePanel != null)
        {
            SetCurrent(likertScaleInteractionManager.postVideoLikertScalePanel);
            likertLock = true;
            overrideHideBackButton = true;
            likertScaleInteractionManager.SetPostLikertScaleValue();
            showPostLikert = false;
        }
        else
        {
            currentPanel.Show();
        }
        
        CurrentPanelButtonWrap();
    }

    void Update()
    {
        //Debug.Log(currentPanel.ToString());
        
        if(!device.isValid)
        {
            GetDevice();
        }

        // Hiding the back button if there is no menu history
        SetBackButtonState();
        // Hide settings menu button when the menu is hidden
        SettingsButtonState();

        // This functions can be done using the XR Interactions ranther than through update
        BackButtonPress();
    }

    private void OnDestroy()
    {
        
        Debug.Log("MenuManager was destroyed");
    }

    public void BackToMenu()
    {
		SceneLoader.Instance.ReturnToMenu();
    }

    void BackButtonPress()
    {
        // capturing secondary button press and release
        bool secondaryButtonValue = false;

        // Check if the app is running in the editor and presents Oculus based interactions, otherwise presents Pico controls
        InputFeatureUsage<bool> secondaryButtonUsage;
        //Oculus Secondary Button
        if(Application.isEditor){secondaryButtonUsage = CommonUsages.secondaryButton;}
        //Pico Menu button
        else{secondaryButtonUsage = CommonUsages.menuButton;}
        
        if (device.TryGetFeatureValue(secondaryButtonUsage, out secondaryButtonValue) && secondaryButtonValue && !secondaryButtonIsPressed)
        {
            //disabled the back button here
            secondaryButtonIsPressed = true;
            GoToPrevious();
        }
        else if (!secondaryButtonValue && secondaryButtonIsPressed)
        {
            secondaryButtonIsPressed = false;
        }
        else {
            return;
        }
    }

    public void GoToPrevious()
    {        
        if(likertLock) // check if the current panel is the post video likert scale, if so, don't go to previous
        {
            return;
        }
        else if(menuHidden == false && panelHistory.Count != 0)
        {
            int lastIndex = panelHistory.Count - 1;
            SetCurrent(panelHistory[lastIndex]);
            panelHistory.RemoveAt(lastIndex);
        }
        else if(panelHistory.Count == 0)
        {
            GoToTop();
        }
        else if(menuHidden == true)
        {
            ShowMenu();
        }
        else {
            //SavePanelHistory();
            return;
        }
    }

    public void GoToTop()
    {
        if(defaultPanel != null) {SetCurrent(defaultPanel);}
    }
 
    // Call to set current panel with history
    public void SetCurrentWithHistory(Panel newPanel)
    {
        panelHistory.Add(currentPanel);;
        SetCurrent(newPanel);
    }

    // Set the current panel using the new panel or call to make current panel without history
    public void SetCurrent(Panel newPanel)
    {
        currentPanel.Hide();
        currentPanel = newPanel;
        CurrentPanelButtonWrap();
        currentPanel.Show();
    }

    public void ToggleSettingsWithHistory(Panel newPanel)
    {
        if(currentPanel.ToString() == "Panel_Settings (Panel)")
        {
            GoToPrevious();
        }
        else
        {
            panelHistory.Add(currentPanel);
            SetCurrent(newPanel);
        }        
    }

    public void LoadScene(string level)
    {
        // A video button to selected to load to that video.

        // Check to see if the custom prompt should be shown and if a selection has already been made
        if(customPopup == true && customPopupGiven == false)
        {
            // Sets the current panel to audio prompt
            SetCurrentWithHistory(customPopupPanelLink);
            // Save the level string to use in a moment to load the video
            savedLevelString = level;
            // End: a user selects how to proceed which also recalls LoadScene
            return;
        }
        // Check to see if the audio prompt should be shown and if a selection has already been made
        else if (audioPrompt == true && audioPromptGiven == false)
        {
            // Sets the current panel to audio prompt
            SetCurrentWithHistory(audioPromptBoxPanel);
            // Save the level string to use in a moment to load the video
            savedLevelString = level;
            //Debug.Log("saved level as: " + savedLevelString);

            // End: a user selects their audio preference which also recalls LoadScene
            return;
        }
        // Check if the likert scale popup should be shown and if a selection has already been made
        else if(likertScalePopup == true && likertScaleGiven == false)
        {
            // Sets the current panel to the likert popup
            SetCurrentWithHistory(likertScaleInteractionManager.preVideoLikertScalePanel);
            // Save the level string to use in a moment to load the video
            savedLevelString = level;
            // Save the video string name to playerPrefs so that it can be written to JSON later
            likertScaleInteractionManager.LikertRecordVideoSceneString();

            // End: a user makes a selection and calls LoadScene again
            return;
        }
        // Checks if the audio prompt has been given
        else if (audioPromptGiven == true || likertScaleGiven == true)
        {    
            // Resets the bools
            audioPromptGiven = false;
            likertScaleGiven = false;
        }

        sceneLoading = true;
        SceneLoader.Instance.LoadNewScene(level);
        SavePanelHistory();
        panelHistory.Clear();   
    }

    public void playMuteLoadScene()
    {
        PlayerPrefs.SetInt("PlayMute", 1);
        audioPromptGiven = true;
        LoadScene(savedLevelString);
    }

    public void playNotMuteLoadScene()
    {
        PlayerPrefs.SetInt("PlayMute", 0);
        audioPromptGiven = true;
        LoadScene(savedLevelString);
    }

    public void LikertLoadScene()
    {
        likertScaleInteractionManager.UpdateLikertSliderValue();
        likertScaleGiven = true;
        // Set binary int to trigger the post video popup on return to the menu 0=no 1=yes
        PlayerPrefs.SetInt("ShowPostLikert", 1);
        LoadScene(savedLevelString);
    }

    public void LikertScaleFinish()
    {
        if(currentPanel == likertScaleInteractionManager.postVideoLikertScalePanel)
        {
            likertScaleInteractionManager.LikertWriteToJSON();
            likertScaleInteractionManager.LikertCompareCheck();
            likertLock = false;
            overrideHideBackButton = false;

            if(likertScaleInteractionManager.showReviewMessage == true)
            {
                SetCurrent(likertScaleInteractionManager.postLikertScaleMessagePanel);
            }
            else
            {
                GoToPrevious();
            }
        }
        else 
        {
            GoToPrevious();
        }
    }

    public void NighWindDownVoice()
    {
        customPopupGiven = true;
        LoadScene(savedLevelString);
    }

    public void NightWindDownAtmos(string atmosLevel)
    {
        customPopupGiven = true;
        LoadScene(atmosLevel);
    }

    public void PMRLevelSelect(string PMRLevelSelect)
    {
        savedLevelString = PMRLevelSelect;
    }

    public void PMRLevelLoad()
    {
        customPopupGiven = true;
        LoadScene(savedLevelString);
    }

    public void HideMenu()
    {
        menuHidden = true;
        
        if(menuHideButton) {menuHideButton.SetActive(false);}
        if(menuShowButton) {menuShowButton.SetActive(true);}
        SetBackButtonState();
        if(menuSettingsButton) {menuSettingsButton.SetActive(false);}

        currentPanel.Hide();
        if(persistentLogo) {persistentLogo.SetActive(false);}

    }

    public void ShowMenu()
    {
        menuHidden = false;
        
        if(menuHideButton) {menuHideButton.SetActive(true);}
        if(menuShowButton) {menuShowButton.SetActive(false);}
        SettingsButtonState();
        SetBackButtonState();

        currentPanel.Show();
        if(persistentLogo) {persistentLogo.SetActive(true);}
    }

    public void ClearHistory()
    {
        panelHistory.Clear();
        PlayerPrefs.DeleteKey("SavedCurrentPanel");
        PlayerPrefs.DeleteKey("SavedPanelHistory");
        PlayerPrefs.DeleteKey("SavedPanelHistoryCount");
        SetCurrent(defaultPanel);
    }

    void SetBackButtonState()
    {
        if(menuHidden || overrideHideBackButton)
        {
            if(menuBackButton) {menuBackButton.SetActive(false);}
            if(menuHomeButton) {menuHomeButton.SetActive(false);}

        }
        else if(panelHistory.Count == 0)
        {
            if(menuBackButton) {menuBackButton.SetActive(false);}
            if(menuHomeButton && currentPanel != defaultPanel && !sceneLoading) {menuHomeButton.SetActive(true);}
            else if (menuHomeButton && currentPanel == defaultPanel) {menuHomeButton.SetActive(false);}

        }
        else
        {
            if(menuBackButton) {menuBackButton.SetActive(true);}
            if(menuHomeButton) {menuHomeButton.SetActive(false);}
        }
    }

    void SettingsButtonState()
    { 
        if(menuHidden || overrideHideBackButton)
        {
            if(menuSettingsButton) {menuSettingsButton.SetActive(false);}
        }
        else
        {
            if(menuSettingsButton) {menuSettingsButton.SetActive(true);}
        }
    }

    void CurrentPanelButtonWrap()
    {
        // If active panel has a buttonWrap GameObject then apply the v controller value to the scrollbar value
        if (currentPanel.buttonWrap != null)
        {
            currentButtonWrap = currentPanel.buttonWrap;
        }
        else
        {
            return;
        }
    }

    void SavePanelHistory()
    {            
        // Get temp the current panel to string
        string currentPanelTemp = currentPanel.ToString();
        // Get temp the amount of items within the history count
        int panelHistoryCountTotal = panelHistory.Count - 1;
        
        // Check if the current panel is the custom popup
        if(currentPanelTemp.Contains("Panel_CustomPopups_")) 
        {
            // Save the panel before the audio popup as the current panel
            PlayerPrefs.SetString("SavedCurrentPanel", panelHistory[panelHistoryCountTotal].ToString());
        }
        // Check if the current panel is the audio popup
        else if(currentPanelTemp == "Panel_Audio (Panel)") 
        {
            // Save the panel before the audio popup as the current panel
            PlayerPrefs.SetString("SavedCurrentPanel", panelHistory[panelHistoryCountTotal].ToString());
        }
        // Check if there likert panel is current panel
        else if(currentPanelTemp == "Panel_Likert_Pre (Panel)")
        {
            // Check if the audio popup came before the likert scale
            if(panelHistory[panelHistoryCountTotal].ToString() == "Panel_Audio (Panel)") 
            {
                // Remove Audio panel from list history
                panelHistory.RemoveAt(panelHistoryCountTotal);
               
                // Save the panel before the audio popup as current
                PlayerPrefs.SetString("SavedCurrentPanel", panelHistory[panelHistoryCountTotal - 1].ToString());
            }
            else
            {
                // Otherwise save the panel before the Likert scale as current
                PlayerPrefs.SetString("SavedCurrentPanel", panelHistory[panelHistoryCountTotal].ToString());
            }
        }
        else
        {
            // Save the current panel to Player Prefs
            PlayerPrefs.SetString("SavedCurrentPanel", currentPanel.ToString());
        }

        // Save Panel history to Player Prefs history
        int panelHistoryCount = panelHistory.Count;
        for(int i = 0; i < panelHistoryCount; i++)
        {
            string panelString = panelHistory[i].ToString();
            PlayerPrefs.SetString("SavedPanelHistory" + i, panelString);
        }
        PlayerPrefs.SetInt("SavedPanelHistoryCount", panelHistoryCount);
    }

    void GetPanelHistoryMemory()
    {
        panelHistory.Clear();
        
        if (PlayerPrefs.GetString("SavedCurrentPanel") != "")
        {
            string savedCurrentPanelString = PlayerPrefs.GetString("SavedCurrentPanel"); // Take the saved string and hold it in a temp string
            savedCurrentPanelString = savedCurrentPanelString.Substring(0, savedCurrentPanelString.Length -8); // remove the " (Panel)" from the end of the string so it will relink correctly
            GameObject tempCurrentPanelGameObject = GameObject.Find(savedCurrentPanelString); // Find the GameObject of that string
            Panel savedCurrentPanel = tempCurrentPanelGameObject.GetComponent<Panel>(); // Find the Panel on that GameObject
            currentPanel = savedCurrentPanel; // Set the temp panel as the current panel
        }
        else
        {
            currentPanel = defaultPanel; // Otherwise use the defaultPanel given in the editor
        }
        
        int panelHistoryCount = PlayerPrefs.GetInt("SavedPanelHistoryCount"); // Set the history count from the player prefs
        for(int i = 0; i < panelHistoryCount; i++)
        {
            // Get the string from Player Prefs
            string panelString = PlayerPrefs.GetString("SavedPanelHistory" + i);
            // Remove the last 8 characters from the string
            panelString = panelString.Substring(0, panelString.Length - 8);
            // Use the string to search for the GameObject name
            GameObject tempGameObjectSearch = GameObject.Find(panelString);
            // Find the Panel component on that GameObject
            Panel panelLocation = tempGameObjectSearch.GetComponent<Panel>();
            // Add the Panel to the panelHistroy List
            panelHistory.Add(panelLocation);
        }

        // Check if the current panel is a panel that shouldn't be shown and jumps back until it is an acceptable pannel
        PopupCheckForPanelHistory();  
    }

    void PopupCheckForPanelHistory()
    {
        Debug.Log("Starting while loop check for current panel");
        while((currentPanel.ToString() == "Panel_Likert_Pre (Panel)") || (currentPanel.ToString() == "Panel_Likert_Post (Panel)") || (currentPanel.ToString() == "Panel_Likert_Message (Panel)") || (currentPanel.ToString() == "Panel_Audio (Panel)"))
        {
            Debug.Log("Current panel is identified as a one to be removed");
            // set current panel to the one before
            GoToPrevious();
            // Restart loop
        }
    }
}
