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

    private List<GameObject> panelPopupBox = new List<GameObject>();

    private List<Panel> panelHistory = new List<Panel>();

    private Button[] allButtons;

    //XR Toolkit controllers
    [SerializeField]
    private XRNode xRNode = XRNode.LeftHand;

    private List<InputDevice> devices = new List<InputDevice>();

    private InputDevice device;

    //to avoid repeat readings
    private bool secondaryButtonIsPressed;

    // Hide and show Game Objects
    [Header("Menu Tools")]
    public string topPanelName;
    public GameObject menuBackButton;
    public GameObject menuHideButton;
    public GameObject menuShowButton;
    public GameObject menuSettingsButton;

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
        SetBackButtonState();
        SettingsButtonState();
        
        if(!device.isValid)
        {
            GetDevice();
        }

        // capturing primary button press and release
        bool secondaryButtonValue = false;
        InputFeatureUsage<bool> secondaryButtonUsage = CommonUsages.secondaryButton;
        
        //if (OVRInput.GetDown(OVRInput.Button.Back) || OVRInput.GetDown(OVRInput.Button.Two))
        if (device.TryGetFeatureValue(secondaryButtonUsage, out secondaryButtonValue) && secondaryButtonValue && !secondaryButtonIsPressed)
        {
            /*disabled the back button here*/
            //GoToPrevious();
        }
    }

    public void GoToPrevious()
    {
        //Debug.Log("Start Back Press");
        
        if(panelHistory.Count == 0)
        //execute function if there is no panel histroy, for example exiting the app? else..... return;
        {
            //OVRManager.PlatformUIConfirmQuit();
            return;
        }

        int lastIndex = panelHistory.Count - 1;
        SetCurrent(panelHistory[lastIndex]);
        panelHistory.RemoveAt(lastIndex);
    }
 
    public void SetCurrentWithHistory(Panel newPanel)
    {
        panelHistory.Add(currentPanel);
        SetCurrent(newPanel);
    }

    public void SetCurrentChildWithHistory(Panel newPanel)
    {
        panelHistory.Add(currentPanel);
        SetCurrentChild(newPanel);
    }

    public void SetCurrent(Panel newPanel)
    {
        currentPanel.Hide();

        currentPanel = newPanel;
        currentPanel.Show();
    }

    public void SetCurrentChild(Panel newPanel)
    {
        currentPanel = newPanel;
        currentPanel.Show();
    }

    public void LoadScene(string level)
    {
        SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive);
    }

    void SetBackButtonState()
    {
        if(currentPanel.ToString() == topPanelName+" (Panel)")
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
        if(currentPanel.ToString() == "Panel_Settings (Panel)")
        {
            menuSettingsButton.SetActive(false);
        }
        else
        {
            menuSettingsButton.SetActive(true);
        }
    }

    public void HideMenu()
    {
        menuHideButton.SetActive(false);
        menuShowButton.SetActive(true);
        menuBackButton.SetActive(false);
        menuSettingsButton.SetActive(false);
        gameObject.SetActive(false);
    }

    public void ShowMenu()
    {
        menuHideButton.SetActive(true);
        menuShowButton.SetActive(false);
        SettingsButtonState();
        SetBackButtonState();
        gameObject.SetActive(true);
    }
}
