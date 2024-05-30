using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(TimedTask))]
public class TriggerPressCounter : MonoBehaviour
{
    private bool countButtonPresses = false;
    [SerializeField]
    private List<float> buttonPressTimes = new List<float>();

    public List<float> GetButtonPresses{
        get => buttonPressTimes.ToList();
    }

    private InputDevice controller;
    private bool buttonPressed = false;
    public TimedTask timedTask;

    public void CountButtonPresses(bool count){
        countButtonPresses = count;
    }

    void Start(){
        var leftHandDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.LeftHand, leftHandDevices);
        if(leftHandDevices.Count > 0)
            controller = leftHandDevices[0];
    }

    void Update(){
        if(!countButtonPresses){
            return;
        }

        #if UNITY_EDITOR
            if(Input.GetMouseButtonDown(0)){
                AddButtonPress();
            }
        #else
            if(controller != null){
                bool triggerValue;
                if (controller.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerValue) && triggerValue)
                {
                    if(!buttonPressed){
                        buttonPressed = true;
                        buttonPressTimes.Add(timedTask.GetTimeElapsed);
                    }
                }else{
                    buttonPressed = false;
                }
            }
        #endif
    }

    public void AddButtonPress()
    {
        buttonPressTimes.Add(timedTask.GetTimeElapsed);
    }
}
