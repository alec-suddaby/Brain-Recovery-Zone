using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class VideoMenuManager : MonoBehaviour
{
    public Canvas selfCanvas;
    
    //XR Toolkit controllers
    [SerializeField]
    private XRNode xRNode = XRNode.LeftHand;

    private List<InputDevice> devices = new List<InputDevice>();

    private InputDevice device;

    public GameObject rotationOffset;

    //to avoid repeat readings
    private bool triggerButtonIsPressed;
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
        HideVideoControls();
    }

    void Update()
    {
        if(!device.isValid)
        {
            GetDevice();
        }

        //Setup back button again

        // capturing trigger button press and release
        bool triggerButtonValue = false;
        InputFeatureUsage<bool> triggerButtonUsage = CommonUsages.triggerButton;
        
        if (device.TryGetFeatureValue(triggerButtonUsage, out triggerButtonValue) && triggerButtonValue && !triggerButtonIsPressed)
        {
            //disabled the back button here
            triggerButtonIsPressed = true;
            ShowVideoControls();
        }
        else if (!triggerButtonValue && triggerButtonIsPressed /*add here an && trigger is in raycast target area */)
        {
            triggerButtonIsPressed = false;
        }
        else {
            return;
        }

        // capturing secondary button press and release
        bool secondaryButtonValue = false;
        InputFeatureUsage<bool> secondaryButtonUsage = CommonUsages.secondaryButton;
        
        if (device.TryGetFeatureValue(secondaryButtonUsage, out secondaryButtonValue) && secondaryButtonValue && !secondaryButtonIsPressed)
        {
            //disabled the back button here
            secondaryButtonIsPressed = true;
            SceneLoader.Instance.ReturnToMenu();
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
        //write some script that will send you back to the previous menu structure you were on
        return;

    }

    public void LoadScene(string level)
    {
        SceneLoader.Instance.LoadNewScene(level);
    }

    public void BackToMenu()
    {
        SceneLoader.Instance.ReturnToMenu();
    }

    public void HideVideoControls()
    {
        selfCanvas.enabled = false;
    }

    void ShowVideoControls()
    {
        selfCanvas.enabled = true;
        //rotationOffset.GetComponent<VideoControlsTrackRotate>().GetCameraRotatePosition();
    }
    
}
