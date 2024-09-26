using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LikertScaleManager : MonoBehaviour
{
    [SerializeField] private LikertScale preLikertScale;
    [SerializeField] private LikertScale postLikertScale;

    private static float? preLikertValue = null;
    private static float? postLikertValue = null;
    private static string videoName = null;
    private static bool isActive = false;
    public bool IsActive => isActive;

    [SerializeField] private Elixr.MenuSystem.MenuManager menuManager;

    [Range(0, 5)]
    [SerializeField] private float fadeDuration = 1f;

    public void Start()
    {
        preLikertScale.Init(menuManager);
        postLikertScale.Init(menuManager, active: isActive);
    }

    public void Close()
    {
        StartCoroutine(menuManager.Fade(1, fadeDuration, true, startDelay: fadeDuration));
        preLikertScale.Display(fadeDuration, false, null, fadeDelay: 0);
        postLikertScale.Display(fadeDuration, false, null, fadeDelay: 0);
        isActive = false;
    }

    public void ShowPreLikertScale(UnityAction<float> eventOnComplete, VideoDescription videoDescription)
    {
        preLikertValue = null;
        postLikertValue = null;
        videoName = videoDescription.name;

        StartCoroutine(menuManager.Fade(0, fadeDuration, false));

        preLikertScale.Display(fadeDuration, true, eventOnComplete);
        preLikertScale.AddListener(SetPreLikertScaleValue);

        isActive = true;
    }

    public void SetPreLikertScaleValue(float value)
    {
        preLikertValue = value;

        isActive = false;
    }

    public void ShowPostLikertScale(UnityAction<float> eventOnComplete)
    {
        StartCoroutine(menuManager.Fade(0, 0, false));

        postLikertScale.Display(fadeDuration, true, eventOnComplete, preLikertValue);
        postLikertScale.AddListener(SetPostLikertScaleValue);

        isActive = true;
    }

    public void SetPostLikertScaleValue(float value)
    {
        postLikertValue = value;
        ToJSON();

        StartCoroutine(menuManager.Fade(1, fadeDuration));

        isActive = false;
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
}