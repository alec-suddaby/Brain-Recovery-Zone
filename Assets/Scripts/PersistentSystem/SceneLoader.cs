using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    public UnityEvent OnLoadBegin = new UnityEvent();
    public UnityEvent OnLoadEnd = new UnityEvent();
    public ScreenFader screenFader = null;

    public string mainMenuName;
    private string sceneName;

    public bool IsLoading { get; private set; } = false;

    // Deleted player prefs temp holding
    private string deletedSavedCurrentPanel;
    private int deletedSavedPanelHistoryCount;
    private List<string> deletedSavedPanelHistory = new List<string>();
    private int deleteShowPostLikert;

    private void Awake()
    {
        if (!Application.isEditor)
        {
            LoadPersistent();
        }

        StartCoroutine(StartupFade());
        SceneManager.sceneLoaded += SetActiveScene;
    }

    private void OnDestroy()
    {
        Debug.Log("Scene Loader Destroy");
        SceneManager.sceneLoaded -= SetActiveScene;
    }

    private void OnApplicationPause()
    {
        Debug.Log("Application is paused and player prefs have been cleared");
        ClearPlayerPrefPanelHistory();
    }

    private void OnApplicationFocus()
    {
        Debug.Log("Application is running and player prefs have been reloaded");
        ReloadPlayerPrefPanelHistory();
    }

    public void LoadPersistent()
    {
        SceneManager.LoadSceneAsync(mainMenuName, LoadSceneMode.Additive);
    }

    // sceneName is the string for the name of the new scene
    public void ReturnToMenu()
    {
        sceneName = mainMenuName;
        if (!IsLoading)
        {
            StartCoroutine(LoadScene(sceneName));
        }
    }

    // sceneName is the string for the name of the new scene
    public void LoadNewScene(string sceneName)
    {
        if (!IsLoading)
        {
            StartCoroutine(LoadScene(sceneName));
        }
    }

    private IEnumerator LoadScene(string sceneName)
    {
        IsLoading = true;

        //OnLoadBegin?.Invoke();
        yield return screenFader.StartFadeIn();
        yield return StartCoroutine(UnloadCurrent());

        // Fake wait time
        //yield return new WaitForSeconds(1.0f);

        yield return StartCoroutine(LoadNew(sceneName));
        yield return screenFader.StartFadeOut();
        //OnLoadEnd?.Invoke();

        //yield return new WaitForSeconds(0.5f);

        IsLoading = false;
    }

    private IEnumerator StartupFade()
    {
        //OnLoadBegin?.Invoke();
        //yield return screenFader.StartFadeIn();
        //yield return StartCoroutine(UnloadCurrent());

        // Fake wait time
        //yield return new WaitForSeconds(1.0f);

        // yield return StartCoroutine(LoadNew(sceneName));
        yield return screenFader.FirstFadeUp();
        //OnLoadEnd?.Invoke();

        //yield return new WaitForSeconds(0.5f);

        //isLoading = false;
    }

    private IEnumerator UnloadCurrent()
    {
        if(SceneManager.GetActiveScene() == null || SceneManager.sceneCount == 1)
        {
            yield break;
        }

        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

        //Can add the progress loading bar here for async progress

        while (!unloadOperation.isDone)
        {
            yield return null;
        }
    }

    private IEnumerator LoadNew(string sceneName)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!loadOperation.isDone)
        {
            yield return null;
        }
    }

    private void SetActiveScene(Scene scene, LoadSceneMode mode)
    {
        SceneManager.SetActiveScene(scene);

    }

    private void ClearPlayerPrefPanelHistory()
    {
        // Save player prefs in temp placeholder incase the app has just paused
        deletedSavedCurrentPanel = PlayerPrefs.GetString("SavedCurrentPanel");
        deletedSavedPanelHistoryCount = PlayerPrefs.GetInt("SavedPlayerHistoryCount");
        for (int i = 0; i < deletedSavedPanelHistoryCount; i++)
        {
            string panelString = PlayerPrefs.GetString("SavedPanelHistory" + i);
            deletedSavedPanelHistory.Add(panelString);
        }
        deleteShowPostLikert = PlayerPrefs.GetInt("ShowPostLikert");

        // Delete player prefs
        PlayerPrefs.DeleteKey("SavedCurrentPanel");
        PlayerPrefs.DeleteKey("SavedPanelHistory");
        PlayerPrefs.DeleteKey("SavedPanelHistoryCount");
        PlayerPrefs.DeleteKey("ShowPostLikert");
        Debug.Log("History Cleared from palyer prefs in scene loader script");
    }

    private void ReloadPlayerPrefPanelHistory()
    {
        PlayerPrefs.SetString("SavedCurrentPanel", deletedSavedCurrentPanel);
        PlayerPrefs.SetInt("SavedPlayerHistoryCount", deletedSavedPanelHistoryCount);
        for (int i = 0; i < deletedSavedPanelHistoryCount; i++)
        {
            string panelString = deletedSavedPanelHistory[i];
            PlayerPrefs.SetString("SavedPanelHistory" + i, panelString);
        }
        PlayerPrefs.SetInt("ShowPostLikert", deleteShowPostLikert);

        deletedSavedCurrentPanel = "";
        deletedSavedPanelHistoryCount = 0;
        deletedSavedPanelHistory.Clear();
        deleteShowPostLikert = 0;

    }




}

