using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI.ProceduralImage;

public class VideoButtonDetailsTests
{
    private VideoButtonDetails CreateVideoButton(TextMeshProUGUI titleText, string title, TextMeshProUGUI descriptionText, string description, TextMeshProUGUI durationText, string duration, ProceduralImage backgroundImage, Sprite icon)
    {
        GameObject gameObject = new GameObject();
        VideoButtonDetails videoButton = gameObject.AddComponent<VideoButtonDetails>();
        videoButton.videoTitle = title;
        videoButton.videoDescription = description;
        videoButton.videoDuration = duration;
        videoButton.videoBackground = icon;

        videoButton.titleGameObject = titleText.gameObject;
        videoButton.descriptionGameObject = descriptionText.gameObject;
        videoButton.durationGameObject = durationText.gameObject;
        videoButton.thumbnailGameObject = backgroundImage.gameObject;

        return videoButton;
    }

    private TextMeshProUGUI CreateText()
    {
        GameObject text = new GameObject();
        return text.AddComponent<TextMeshProUGUI>();
    }

    private ProceduralImage CreateProceduralImage()
    {
        GameObject text = new GameObject();
        return text.AddComponent<ProceduralImage>();
    }

    private Sprite CreateSprite()
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, Color.red);
        texture.Apply();

        return Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0, 0));
    }

    [Test]
    public void VideoButtonDetailsSetup_Test97()
    {
        string title = "Test Title";
        string description = "Test Description";
        string duration = "Test Duration";
        Sprite sprite = CreateSprite();

        TextMeshProUGUI titleText = CreateText();
        TextMeshProUGUI descriptionText = CreateText();
        TextMeshProUGUI durationText = CreateText();
        ProceduralImage backgroundImage = CreateProceduralImage();
        VideoButtonDetails videoButton = CreateVideoButton(titleText, title, descriptionText, description, durationText, duration, backgroundImage, sprite);

        videoButton.Init();

        Assert.IsTrue(videoButton.titleGameObject.GetComponent<TextMeshProUGUI>().text == title);
        Assert.IsTrue(videoButton.descriptionGameObject.GetComponent<TextMeshProUGUI>().text == description);
        Assert.IsTrue(videoButton.durationGameObject.GetComponent<TextMeshProUGUI>().text == duration);
        Assert.IsTrue(backgroundImage.sprite == sprite);
    }

    public void LoadMainMenu()
    {
        try
        {
            SceneManager.LoadScene(StaticReferences.PersistentVRSceneName);
            SceneManager.LoadScene(StaticReferences.MainMenuSceneName, LoadSceneMode.Additive);
        }
        catch (Exception ex)
        {
            Assert.Fail(ex.Message);
            return;
        }

        Assert.Pass("Scenes loaded successfully");
    }

    [Test]
    public void VideoButtonDetailsTriggerAudioPrompt_Test98()
    {
        LoadMainMenu();
        MenuManager menuManager = GameObject.FindObjectOfType<MenuManager>();

        Type videoButtonType = typeof(VideoButtonDetails);
        FieldInfo fieldInfo = videoButtonType.GetField("audioPopup", BindingFlags.NonPublic | BindingFlags.Instance);
        VideoButtonDetails videoButtonDetails = GameObject.FindObjectsOfType<VideoButtonDetails>().First(x => (bool)fieldInfo.GetValue(x));
        EventSystem eventSystem = GameObject.FindObjectOfType<EventSystem>();

        if(videoButtonDetails == null)
        {
            Assert.Pass("No audio popups in use");
            return;
        }

        Transform panelTransform = videoButtonDetails.transform;
        Panel panel = null;
        while((panel = panelTransform.GetComponent<Panel>()) == null) 
        { 
            if(panelTransform.parent == null)
            {
                Assert.Fail("Couldn't find panel");
                return;
            }

            panelTransform = panelTransform.parent;
        }

        menuManager.SetCurrent(panel);
        videoButtonDetails.GetComponent<EventTrigger>().OnPointerClick(new PointerEventData(eventSystem));
        Assert.IsTrue(menuManager.audioPromptBoxPanel.canvas.enabled);
    }

    [Test]
    public void VideoButtonDetailsTriggerLikertPrompt_Test99()
    {
        LoadMainMenu();
        MenuManager menuManager = GameObject.FindObjectOfType<MenuManager>();

        Type videoButtonType = typeof(VideoButtonDetails);
        FieldInfo audioPromptField = videoButtonType.GetField("audioPopup", BindingFlags.NonPublic | BindingFlags.Instance);
        FieldInfo likertPromptField = videoButtonType.GetField("likertScalePopup", BindingFlags.NonPublic | BindingFlags.Instance);
        VideoButtonDetails videoButtonDetails = GameObject.FindObjectsOfType<VideoButtonDetails>().First(x => !(bool)audioPromptField.GetValue(x) && (bool)likertPromptField.GetValue(x));
        EventSystem eventSystem = GameObject.FindObjectOfType<EventSystem>();

        if (videoButtonDetails == null)
        {
            Assert.Pass("No likert popups in use");
            return;
        }

        Transform panelTransform = videoButtonDetails.transform;
        Panel panel = null;
        while ((panel = panelTransform.GetComponent<Panel>()) == null)
        {
            if (panelTransform.parent == null)
            {
                Assert.Fail("Couldn't find panel");
                return;
            }

            panelTransform = panelTransform.parent;
        }

        menuManager.SetCurrent(panel);
        videoButtonDetails.GetComponent<EventTrigger>().OnPointerClick(new PointerEventData(eventSystem));
        Assert.IsTrue(menuManager.likertScaleInteractionManager.preVideoLikertScalePanel.canvas.enabled);
    }

    [Test]
    public void VideoButtonDetailsTriggerCustomPrompt_Test100()
    {
        LoadMainMenu();
        MenuManager menuManager = GameObject.FindObjectOfType<MenuManager>();

        Type videoButtonType = typeof(VideoButtonDetails);
        FieldInfo audioPromptField = videoButtonType.GetField("audioPopup", BindingFlags.NonPublic | BindingFlags.Instance);
        FieldInfo likertPromptField = videoButtonType.GetField("likertScalePopup", BindingFlags.NonPublic | BindingFlags.Instance);
        FieldInfo customPromptField = videoButtonType.GetField("customPopup", BindingFlags.NonPublic | BindingFlags.Instance);
        VideoButtonDetails videoButtonDetails = GameObject.FindObjectsOfType<VideoButtonDetails>().First(x => !(bool)audioPromptField.GetValue(x) && !(bool)likertPromptField.GetValue(x) && (bool) customPromptField.GetValue(x));
        EventSystem eventSystem = GameObject.FindObjectOfType<EventSystem>();

        if (videoButtonDetails == null)
        {
            Assert.Pass("No custom popups");
            return;
        }

        Transform panelTransform = videoButtonDetails.transform;
        Panel panel = null;
        while ((panel = panelTransform.GetComponent<Panel>()) == null)
        {
            if (panelTransform.parent == null)
            {
                Assert.Fail("Couldn't find panel");
                return;
            }

            panelTransform = panelTransform.parent;
        }

        menuManager.SetCurrent(panel);
        videoButtonDetails.GetComponent<EventTrigger>().OnPointerClick(new PointerEventData(eventSystem));
        Assert.IsTrue(videoButtonDetails.customPopupPanel.canvas.enabled);
    }

    [Test]
    public void VideoButtonDetailsSaveLoopPreferences_Test101()
    {
        string title = "Test Title";
        string description = "Test Description";
        string duration = "Test Duration";
        Sprite sprite = CreateSprite();

        TextMeshProUGUI titleText = CreateText();
        TextMeshProUGUI descriptionText = CreateText();
        TextMeshProUGUI durationText = CreateText();
        ProceduralImage backgroundImage = CreateProceduralImage();
        VideoButtonDetails videoButton = CreateVideoButton(titleText, title, descriptionText, description, durationText, duration, backgroundImage, sprite);

        videoButton.Init();

        videoButton.menuVideoLoopCheck = true;
        videoButton.SaveLoopPreferance();
        Assert.IsTrue(PlayerPrefs.GetInt("LoopVideo", 0) == 1);

        videoButton.menuVideoLoopCheck = false;
        videoButton.SaveLoopPreferance();
        Assert.IsTrue(PlayerPrefs.GetInt("LoopVideo", 1) == 0);
    }
}
