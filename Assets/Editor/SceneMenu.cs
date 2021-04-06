﻿using UnityEditor;
using UnityEditor.SceneManagement;

public static class SceneMenu
{
    //////////////
    //   Main Menu
    //////////////
    
    [MenuItem("Scenes/MainMenu/Menu")]
    public static void OpenMenu()
    {
        OpenScene("MainMenuAddative");
    }

    //////////////
    //   Discover
    //////////////

    [MenuItem("Scenes/Discover/Brain")]
    public static void OpenDiscoverBrain()
    {
        OpenScene("Experiences/01_Discover/01_Discover_01_Brain");
    }

    //////////////
    //   Practise
    //////////////

    [MenuItem("Scenes/Practise/Communication/Menu")]
    public static void OpenPractiseCommunication()
    {
        OpenScene("Experiences/02_Practice/02_Practice_01_Communication");
    }

    [MenuItem("Scenes/Practise/Communication/Introduction")]
    public static void OpenPractiseCommunicationIntroduction()
    {
        OpenScene("Experiences/02_Practice/02_Practice_01_Communication/02_Practice_01_Communication_01_Introduction");
    }

    [MenuItem("Scenes/Practise/Communication/Respiration")]
    public static void OpenPractiseCommunicationRespiration()
    {
        OpenScene("Experiences/02_Practice/02_Practice_01_Communication/02_Practice_01_Communication_02_Respiration_Phonation");
    }

    [MenuItem("Scenes/Practise/Communication/Articulation")]
    public static void OpenPractiseCommunicationArticulation()
    {
        OpenScene("Experiences/02_Practice/02_Practice_01_Communication/02_Practice_01_Communication_03_Articulation");
    }

    [MenuItem("Scenes/Practise/Communication/Prosody")]
    public static void OpenPractiseCommunicationProsody()
    {
        OpenScene("Experiences/02_Practice/02_Practice_01_Communication/02_Practice_01_Communication_04_Prosody");
    }

    [MenuItem("Scenes/Practise/Communication/OtherStrats")]
    public static void OpenPractiseCommunicationOtherStrats()
    {
        OpenScene("Experiences/02_Practice/02_Practice_01_Communication/02_Practice_01_Communication_05_Other_Strategies");
    }

    [MenuItem("Scenes/Practise/AttentionTraining/Menu")]
    public static void OpenPractiseAttentionTraining()
    {
        OpenScene("Experiences/02_Practice/02_Practice_02_AttentionTraining");
    }

    [MenuItem("Scenes/Practise/AttentionTraining/Mild")]
    public static void OpenPractiseAttentionTrainingMild()
    {
        OpenScene("Experiences/02_Practice/02_Practice_02_AttentionTraining/02_Practice_02_AttentionTraining_01_Mild");
    }

    [MenuItem("Scenes/Practise/AttentionTraining/Moderate")]
    public static void OpenPractiseAttentionTrainingModerate()
    {
        OpenScene("Experiences/02_Practice/02_Practice_02_AttentionTraining/02_Practice_02_AttentionTraining_02_Moderate");
    }

    [MenuItem("Scenes/Practise/AttentionTraining/Severe")]
    public static void OpenPractiseAttentionTrainingSevere()
    {
        OpenScene("Experiences/02_Practice/02_Practice_02_AttentionTraining/02_Practice_02_AttentionTraining_03_Severe");
    }

    //////////////
    //   Calm
    //////////////

    [MenuItem("Scenes/Calm/SpaceAnimation")]
    public static void OpenCalmSpace()
    {
        OpenScene("Experiences/03_Calm/03_Calm_01_SpaceAnimation");
    }

    [MenuItem("Scenes/Calm/AshdownForest")]
    public static void OpenCalmAshdown()
    {
        OpenScene("Experiences/03_Calm/03_Calm_02_AshdownForest");
    }

    [MenuItem("Scenes/Calm/AshdownForestCopse")]
    public static void OpenCalmCopse()
    {
        OpenScene("Experiences/03_Calm/03_Calm_03_AshdownForestCopse");
    }

    [MenuItem("Scenes/Calm/AshdownForestBench")]
    public static void OpenCalmBench()
    {
        OpenScene("Experiences/03_Calm/03_Calm_04_AshdownForestBench");
    }
    [MenuItem("Scenes/Calm/AshdownForestSunset")]
    public static void OpenCalmSunset()
    {
        OpenScene("Experiences/03_Calm/03_Calm_05_AshdownForestSunset");
    }

    [MenuItem("Scenes/Calm/NorthernLights")]
    public static void OpenNorthernLights()
    {
        OpenScene("Experiences/03_Calm/03_Calm_06_NorthernLights");
    }

    [MenuItem("Scenes/Calm/Beach")]
    public static void OpenBeach()
    {
        OpenScene("Experiences/03_Calm/03_Calm_07_Beach");
    }

    [MenuItem("Scenes/Calm/Lakeside")]
    public static void OpenLakeside()
    {
        OpenScene("Experiences/03_Calm/03_Calm_08_Lakeside");
    }

    [MenuItem("Scenes/Calm/Mountain")]
    public static void OpenMountain()
    {
        OpenScene("Experiences/03_Calm/03_Calm_09_Mountain");
    }

    [MenuItem("Scenes/Calm/Waterfall")]
    public static void OpenWaterfall()
    {
        OpenScene("Experiences/03_Calm/03_Calm_10_Waterfall");
    }

    [MenuItem("Scenes/Calm/MindfulBreathing")]
    public static void OpenMindfulBreathing()
    {
        OpenScene("Experiences/03_Calm/03_Calm_11_MindfulBreathing");
    }
    
    [MenuItem("Scenes/Calm/UnderwaterWorld")]
    public static void OpenUnderwaterWorld()
    {
        OpenScene("Experiences/03_Calm/03_Calm_12_UnderwaterWorld");
    }


    //////////////
    //   Solo
    //////////////

    [MenuItem("Scenes/TestVideo")]
    public static void OpenGame()
    {
        OpenScene("PersistentVideoSample");
    }

    private static void OpenScene(string sceneName)
    {
        EditorSceneManager.OpenScene("Assets/Scenes/AddativeScenes/PersistentVR.unity", OpenSceneMode.Single);
        EditorSceneManager.OpenScene("Assets/Scenes/AddativeScenes/" + sceneName + ".unity", OpenSceneMode.Additive);
    }
}