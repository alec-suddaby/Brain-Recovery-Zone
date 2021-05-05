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

    //Default Gradient
    private Gradient hiddenGradient;
    GradientColorKey[] colorKey;
    GradientAlphaKey[] alphaKey;

    private bool offsetSwitch = true;

    void Start()
    {
        videoControlsManager = gameObject.GetComponent<VideoControlsManager>();

        activeController = GameObject.Find("Controller");

        //Line Renderer
        savedGradient = activeController.GetComponent<XRInteractorLineVisual>().invalidColorGradient;
        
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
        if (videoControlsManager.videoControlsOn == false && videoControlsManager.returningToMenu == false)
        {
            hideControllerLine();   
        }
        else if (videoControlsManager.videoControlsOn == true && videoControlsManager.returningToMenu == false)
        {
            showControllerLine();
        }
    }

    private void hideControllerLine()
    {
        //Line Renderer
        activeController.GetComponent<XRInteractorLineVisual>().invalidColorGradient = hiddenGradient;
    }

    public void showControllerLine()
    {
        //Line Renderer
        activeController.GetComponent<XRInteractorLineVisual>().invalidColorGradient = savedGradient;
    }
}
