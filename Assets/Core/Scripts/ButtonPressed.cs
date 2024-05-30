using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Events;

public class ButtonPressed : MonoBehaviour
{
    public UnityEvent<bool> OnButtonPressed;
    private InputDevice controller;
    private bool isPressed = false;
    public bool lockOn = false;

    void Start()
    {
        var leftHandDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.LeftHand, leftHandDevices);
        if(leftHandDevices.Count > 0)
            controller = leftHandDevices[0];
    }

    void Update(){
        #if UNITY_EDITOR
        if(Input.GetMouseButtonDown(1)){
            OnButtonPressed.Invoke(true);
        }else if(Input.GetMouseButtonUp(1) && !lockOn){
            OnButtonPressed.Invoke(false);
        }
        #endif

        bool buttonPressed;

        //UnityEngine.XR.CommonUsages.primary2DAxisClick when trackpad pressed
        //UnityEngine.XR.CommonUsages.menuButton when button above Pico button pressed
        if(controller.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxisClick, out buttonPressed) && buttonPressed != isPressed){
            OnButtonPressed.Invoke(buttonPressed);
            isPressed = buttonPressed;
        }
    }
}
