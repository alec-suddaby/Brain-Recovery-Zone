using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class HandSelection : MonoBehaviour
{
    [Header("XR Controller")]
    public XRNode masterXRNode = XRNode.LeftHand;

    [SerializeField] private XRController xRController;


    private void Start()
    {
        // Check if the app is running in the editor and presents Oculus based interactions, otherwise presents Pico controls
        if(Application.isEditor){masterXRNode = XRNode.RightHand;}
        else{masterXRNode = XRNode.LeftHand;}
        
        ChangeControllerHand();
    }

    public void ChangeControllerHand()
    {
        xRController = FindObjectOfType<XRController>();
        xRController.controllerNode = masterXRNode;
    }
}
