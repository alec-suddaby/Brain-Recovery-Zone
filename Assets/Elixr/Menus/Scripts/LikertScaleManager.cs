using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LikertScaleManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup panel;
    public CanvasGroup Panel => panel;

    [SerializeField] private LikertScale preLikertScale;
    [SerializeField] private LikertScale postLikertScale;

    private static float? preLikertValue = null;
    private static float? postLikertValue = null;
    private static string videoName = null;

    private VideoDescription videoDescription = null;

    private LikertScaleMemory memory => GameObject.FindObjectOfType<LikertScaleMemory>();
    public bool IsActive
    {
        get
        {
            return memory.active;
        }

        set
        {
            memory.active = value;
        }
    }

    [SerializeField] private Elixr.MenuSystem.MenuManager menuManager;

    private LikertScale activeScale = null;
    public LikertScale ActiveScale => activeScale;

    public void Start()
    {
        activeScale = preLikertScale;

        Debug.Log("Init Pre Likert Scale Manager");
        preLikertScale.Init(menuManager);
        Debug.Log("Init Post Likert Scale Manager");
        postLikertScale.Init(menuManager);
        Debug.Log("Init Likert Scale Complete");

        if (IsActive)
        {
            ShowPostLikertScale((float f) => { });
        }
    }

    public void Close()
    {
        StartCoroutine(menuManager.Fade(1, menuManager.transitionTime, true));
        preLikertScale.Display(menuManager.transitionTime, false, null, fadeDelay: 0);
        postLikertScale.Display(menuManager.transitionTime, false, null, fadeDelay: 0);
        IsActive = false;
    }

    public void ShowPreLikertScale(UnityAction<float> eventOnComplete, VideoDescription videoDescription)
    {
        activeScale = preLikertScale;

        preLikertValue = null;
        postLikertValue = null;
        videoName = videoDescription.name;
        this.videoDescription = videoDescription;

        StartCoroutine(menuManager.Fade(0, menuManager.transitionTime, false));

        preLikertScale.Display(menuManager.transitionTime, true, eventOnComplete);
        preLikertScale.AddListener(SetPreLikertScaleValue);

        IsActive = true;
    }

    public void SetPreLikertScaleValue(float value)
    {
        preLikertValue = value;
    }

    public void ShowPostLikertScale(UnityAction<float> eventOnComplete)
    {
        activeScale = postLikertScale;

        StartCoroutine(menuManager.Fade(0, 0, false));

        postLikertScale.Display(0, true, eventOnComplete, preLikertValue);
        postLikertScale.AddListener(SetPostLikertScaleValue);

        IsActive = true;
    }

    public void SetPostLikertScaleValue(float value)
    {
        postLikertValue = value;
        ToJSON();

        IsActive = false;

        postLikertScale.Display(menuManager.transitionTime, false);

        menuManager.menus.blackboard.ReloadMenus(menuManager);
        //menuManager.Fade(1, menuManager.transitionTime);
    }

    public void ToJSON()
    {
        if(preLikertValue == null)
        {
            Debug.Log("Pre-likert value was null. Unable to save Likert to JSON");
            return;
        }

        if (postLikertValue == null)
        {
            Debug.Log("Post-likert value was null. Unable to save Likert to JSON");
            return;
        }

        if(videoName == null)
        {
            Debug.Log("Video name value was null. Unable to save Likert to JSON");
            return;
        }

        Debug.Log("Begin Write Likert to JSON");

        StatisticsController.Instance.RecordLikertToJSON((float)preLikertValue, videoName, (float)postLikertValue);

        Debug.Log("Video Name: " + videoName);

        Debug.Log("Starting Likert Score: " + preLikertValue);

        Debug.Log("Ending Likert Score: " + postLikertValue);

        Debug.Log("End Write Likert to JSON");
    }

    public void LoadVideo(float value)
    {
        Debug.Log("Load video");
        FindObjectOfType<SceneLoader>().LoadNewScene(videoDescription.VideoScene);
    }
}