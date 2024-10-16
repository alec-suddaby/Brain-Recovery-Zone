using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RenderHeads.Media.AVProVideo;

public class ConnectToMainCamera : MonoBehaviour
{
    private Canvas findCanvas;
    private UpdateStereoMaterial findStereoMaterial;
    private MediaPlayer findMediaPlayer;
    
    void Awake()
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
       }

       //Debug.Log("end of camera search");

    }
}
