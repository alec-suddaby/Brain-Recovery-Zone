using NUnit.Framework;
using RenderHeads.Media.AVProVideo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class LikertScaleTests
{

    private LikertScaleInteractionManager GetLikertScaleInteractionManager()
    {
        MainMenuUnitTestsPlayMode mainMenuTests = new MainMenuUnitTestsPlayMode();
        mainMenuTests.LoadMainMenu_Test6();

        return GameObject.FindObjectOfType<LikertScaleInteractionManager>();
    }

    [Test]
    public void LoadPostLikertScaleTest_Test84()
    {
        PlayerPrefs.SetInt("ShowPostLikert", 1);

        LikertScaleInteractionManager likertScaleInteractionManager = GetLikertScaleInteractionManager();

        Assert.IsTrue(likertScaleInteractionManager.postVideoLikertScalePanel.canvas.enabled);
    }

    [Test]
    public void LikertScaleTemporarySavePreExerciseTest_Test92()
    {
        LikertScaleInteractionManager likertManager = GetLikertScaleInteractionManager();
        likertManager.preVideoLikertScalePanel.canvas.enabled = true;

        float increment = (likertManager.preLikertSlider.maxValue - likertManager.preLikertSlider.minValue) / 10f;
        for (float i = likertManager.preLikertSlider.minValue; i <= likertManager.preLikertSlider.maxValue; i += increment)
        {
            likertManager.preLikertSlider.value = i;
            likertManager.UpdateLikertSliderValue();

            Assert.IsTrue(PlayerPrefs.GetFloat("PreLikert") == i);
        }
    }

    [Test]
    public void LikertScaleTemporarySavePostExerciseTest_Test92()
    {
        LikertScaleInteractionManager likertManager = GetLikertScaleInteractionManager();
        likertManager.postVideoLikertScalePanel.canvas.enabled = true;

        float increment = (likertManager.postLikertSlider.maxValue - likertManager.postLikertSlider.minValue) / 10f;
        for (float i = likertManager.postLikertSlider.minValue; i <= likertManager.postLikertSlider.maxValue; i += increment)
        {
            likertManager.postLikertSlider.value = i;
            likertManager.UpdateLikertSliderValue();

            Assert.IsTrue(PlayerPrefs.GetFloat("PostLikert") == i);
        }
    }

    private void TestLikertSlider(Canvas canvas, Slider slider, string playerPref)
    {
        canvas.enabled = true;
        float value = Random.Range(slider.minValue, slider.minValue);
        slider.onValueChanged.Invoke(value);
        Assert.IsTrue(PlayerPrefs.GetFloat(playerPref) == value);
    }

    private void SetLikertScaleValue(Slider slider, float value)
    {
        slider.value = value;
        slider.onValueChanged.Invoke(value);
    }

    [Test]
    public void LikertScaleValueUpdated_Test94()
    {
        LikertScaleInteractionManager likertManager = GetLikertScaleInteractionManager();

        TestLikertSlider(likertManager.preVideoLikertScalePanel.canvas, likertManager.preLikertSlider, "PreLikert");
        TestLikertSlider(likertManager.postVideoLikertScalePanel.canvas, likertManager.postLikertSlider, "PostLikert");
    }

    [Test]
    public void LikertScalePostExerciseMessageShownCorrectly_Test95()
    {
        LikertScaleInteractionManager likertManager = GetLikertScaleInteractionManager();
        MenuManager menuManager = GameObject.FindObjectOfType<MenuManager>();

        SetLikertScaleValue(likertManager.preLikertSlider, 1);
        SetLikertScaleValue(likertManager.postLikertSlider, 2);

        menuManager.LikertScaleFinish();
        Assert.IsTrue(!likertManager.postLikertScaleMessagePanel.canvas.enabled);

        SetLikertScaleValue(likertManager.preLikertSlider, 2);
        SetLikertScaleValue(likertManager.postLikertSlider, 1);

        menuManager.LikertScaleFinish();
        Assert.IsTrue(likertManager.postLikertScaleMessagePanel.canvas.enabled);
    }
}
