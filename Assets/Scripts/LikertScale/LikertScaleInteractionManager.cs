using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LikertScaleInteractionManager : MonoBehaviour
{
    [Header("Pre Video Likert Scale")]
    public Panel preVideoLikertScalePanel;
    public Slider preLikertSlider;
    private float preLikertSliderValue;

    [Header("Post Video Likert Scale")]
    public Panel postVideoLikertScalePanel;
    public Slider postLikertSliderSaved;
    private float postLikertSliderSavedValue;
    public Slider postLikertSlider;
    private float postLikertSliderValue;

    [Header("Post Video Review Message")]
    public Panel postLikertScaleMessagePanel;
    private bool showReviewMessage = false;

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
        if(postLikertSliderValue >= postLikertSliderSavedValue)
        {
            // Display the message on continue click
            showReviewMessage = true;
        }
        else
        {
            // Skip showing the message
            showReviewMessage = false;
        }
    }

    // Call this function in OnValueChanged for each slider
    public void UpdateLikertSliderValue()
    {
        if(preLikertSlider != null)
        {
            preLikertSliderValue = preLikertSlider.value;
            PlayerPrefs.SetFloat("PreLikert", preLikertSliderValue);
        }
        
        if(postLikertSlider != null)
        {
            postLikertSliderValue = postLikertSlider.value;
            PlayerPrefs.SetFloat("PostLikert", postLikertSliderValue);
        }
    }

    // Called when the pre video Likert scale is presented so that it can record the scring name for the upcoming video
    public void LikertRecordVideoSceneString()
    {
        PlayerPrefs.SetString("LikertVideoTitle", upcomingVideoTitle);
    }
}
