using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RenderHeads.Media.AVProVideo;

public class ConnectMainCamera : MonoBehaviour
{
    [TextArea]
    public string Description = "V1.0 This script will, on awake, link the Canvas, UpdateStereoMaterial or MediaPlayer to the main camera when placed on the same GameObject";
    
    private Canvas findCanvas;
    private UpdateStereoMaterial findStereoMaterial;
    private MediaPlayer findMediaPlayer;
    
    void Start()
    {
        if(GetComponent<Canvas>() != null)
       {
            //Debug.Log("Looking for Main Camera for Canvas");
            findCanvas = GetComponent<Canvas>();
            findCanvas.worldCamera = Camera.main;
       }

        if(GetComponent<UpdateStereoMaterial>() != null)
       {
            //Debug.Log("Looking for Main Camera for UpdateStereoMaterial");
            findStereoMaterial = GetComponent<UpdateStereoMaterial>();
            findStereoMaterial._camera = Camera.main;
       }

        if(GetComponent<MediaPlayer>() != null)
       {
           //Debug.Log("Looking for Main Camera for MediaPlayer");
           findMediaPlayer = GetComponent<MediaPlayer>();
           findMediaPlayer.AudioHeadTransform = Camera.main.transform;
           findMediaPlayer.AudioFocusTransform = Camera.main.transform;
       }

        if(GetComponent<UpdateStereoMaterial>() != null)
       {
           //Debug.Log("Looking for Main Camera for Update Stereo Material");
           findStereoMaterial = GetComponent<UpdateStereoMaterial>();
           findStereoMaterial._camera = Camera.main;
       }

       //Debug.Log("end of camera search");

    }
}
