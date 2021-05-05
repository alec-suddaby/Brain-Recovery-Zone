using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LikertScaleInteractionManager : MonoBehaviour
{
    [Header("Pre Video Likert Scale")]
    public Panel preVideoLikertScalePanel;
    public Slider preLikertSlider;
    //private float preLikertSliderValue;

    [Header("Post Video Likert Scale")]
    public Panel postVideoLikertScalePanel;
    public Slider postLikertSliderSaved;
    //private float postLikertSliderSavedValue;
    public Slider postLikertSlider;
    //private float postLikertSliderValue;

    [Header("Post Video Review Message")]
    public Panel postLikertScaleMessagePanel;
    public bool showReviewMessage = false;

    [Header("Additional Information")]
    // This string is set from the Video Button itself
    public string upcomingVideoTitle;

    void Start()
    {
        if(preLikertSlider != null)
        {
            preLikertSlider.value = 5f;
        }
        
        if(postLikertSlider != null)
        {
            postLikertSlider.value = 5f;
        }
    }

    
    // Compares the saved value against the given value
    public void LikertCompareCheck()
    {
        if(postLikertSlider.value <= postLikertSliderSaved.value)
        {
            // Display the message on continue click
            showReviewMessage = true;
        }
        else
        {
            // Skip showing the message
            showReviewMessage = false;
        }

        Debug.Log("Review message check: " + showReviewMessage);
    }

    // Call this function in OnValueChanged for each slider
    public void UpdateLikertSliderValue()
    {
        if(preLikertSlider != null && preVideoLikertScalePanel.canvas.enabled == true)
        {
            PlayerPrefs.SetFloat("PreLikert", preLikertSlider.value);
        }
        
        if(postLikertSlider != null && postVideoLikertScalePanel.canvas.enabled == true)
        {
            PlayerPrefs.SetFloat("PostLikert", postLikertSlider.value);
        }
    }

    // Called to set the post likert scale to show the value from the opening likert scale test
    public void SetPostLikertScaleValue()
    {
        Debug.Log("Setting value of slider to :" +  PlayerPrefs.GetFloat("PreLikert"));
        postLikertSliderSaved.value = PlayerPrefs.GetFloat("PreLikert");
    }

    // Called when the pre video Likert scale is presented so that it can record the scring name for the upcoming video
    public void LikertRecordVideoSceneString()
    {
        PlayerPrefs.SetString("LikertVideoTitle", upcomingVideoTitle);
    }

    public void LikertWriteToJSON()
    {
        Debug.Log("Begin Write Likert to JSON");

        StatisticsController.Instance.RecordLikertToJSON(PlayerPrefs.GetFloat("PreLikert"), PlayerPrefs.GetString("LikertVideoTitle"), PlayerPrefs.GetFloat("PostLikert"));

        Debug.Log("Video Name: " + PlayerPrefs.GetString("LikertVideoTitle"));
        //PlayerPrefs.GetString("LikertVideoTitle");

        Debug.Log("Starting Likert Score: " + PlayerPrefs.GetFloat("PreLikert"));
        //PlayerPrefs.GetFloat("PreLikert");

        Debug.Log("Ending Likert Score: " + PlayerPrefs.GetFloat("PostLikert"));
        //PlayerPrefs.GetFloat("PostLikert");

        Debug.Log("End Write Likert to JSON");
    }

    public void LikertClearSave()
    {
        upcomingVideoTitle = "";
        PlayerPrefs.SetString("LikertVideoTitle", "");
    }
}
