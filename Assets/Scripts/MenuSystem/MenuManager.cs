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

    //Current panel scrollbar
    private GameObject currentScrollBar;
    
    [Header("Panel Navigation")]
    [Tooltip("Set this as your default panel to show on load")]
    [SerializeField]
    public Panel currentPanel = null;
    [SerializeField]
    public Component[] canvasInPanels;
    [SerializeField]
    public List<Panel> panelHistory = new List<Panel>();
    //Panel history debug
    private string panelList = "List Panel History: ";
    private string savedPanelList = "Saved List Panel History: ";

    [Header("Saved Panel Navigation")]
    public PreviousPanelMemory previousPanelMemory;
    public Panel savedCurrentPanel = null;
    public List<Panel> savedPanelHistory;

    [Header("XR Controller")]
    //WIll aim to remove or update this function
    //XR Toolkit controllers
    [SerializeField]
    private XRNode xRNode = XRNode.LeftHand;
    private List<InputDevice> devices = new List<InputDevice>();
    private InputDevice device;

    //to avoid repeat readings
    private bool secondaryButtonIsPressed;
    private bool primary2DAxisIsChosen;
    private Vector2 primary2DAxisValue = Vector2.zero;
    private Vector2 prevPrimary2DAxisValue;

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
        previousPanelMemory = FindObjectOfType<PreviousPanelMemory>();
        
        ShowMenu();
        SetupPanels();
    }

    void SetupPanels()
    {
        Panel[] panels = GetComponentsInChildren<Panel>();
        
        foreach(Panel panel in panels)
            panel.Setup(this);

        // triggering the saved histry to convert from saved string to panels
        previousPanelMemory.SavedFromString();

        // Checking if there is a saved panel and setting it to current
        if (previousPanelMemory.savedPanel != null)
        {
            currentPanel = previousPanelMemory.savedPanel;
        }

        // Checking if there is a saved panel history and setting it to current
        if (panelHistory.Count == 0 && previousPanelMemory.savedPanelListString.Count != 0)
        {
            panelHistory = previousPanelMemory.savedPanelList;
        }
        
        currentPanel.Show();

        CurrentPanelScrollbar(); 
    }

    void Update()
    {
        //Debug.Log(currentPanel.ToString());
        
        if(!device.isValid)
        {
            GetDevice();
        }

        SetBackButtonState();
        SettingsButtonState();

        // capturing primary 2D Axis changes and release
        InputFeatureUsage<Vector2> primary2DAxisUsage = CommonUsages.primary2DAxis;
        // make sure the value is not zero and that it has changed
        if (primary2DAxisValue != prevPrimary2DAxisValue)
        {
            primary2DAxisIsChosen = false;
            //Debug.Log($"CHANGED and prev value is {prevPrimary2DAxisValue} and the new value is {primary2DAxisValue}");
        }
        // was for checking to see if the axis values were reading as changed properly
        /* else
        {
            Debug.Log($"Nope, prev value is {prevPrimary2DAxisValue} and the new value is {primary2DAxisValue}");
        } */
        if (device.TryGetFeatureValue(primary2DAxisUsage, out primary2DAxisValue) && primary2DAxisValue != Vector2.zero && !primary2DAxisIsChosen)
        {
            prevPrimary2DAxisValue = primary2DAxisValue;
            primary2DAxisIsChosen = true;  
            //Debug.Log($"Primary2DAxis value activated {primary2DAxisValue} on {xRNode}");
        }
        else if (primary2DAxisValue == Vector2.zero && primary2DAxisIsChosen)
        {
            prevPrimary2DAxisValue = primary2DAxisValue;
            primary2DAxisIsChosen = false;
            //Debug.Log($"Primary2DAxis deactivated {primary2DAxisValue} on {xRNode}");
        }

        if (currentScrollBar != null)
        {
            // Calculating the rate of which to scroll. Currently this may be uneven depending on different lengths of scroll as the length of scroll is always 0 to 1 so the factor added to it will always be the same. Need to calculate a way of adding a % of 100 instead of a single number
            currentScrollBar.GetComponent<Scrollbar>().value = currentScrollBar.GetComponent<Scrollbar>().value + (primary2DAxisValue.x * 0.01f) ;
        }
        

        // capturing secondary button press and release
        bool secondaryButtonValue = false;
        InputFeatureUsage<bool> secondaryButtonUsage = CommonUsages.secondaryButton;
        
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

    private void OnDestroy()
    {
        previousPanelMemory.SavedToString();
        panelHistory.Clear();
        Debug.Log("MenuManager was destroyed");
    }

    public void GoToPrevious()
    {        
        if(menuHidden == false && panelHistory.Count != 0)
        {
            int lastIndex = panelHistory.Count - 1;
            SetCurrent(panelHistory[lastIndex]);
            panelHistory.RemoveAt(lastIndex);
            DebugList();
            SavePanelHistory();
        }
        else if(menuHidden == true)
        {
            ShowMenu();
        }
        else {
            DebugList();
            SavePanelHistory();
            return;
        }
    }
 
    public void SetCurrentWithHistory(Panel newPanel)
    {
        panelHistory.Add(currentPanel);
        SetCurrent(newPanel);
        DebugList();
        SavePanelHistory();
    }

    public void SetCurrent(Panel newPanel)
    {
        currentPanel.Hide();

        currentPanel = newPanel;

        CurrentPanelScrollbar();

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
        SceneLoader.Instance.LoadNewScene(level);
    }

    public void HideMenu()
    {
        menuHidden = true;
        
        menuHideButton.SetActive(false);
        menuShowButton.SetActive(true);
        SetBackButtonState();
        menuSettingsButton.SetActive(false);

        currentPanel.Hide();
        persistentLogo.SetActive(false);

    }

    public void ShowMenu()
    {
        menuHidden = false;
        
        menuHideButton.SetActive(true);
        menuShowButton.SetActive(false);
        SettingsButtonState();
        SetBackButtonState();

        currentPanel.Show();
        persistentLogo.SetActive(true);
    }

    void SetBackButtonState()
    {
        if(panelHistory.Count == 0 || menuHidden)
        {
            menuBackButton.SetActive(false);
        }
        else
        {
            menuBackButton.SetActive(true);
        }
    }

    void SettingsButtonState()
    { 
        if(menuHidden)
        {
            menuSettingsButton.SetActive(false);
        }
        else
        {
            menuSettingsButton.SetActive(true);
        }
    }

    void CurrentPanelScrollbar()
    {
        // If active panel has a scrollbar GameObject then apply the v controller value to the scrollbar value
        if (currentPanel.scrollBar != null)
        {
            //Debug.Log("Current Panel Has Scrollbar");

            currentScrollBar = currentPanel.scrollBar;
        }
        else if (currentPanel.scrollBar == null)
        {
            //Debug.Log("Current Panel No Scrollbar");
            return;
        }
        else
        {
            //Debug.Log("Current Panel Scrollbar is buggered");
            return;
        }
    }

    void DebugList()
    {
        Debug.Log("Current Panel: " + currentPanel.ToString());

        foreach (var panel in panelHistory)
        {
            panelList += panel.ToString() + ", ";
        }

        Debug.Log(panelList);
    }

    void SavePanelHistory()
    {
        savedCurrentPanel = currentPanel;
        previousPanelMemory.savedPanel = currentPanel;
        Debug.Log("Saved Current Panel: " + savedCurrentPanel.ToString());
        
        
        savedPanelHistory = panelHistory;
        previousPanelMemory.savedPanelList = panelHistory;
        foreach (var panel in savedPanelHistory)
        {
            savedPanelList += panel.ToString() + ", ";
        }

        //Debug.Log("String list from Menu Manager: " + savedPanelList);

        
        
        
        
        
    }
}
