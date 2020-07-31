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
            Debug.Log("####################PRESSSSEDDDDD");
            GoToPrevious();
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
}
