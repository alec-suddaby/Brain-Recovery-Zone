using NUnit.Framework;
using RenderHeads.Media.AVProVideo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class VideoSceneTests
{
    private const string headsetAssetsPath = "file://mnt/sdcard";
    private const string desktopAssetsPath = "D://Brain Recovery Zone";

    private IEnumerator LoadScene(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(StaticReferences.PersistentVRSceneName, LoadSceneMode.Single);
        yield return SceneManager.LoadSceneAsync(sceneName, new LoadSceneParameters(LoadSceneMode.Additive));
    }

    private IEnumerator CheckVideoPlaysOnLoad(string sceneName)
    {
        LogAssert.ignoreFailingMessages = true;
        yield return LoadScene(sceneName);
        MediaPlayer player = GameObject.FindObjectOfType<MediaPlayer>();

        Assert.IsNotNull(player, "Media player not found");

        string videoPath = player.m_VideoPath;

        videoPath = videoPath.Replace(headsetAssetsPath, desktopAssetsPath);
        Assert.IsTrue(File.Exists(videoPath), "File not found");
        Assert.IsTrue(player.m_AutoOpen, "Video not set to open automatically");
        Assert.IsTrue(player.m_AutoStart, "Video not set to play automatically");

        LogAssert.ignoreFailingMessages = false;
        //Assert.IsTrue(player.Control.IsPlaying());
        //Assert.IsTrue(player.VideoOpened);
    }

    [UnityTest]
    public IEnumerator BrainVideoPlayerTest_Test30()
    {
        yield return CheckVideoPlaysOnLoad("01_Discover_01_Brain");
    }

    [UnityTest]
    public IEnumerator StrokeVideoPlayerTest_Test31()
    {
        yield return CheckVideoPlaysOnLoad("01_Discover_02_Stroke");
    }

    [UnityTest]
    public IEnumerator FatigueVideoPlayerTest_Test32()
    {
        yield return CheckVideoPlaysOnLoad("01_Discover_03_Fatigue");
    }

    [UnityTest]
    public IEnumerator AttentionTrainingLevel1VideoPlayerTest_Test34()
    {
        yield return CheckVideoPlaysOnLoad("02_Practice_02_AttentionTraining_Level1");
    }

    [UnityTest]
    public IEnumerator AttentionTrainingLevel2VideoPlayerTest_Test35()
    {
        yield return CheckVideoPlaysOnLoad("02_Practice_02_AttentionTraining_Level2");
    }

    [UnityTest]
    public IEnumerator AttentionTrainingLevel3VideoPlayerTest_Test36()
    {
        yield return CheckVideoPlaysOnLoad("02_Practice_02_AttentionTraining_Level3");
    }

    [UnityTest]
    public IEnumerator HundredAcreWoodVideoPlayerTest_Test37()
    {
        yield return CheckVideoPlaysOnLoad("03_Calm_02_AshdownForest");
    }

    [UnityTest]
    public IEnumerator CopseVideoPlayerTest_Test38()
    {
        yield return CheckVideoPlaysOnLoad("03_Calm_03_AshdownForestCopse");
    }

    [UnityTest]
    public IEnumerator ForestLandscapeVideoPlayerTest_Test39()
    {
        yield return CheckVideoPlaysOnLoad("03_Calm_04_AshdownForestBench");
    }

    [UnityTest]
    public IEnumerator SunsetForestVideoPlayerTest_Test40()
    {
        yield return CheckVideoPlaysOnLoad("03_Calm_05_AshdownForestSunset");
    }

    [UnityTest]
    public IEnumerator PrivateBeachVideoPlayerTest_Test41()
    {
        yield return CheckVideoPlaysOnLoad("03_Calm_07_Beach");
    }

    [UnityTest]
    public IEnumerator LakesideVideoPlayerTest_Test42()
    {
        yield return CheckVideoPlaysOnLoad("03_Calm_08_Lakeside");
    }

    [UnityTest]
    public IEnumerator MountainVideoPlayerTest_Test43()
    {
        yield return CheckVideoPlaysOnLoad("03_Calm_09_Mountain");
    }

    [UnityTest]
    public IEnumerator WaterfallVideoPlayerTest_Test44()
    {
        yield return CheckVideoPlaysOnLoad("03_Calm_10_Waterfall");
    }

    [UnityTest]
    public IEnumerator UnderwaterWorldVideoPlayerTest_Test45()
    {
        yield return CheckVideoPlaysOnLoad("03_Calm_12_UnderwaterWorld");
    }

    [UnityTest]
    public IEnumerator MindfullBreathingVideoPlayerTest_Test46()
    {
        yield return CheckVideoPlaysOnLoad("03_Calm_11_MindfulBreathing");
    }

    [UnityTest]
    public IEnumerator BreathingAndMuscleRelaxationVideoPlayerTest_Test47()
    {
        yield return CheckVideoPlaysOnLoad("03_Calm_14A_PMRLevel1");
    }

    [UnityTest]
    public IEnumerator FlightThroughSpaceVideoPlayerTest_Test48()
    {
        yield return CheckVideoPlaysOnLoad("03_Calm_15_FlyThroughSpace");
    }

    [UnityTest]
    public IEnumerator NorthernLightsVideoPlayerTest_Test49()
    {
        yield return CheckVideoPlaysOnLoad("03_Calm_06_NorthernLights");
    }

    [UnityTest]
    public IEnumerator NightTimeWindDownVideoPlayerTest_Test50()
    {
        yield return CheckVideoPlaysOnLoad("03_Calm_13_SleepWindDown");
    }

    [UnityTest]
    public IEnumerator ImmersiveGuidedMeditationVideoPlayerTest_Test51()
    {
        yield return CheckVideoPlaysOnLoad("03_Calm_01_SpaceAnimation");
    }
}
