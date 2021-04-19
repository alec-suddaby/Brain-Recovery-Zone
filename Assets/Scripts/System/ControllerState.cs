using UnityEngine;
using TMPro;
using UnityEngine.XR;
using Unity.XR.PXR;

public class ControllerState : MonoBehaviour
{
    public TextMeshProUGUI controllerStateText;
    //public TextMeshProUGUI controllerBatteryText;

    private string connectedController = "None";

    void Update()
    {   
        //Debug.Log("GetDominantHand: " + PXR_Input.GetDominantHand());
        //Debug.Log(PXR_Input.IsControllerConnected(PXR_Input.Controller.LeftController));
        //Debug.Log("GetActiveController: " + PXR_Input.GetActiveController());

        if(PXR_Input.IsControllerConnected(PXR_Input.Controller.LeftController))
        {
            connectedController = "Left";
        }
        else if (PXR_Input.IsControllerConnected(PXR_Input.Controller.RightController))
        {
            connectedController = "Right";
        }
        else
        {
            connectedController = "None";
        }

        
        controllerStateText.text =
        "IsControllerConnected: " + connectedController + "\n" +
        "GetDominantHand: " + PXR_Input.GetDominantHand() + "\n" +
        "GetActiveController: " + PXR_Input.GetActiveController() + "\n"
        ;


    }

    public void SetRightController()
    {
        PXR_Input.SetDominantHand(PXR_Input.Controller.RightController);
    }

    public void SetLeftController()
    {
        PXR_Input.SetDominantHand(PXR_Input.Controller.LeftController);
    }
}
