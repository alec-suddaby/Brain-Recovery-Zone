using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI.ProceduralImage;

[ExecuteInEditMode]
public class VideoButtonDetails : MonoBehaviour
{
    // Edit Video Button Details
    [Header("Video Button Details")]
    public string videoTitle = "Video Title";
    [TextArea]
    public string videoDescription = "Video description";
    public string videoDuration = "x mins"; 
    public Sprite videoBackground; 


    // Input Linked Video Bits
    [Header("Linked GameObjects")]
    public GameObject titleGameObject;
    public GameObject descriptionGameObject;
    public GameObject durationGameObject;
    public GameObject thumbnailGameObject;

    [SerializeField]
    private TextMeshProUGUI titleTMP;

    [SerializeField]
    private TextMeshProUGUI decriptionTMP;

    [SerializeField]
    private TextMeshProUGUI durationTMP;

    [SerializeField]
    private ProceduralImage thumbnailIMG;
       
    
    
    // Start is called before the first frame update
    void Start()
    {
        //Set Title
        titleTMP = titleGameObject.GetComponent<TextMeshProUGUI>();
        titleTMP.text = videoTitle;


        //Set Description
        decriptionTMP = descriptionGameObject.GetComponent<TextMeshProUGUI>();
        decriptionTMP.text = videoDescription;

        
        //Set Duration
        durationTMP = durationGameObject.GetComponent<TextMeshProUGUI>();
        durationTMP.text = videoDuration;


        //Set Thumbnail Image
        thumbnailIMG = thumbnailGameObject.GetComponent<ProceduralImage>();
        thumbnailIMG.sprite = videoBackground;
    }

}
