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
        ChangeControllerHand();
    }

    public void ChangeControllerHand()
    {
        xRController = FindObjectOfType<XRController>();
        xRController.controllerNode = masterXRNode;
    }
}
