using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class MenuManager : MonoBehaviour
{
    public Panel currentPanel = null;
    public GameObject persistentLogo;

    public Component[] canvasInPanels;

    private List<Panel> panelHistory = new List<Panel>();

    //XR Toolkit controllers
    [SerializeField]
    private XRNode xRNode = XRNode.LeftHand;

    private List<InputDevice> devices = new List<InputDevice>();

    private InputDevice device;

    //to avoid repeat readings
    private bool secondaryButtonIsPressed;

    // Hide and show Game Objects
    [Header("Menu Tools")]
    //public string topPanelName;
    public GameObject menuBackButton;
    public GameObject menuHideButton;
    public GameObject menuShowButton;
    public GameObject menuSettingsButton;

    private bool menuHidden = false;

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
        ShowMenu();
        SetupPanels();
    }

    void SetupPanels()
    {
        Panel[] panels = GetComponentsInChildren<Panel>();
        
        foreach(Panel panel in panels)
            panel.Setup(this);

        currentPanel.Show();
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
            return;
        }
    }
 
    public void SetCurrentWithHistory(Panel newPanel)
    {
        panelHistory.Add(currentPanel);
        SetCurrent(newPanel);
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

    public void SetCurrent(Panel newPanel)
    {
        currentPanel.Hide();

        currentPanel = newPanel;
        currentPanel.Show();
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
}
