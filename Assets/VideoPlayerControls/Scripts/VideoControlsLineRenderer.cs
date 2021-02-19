using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class VideoControlsLineRenderer : MonoBehaviour
{
    private VideoControlsManager videoControlsManager;
    
    private GameObject activeController;
    private GameObject controllerModel;

    private Gradient savedGradient;
    private Vector3 savedControllerModelTransform;

    public string rightHandControllerString = "RightHand Controller";
    public string leftHandControllerString = "LeftHand Controller";

    //Default Gradient
    private Gradient hiddenGradient;
    GradientColorKey[] colorKey;
    GradientAlphaKey[] alphaKey;

    private bool offsetSwitch = true;

    void Awake()
    {
        videoControlsManager = gameObject.GetComponent<VideoControlsManager>();

        if(GameObject.Find(rightHandControllerString) != null)
            activeController = GameObject.Find(rightHandControllerString);
        else if(GameObject.Find(leftHandControllerString) != null)
            activeController = GameObject.Find(leftHandControllerString);
        else
            return;

        //Line Renderer
        savedGradient = activeController.GetComponent<XRInteractorLineVisual>().invalidColorGradient;
    }

    void Start()
    {
        //Line Renderer
        hiddenGradient = new Gradient();

        // Populate the color keys
        colorKey = new GradientColorKey[1];
        colorKey[0].color = Color.blue;

        // Populate the alpha  keys
        alphaKey = new GradientAlphaKey[1];
        alphaKey[0].alpha = 0f;

        hiddenGradient.SetKeys(colorKey, alphaKey);
    }

    // Update is called once per frame
    void Update()
    {
        if (videoControlsManager.videoControlsOn == false)
        {
            //Line Renderer
            activeController.GetComponent<XRInteractorLineVisual>().invalidColorGradient = hiddenGradient;       
        }
        else if (videoControlsManager.videoControlsOn == true)
        {
            //Line Renderer
            activeController.GetComponent<XRInteractorLineVisual>().invalidColorGradient = savedGradient;
        }
    }
}
