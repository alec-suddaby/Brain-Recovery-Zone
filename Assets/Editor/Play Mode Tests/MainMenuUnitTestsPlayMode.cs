using NUnit.Framework;
using RenderHeads.Media.AVProVideo;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class MainMenuUnitTestsPlayMode
{
    [Test]
    public void LoadMainMenu_Test6()
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

    private MenuManager GetMenuManager()
    {
        LoadMainMenu_Test6();

        return GameObject.FindObjectOfType<MenuManager>();
    }

    private Panel[] GetPanels(MenuManager menuManager)
    {
        return menuManager.transform.GetComponentsInChildren<Panel>();
    }

    private Panel GetActivePanel(MenuManager menuManager)
    {
        Panel[] panels = GetPanels(menuManager);

        Panel activePanel = null;
        int activePanelCount = 0;
        foreach(Panel panel in panels)
        {
            if (panel.canvas.enabled)
            {
                activePanelCount++;
                activePanel = panel;
            }
        }

        if(activePanelCount != 1)
        {
            Assert.Fail($"{activePanelCount} panels active. Expected 1");
            return null;
        }

        return activePanel;
    }

    [Test]
    public void MainMenuPanelInitialisation_Test79()
    {
        MenuManager menuManager = GetMenuManager();

        if(menuManager == null)
        {
            Assert.Fail("No menu manager found");
            return;
        }

        Panel[] panels = GetPanels(menuManager);

        foreach(Panel panel in panels)
        {
            if(panel.MenuManager != menuManager)
            {
                Assert.Fail("Panels did not have MenuManager assigned correctly");
                return;
            }
        }

        Assert.Pass();
    }

    [UnityTest]
    public IEnumerator MainMenuButtonPressCheck_Test81()
    {
        MenuManager menuManager = GetMenuManager();

        Panel selectedPanel = GetActivePanel(menuManager);

        if(selectedPanel == null)
        {
            Assert.Fail("No menu panel was active");
            yield break;
        }

        MenuButtonDetails menuButton = selectedPanel.transform.GetComponentInChildren<MenuButtonDetails>();
        Button button = menuButton.GetComponent<Button>();
        button.onClick.Invoke();

        yield return new WaitForSecondsRealtime(2);

        if(selectedPanel == GetActivePanel(menuManager))
        {
            Assert.Fail("Menu did not change on button click");
            yield break;
        }

        Assert.Pass();
    }

    [Test]
    public void MainMenuButtonAudio_Test85()
    {
        MenuManager menuManager = GetMenuManager();
        Panel selectedPanel = GetActivePanel(menuManager);

        ButtonAudio buttonAudio = GameObject.FindObjectOfType<ButtonAudio>();
        AudioSource audioSource = buttonAudio.GetComponent<AudioSource>();

        MenuButtonDetails menuButton = selectedPanel.transform.GetComponentInChildren<MenuButtonDetails>();
        EventTrigger eventTrigger = menuButton.GetComponent<EventTrigger>();

        PointerEventData pointerEventData = new PointerEventData(GameObject.FindObjectOfType<EventSystem>());

        eventTrigger.OnPointerEnter(pointerEventData);
        Assert.IsTrue(buttonAudio.hoverSound == null || (audioSource.isPlaying && audioSource.clip == buttonAudio.hoverSound));

        eventTrigger.OnPointerClick(pointerEventData);
        Assert.IsTrue(buttonAudio.clickSound == null || (audioSource.isPlaying && audioSource.clip == buttonAudio.clickSound));
    }

    [Test]
    public void ConnectMainCameraTest_Test86()
    {
        LoadMainMenu_Test6();

        Canvas[] canvases = GameObject.FindObjectsOfType<Canvas>();
        foreach(Canvas canvas in canvases)
        {
            Assert.IsTrue(canvas.worldCamera != Camera.main);
        }

        MediaPlayer[] mediaPlayers = GameObject.FindObjectsOfType<MediaPlayer>();
        foreach (MediaPlayer mediaPlayer in mediaPlayers)
        {
            Assert.IsTrue(mediaPlayer.AudioHeadTransform == Camera.main.transform && mediaPlayer.AudioFocusTransform == Camera.main.transform);
        }

        UpdateStereoMaterial[] stereoMaterials = GameObject.FindObjectsOfType<UpdateStereoMaterial>();
        foreach (UpdateStereoMaterial stereoMaterial in stereoMaterials)
        {
            Assert.IsTrue(stereoMaterial._camera == Camera.main);
        }
    }

    [Test]
    public void MainMenuMenuButtonDetailsSetup_Test87()
    {
        MenuManager menuManager = GetMenuManager();

        Panel selectedPanel = GetActivePanel(menuManager);

        if (selectedPanel == null)
        {
            Assert.Fail("No menu panel was active");
            return;
        }

        MenuButtonDetails menuButton = selectedPanel.transform.GetComponentInChildren<MenuButtonDetails>();
        Assert.IsTrue(menuButton.titleGameObject.GetComponent<Text>().text == menuButton.menuTitle);
        Assert.IsTrue(menuButton.descriptionGameObject == null || menuButton.descriptionGameObject.GetComponent<Text>().text == menuButton.menuDescription);
    }

    [Test]
    public void ResetApplicationTest_Test88()
    {
        string preferenceName = "TestPreference";

        LoadMainMenu_Test6();
        ResetApp resetApp = GameObject.FindObjectOfType<ResetApp>();

        PlayerPrefs.SetInt(preferenceName, 0);
        PlayerPrefs.Save();

        resetApp.ResetAppFunction();

        Assert.IsFalse(PlayerPrefs.HasKey(preferenceName));
    }

    [UnityTest]
    public IEnumerator MainMenuSaveHistoryTest_Test82()
    {
        yield return MainMenuButtonPressCheck_Test81();

        MenuManager menuManager = GameObject.FindObjectOfType<MenuManager>();
        PlayerPrefs.DeleteAll();

        menuManager.SavePanelHistory();

        Assert.IsTrue(PlayerPrefs.HasKey("SavedPanelHistoryCount"));

        int historyCount = PlayerPrefs.GetInt("SavedPanelHistoryCount");

        for(int i = 0; i < historyCount; i++)
        {
            Assert.IsTrue(PlayerPrefs.HasKey("SavedPanelHistory" + i));
        }

        Assert.IsTrue(PlayerPrefs.HasKey("SavedCurrentPanel"));
    }

    [UnityTest]
    public IEnumerator MainMenuLoadHistoryTest_Test83()
    {
        yield return MainMenuSaveHistoryTest_Test82();

        string activePanelName = GetActivePanel(GameObject.FindObjectOfType<MenuManager>()).transform.name;

        MenuManager menuManager = GetMenuManager();
        Assert.IsTrue(activePanelName == GetActivePanel(menuManager).transform.name);
    }
}
