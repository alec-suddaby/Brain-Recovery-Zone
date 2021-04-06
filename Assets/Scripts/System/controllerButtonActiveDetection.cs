using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;

public class controllerButtonActiveDetection : MonoBehaviour
{
    private HandSelection masterHandSelection;
    public XRNode checkAgainstXRNode = XRNode.LeftHand;


    // Start is called before the first frame update
    void Start()
    {
        //Get Hand Selection
        masterHandSelection = FindObjectOfType<HandSelection>();
    }

    // Update is called once per frame
    void Update()
    {
        if (masterHandSelection.masterXRNode == checkAgainstXRNode)
        {
            gameObject.GetComponent<Button>().interactable = false;
        }
        else
        {
            gameObject.GetComponent<Button>().interactable = true;
        }
    }

    public void SetXRNodeLeft()
    {
        masterHandSelection.masterXRNode = XRNode.LeftHand;
        masterHandSelection.ChangeControllerHand();
    }

    public void SetXRNodeRight()
    {
        masterHandSelection.masterXRNode = XRNode.RightHand;
        masterHandSelection.ChangeControllerHand();
    }
}
