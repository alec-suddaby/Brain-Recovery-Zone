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
        OpenScene("Videos/01_Discover/01_Discover_01_Brain");
    }

    //////////////
    //   Practise
    //////////////

    [MenuItem("Scenes/Practise/")]

    //////////////
    //   Calm
    //////////////

    [MenuItem("Scenes/Calm/SpaceAnimation")]
    public static void OpenCalmSpace()
    {
        OpenScene("Videos/03_Calm/03_Calm_01_SpaceAnimation");
    }

    [MenuItem("Scenes/Calm/AshdownForest")]
    public static void OpenCalmAshdown()
    {
        OpenScene("Videos/03_Calm/03_Calm_02_AshdownForest");
    }

    [MenuItem("Scenes/Calm/AshdownForestCopse")]
    public static void OpenCalmCopse()
    {
        OpenScene("Videos/03_Calm/03_Calm_03_AshdownForestCopse");
    }

    [MenuItem("Scenes/Calm/AshdownForestBench")]
    public static void OpenCalmBench()
    {
        OpenScene("Videos/03_Calm/03_Calm_04_AshdownForestBench");
    }
    [MenuItem("Scenes/Calm/AshdownForestSunset")]
    public static void OpenCalmSunset()
    {
        OpenScene("Videos/03_Calm/03_Calm_05_AshdownForestSunset");
    }

    [MenuItem("Scenes/Calm/NorthernLights")]
    public static void OpenNorthernLights()
    {
        OpenScene("Videos/03_Calm/03_Calm_06_NorthernLights");
    }

    [MenuItem("Scenes/Calm/Beach")]
    public static void OpenBeach()
    {
        OpenScene("Videos/03_Calm/03_Calm_07_Beach");
    }

    [MenuItem("Scenes/Calm/Lakeside")]
    public static void OpenLakeside()
    {
        OpenScene("Videos/03_Calm/03_Calm_08_Lakeside");
    }

    [MenuItem("Scenes/Calm/Mountain")]
    public static void OpenMountain()
    {
        OpenScene("Videos/03_Calm/03_Calm_09_Mountain");
    }

    [MenuItem("Scenes/Calm/Waterfall")]
    public static void OpenWaterfall()
    {
        OpenScene("Videos/03_Calm/03_Calm_10_Waterfall");
    }

    [MenuItem("Scenes/Calm/MindfulBreathing")]
    public static void OpenMindfulBreathing()
    {
        OpenScene("Videos/03_Calm/03_Calm_11_MindfulBreathing");
    }
    
    [MenuItem("Scenes/Calm/UnderwaterWorld")]
    public static void OpenUnderwaterWorld()
    {
        OpenScene("Videos/03_Calm/03_Calm_12_UnderwaterWorld");
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