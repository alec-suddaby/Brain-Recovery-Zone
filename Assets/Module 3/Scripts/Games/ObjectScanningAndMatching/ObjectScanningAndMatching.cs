using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;
using TMPro;

public class ObjectScanningAndMatching : TimedTask
{
    public float timeBetweenSelectionChange = 1f;
    private float lastTimeChanged = 0f;

    public IconSelector mainSelector;
    public IconSelector userMatchingSelector;

    public bool autoSwitch;
    public LayerMask buttonLayer;
    public bool allowDeactivation;
    public AudioSource correctAudioSource;
    public AudioClip correctAudioClip;
    private InputDevice controller;
    private bool buttonReleased = true;

    public Transform controllerTransform;
    public TextMeshProUGUI scoreText;
    public Timer timer;

    protected override void Setup()
    {
        base.Setup();
        timeLimitEnabled = false;
        updateTick.AddListener(CheckUpdate);
        if(autoSwitch){
            

            mainSelector.Init();
            userMatchingSelector.Init();
        }
            
        userMatchingSelector.allowDeactivation = allowDeactivation;
        
        taskCompleted.AddListener(GetScore);

        #if !UNITY_EDITOR
            var leftHandDevices = new List<UnityEngine.XR.InputDevice>();
            UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.LeftHand, leftHandDevices);
            if(leftHandDevices.Count > 0)
                controller = leftHandDevices[0];
        #endif
    }

    void GetScore(){
        scoreText.text = timer.Score();
    }

    void CheckUpdate()
    {
        if(!beginTask){
            return;
        }

#if UNITY_EDITOR
        if(autoSwitch && Input.GetMouseButtonDown(0)){
            CheckForMatch();
        }

        if(!autoSwitch && Input.GetMouseButtonDown(0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Debug.DrawRay(ray.origin, ray.direction, Color.black);
            if(Physics.Raycast(ray, out hit, 10f, buttonLayer)){
                Icon icon = hit.transform.GetComponent<Icon>();
                icon.GetParentIconSelector.SelectIcon(icon);
                CheckForMatch();
            }
            Debug.Log(hit.transform.name);
        }
#else
        if(controller != null){
            bool triggerValue;
            if (!(controller.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerValue) && triggerValue))
            {
                buttonReleased = true;
            }

            if(triggerValue && buttonReleased){
                buttonReleased = false;

                if(autoSwitch){
                    CheckForMatch();
                }else{
                    Ray ray = new Ray(controllerTransform.position, controllerTransform.forward);
                    RaycastHit hit;
                    Debug.DrawRay(ray.origin, ray.direction, Color.black);
                    if(Physics.Raycast(ray, out hit, 10f, buttonLayer)){
                        Icon icon = hit.transform.GetComponent<Icon>();
                        icon.GetParentIconSelector.SelectIcon(icon);
                        CheckForMatch();
                    }
                    Debug.Log(hit.transform.name);
                }
            }
        }
#endif
        

        if(timeElapsed - lastTimeChanged > timeBetweenSelectionChange && autoSwitch){
            userMatchingSelector.Next();
            lastTimeChanged = timeElapsed;
        }
    }

    public void CheckForMatch(){
        bool incomplete = true;
        if(mainSelector.CurrentIconType == userMatchingSelector.CurrentIconType){
            if(!autoSwitch && (!mainSelector.GetIconSelected || !userMatchingSelector.GetIconSelected)){
                return;
            }
            incomplete = mainSelector.DeactivateCurrent(autoSwitch, allowDeactivation);
            userMatchingSelector.DeactivateCurrent(autoSwitch, allowDeactivation);

            if(!incomplete && allowDeactivation){
                if(!autoSwitch){
                    mainSelector.Next();
                    userMatchingSelector.Next();
                }
                mainSelector.DeactivateCurrent(false, allowDeactivation);
                userMatchingSelector.DeactivateCurrent(false, allowDeactivation);
            }

            correctAudioSource.PlayOneShot(correctAudioClip);
            lastTimeChanged = timeElapsed;
        }

        if(!incomplete){
            TaskCompleted();
        }
    }
}
