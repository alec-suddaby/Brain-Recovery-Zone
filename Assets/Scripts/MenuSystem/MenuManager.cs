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
    public GameObject menuBackButton;
    public GameObject menuHideButton;
    public GameObject menuShowButton;
    public GameObject menuSettingsButton;
    private bool menuHidden = false;

    // Current panel buttonWrap
    private GameObject currentButtonWrap;
    private float crrentButtonWrapWidth;
    
    [Header("Panel Navigation")]
    [Tooltip("Set this as your default panel to show on load")]
    private Panel currentPanel = null;
    public Panel defaultPanel = null;
    private Component[] canvasInPanels;
    private List<Panel> panelHistory = new List<Panel>();

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
    public GameObject audioPromptBox;
    private Panel audioPromptBoxPanel;
    private bool audioPromptGiven = false;
    private string savedLevelString;

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
        if(audioPromptBox) {audioPromptBoxPanel = audioPromptBox.GetComponent<Panel>();}

        PlayerPrefs.SetInt("PlayMute", 0);
        savedLevelString = "";
        
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

        GetPanelHistoryMemory();
        currentPanel.Show();
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
        SavePanelHistory();
        panelHistory.Clear();
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


        //Oculus Secondary Button
        //InputFeatureUsage<bool> secondaryButtonUsage = CommonUsages.secondaryButton;

        //Pico Menu button
        InputFeatureUsage<bool> secondaryButtonUsage = CommonUsages.menuButton;
        
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
        if(menuHidden == false && panelHistory.Count != 0)
        {
            int lastIndex = panelHistory.Count - 1;
            SetCurrent(panelHistory[lastIndex]);
            panelHistory.RemoveAt(lastIndex);
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
        if (audioPrompt == true && audioPromptGiven == false)
        {
            SetCurrent(audioPromptBoxPanel);
            savedLevelString = level;
            Debug.Log("saved level as: " + savedLevelString);
        }
        else if (audioPromptGiven == true)
        {
            audioPromptGiven = false;
            SceneLoader.Instance.LoadNewScene(level);
        }
        else
        { 
            SceneLoader.Instance.LoadNewScene(level);
        }   
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
        if(panelHistory.Count == 0 || menuHidden)
        {
            if(menuBackButton) {menuBackButton.SetActive(false);}
        }
        else
        {
            if(menuBackButton) {menuBackButton.SetActive(true);}
        }
    }

    void SettingsButtonState()
    { 
        if(menuHidden)
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
        // Save the current panel to Player Prefs
        PlayerPrefs.SetString("SavedCurrentPanel", currentPanel.ToString());

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
            string savedCurrentPanelString = PlayerPrefs.GetString("SavedCurrentPanel");
            savedCurrentPanelString = savedCurrentPanelString.Substring(0, savedCurrentPanelString.Length -8);
            GameObject tempCurrentPanelGameObject = GameObject.Find(savedCurrentPanelString);
            Panel savedCurrentPanel = tempCurrentPanelGameObject.GetComponent<Panel>();
            currentPanel = savedCurrentPanel;
        }
        else
        {
            currentPanel = defaultPanel;
        }
        
        int panelHistoryCount = PlayerPrefs.GetInt("SavedPanelHistoryCount");
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

        
        
    }
}
