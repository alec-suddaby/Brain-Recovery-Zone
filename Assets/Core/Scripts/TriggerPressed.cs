using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public class TriggerPressed : MonoBehaviour
{
    private InputDevice controller;
    private bool buttonPressed = false;
    public UnityEvent triggerPressed;
    // Start is called before the first frame update
    void Start()
    {
        var leftHandDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.LeftHand, leftHandDevices);
        if(leftHandDevices.Count > 0)
            controller = leftHandDevices[0];
    }

    // Update is called once per frame
    void Update()
    {
        #if UNITY_EDITOR
            if(Input.GetMouseButtonDown(0)){
                triggerPressed.Invoke();
            }
        #else
            if(controller != null){
                bool triggerValue;
                if (controller.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerValue) && triggerValue)
                {
                    if(!buttonPressed){
                        buttonPressed = true;
                        triggerPressed.Invoke();
                    }
                }else{
                    buttonPressed = false;
                }
            }
        #endif
    }
}
