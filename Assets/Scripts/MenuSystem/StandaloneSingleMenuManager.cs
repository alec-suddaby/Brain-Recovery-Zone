using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.Events;

public class StandaloneSingleMenuManager : MonoBehaviour
{

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
        //Get Hand Selection
        masterHandSelection = FindObjectOfType<HandSelection>();
        xRNode = masterHandSelection.masterXRNode;
    }

    void Update()
    {
        //Debug.Log(currentPanel.ToString());
        
        if(!device.isValid)
        {
            GetDevice();
        }

        // This functions can be done using the XR Interactions ranther than through update
        BackButtonPress();

    }

    private void OnDestroy()
    {

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
            BackToMenu();
        }
        else if (!secondaryButtonValue && secondaryButtonIsPressed)
        {
            secondaryButtonIsPressed = false;
        }
        else {
            return;
        }
    }

    public void LoadScene(string level)
    {
        SceneLoader.Instance.LoadNewScene(level);
    }
     
}
