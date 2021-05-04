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
    public bool menuVideoLoopCheck = false;

    // Audio popup
    [SerializeField]
    private MenuManager menuManager;
    [SerializeField]
    private bool audioPopup;

    // Likert Scale
    [SerializeField]
    private bool likertScalePopup;
    [SerializeField]
    private LikertScaleInteractionManager likertScaleInteractionManager;

    // Input Linked Video Bits
    [Header("Linked GameObjects")]
    public GameObject titleGameObject;
    public GameObject descriptionGameObject;
    public GameObject durationGameObject;
    public GameObject thumbnailGameObject;

   
    private TextMeshProUGUI titleTMP;
    private TextMeshProUGUI decriptionTMP;
    private TextMeshProUGUI durationTMP;
    private ProceduralImage thumbnailIMG;
       
    
    
    // Start is called before the first frame update
    void Start()
    {             
        //Set Title
        if ( titleGameObject != null)
        {   
            titleTMP = titleGameObject.GetComponent<TextMeshProUGUI>();
            titleTMP.text = videoTitle;
        }

        //Set Description
        if ( descriptionGameObject != null)
        {   
            decriptionTMP = descriptionGameObject.GetComponent<TextMeshProUGUI>();
            decriptionTMP.text = videoDescription;
        }
        
        //Set Duration
        if ( durationGameObject != null)
        {   
            durationTMP = durationGameObject.GetComponent<TextMeshProUGUI>();
            durationTMP.text = videoDuration;
        }

        //Set Thumbnail Image
        if ( thumbnailGameObject != null)
        {   
            thumbnailIMG = thumbnailGameObject.GetComponent<ProceduralImage>();
            thumbnailIMG.sprite = videoBackground;
        }
    }

    // Manually call this function when this video button is pressed. It will then send a save of the bool to the Persistent VR if the video should loop or not.
    public void SaveLoopPreferance()
    {
        // Loop video 1 = yes 0 = no
        if(menuVideoLoopCheck == true)
        {
            PlayerPrefs.SetInt("LoopVideo", 1);
        }
        else
        {
            PlayerPrefs.SetInt("LoopVideo", 0);
        }
    }

    public void TriggerAudioPrompt()
    {
        menuManager = FindObjectOfType<MenuManager>();

        if(menuManager)
            menuManager.audioPrompt = audioPopup;
            //Debug.Log ("Set prompt");
    }

    public void TriggerLikertPopup()
    {
        menuManager = FindObjectOfType<MenuManager>();
        if(menuManager)
        {
            menuManager.likertScalePopup = likertScalePopup;
            //Debug.Log ("Set prompt");
        }

        likertScaleInteractionManager = FindObjectOfType<LikertScaleInteractionManager>();
        if(likertScaleInteractionManager)
        {
            likertScaleInteractionManager.upcomingVideoTitle = videoTitle;
            //Debug.Log ("Set inbound video title");
        }
    }

}
