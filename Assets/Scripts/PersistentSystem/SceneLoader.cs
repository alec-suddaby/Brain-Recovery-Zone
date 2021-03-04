using System.Collections;
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

    private bool isLoading = false;
    
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
      SceneManager.sceneLoaded -= SetActiveScene;
    }

    private void LoadPersistent()
    {
        SceneManager.LoadSceneAsync(mainMenuName, LoadSceneMode.Additive);
    }

    // sceneName is the string for the name of the new scene
    public void ReturnToMenu()
    {
        sceneName = mainMenuName;
        if(!isLoading)
        {
          StartCoroutine(LoadScene(sceneName));
        }
    }

    // sceneName is the string for the name of the new scene
    public void LoadNewScene(string sceneName)
    {
        if(!isLoading)
        {
          StartCoroutine(LoadScene(sceneName));
        }
    }

    private IEnumerator LoadScene(string sceneName)
    {
      isLoading = true;

      //OnLoadBegin?.Invoke();
      yield return screenFader.StartFadeIn();
      yield return StartCoroutine(UnloadCurrent());

      // Fake wait time
      //yield return new WaitForSeconds(1.0f);

      yield return StartCoroutine(LoadNew(sceneName));
      yield return screenFader.StartFadeOut();
      //OnLoadEnd?.Invoke();

      //yield return new WaitForSeconds(0.5f);

      isLoading = false;
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
      AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

      //Can add the progress loading bar here for async progress

      while(!unloadOperation.isDone)
        yield return null;
    }

    private IEnumerator LoadNew(string sceneName)
    {
		  AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
      
      while(!loadOperation.isDone)
        yield return null;
    }

    private void SetActiveScene(Scene scene, LoadSceneMode mode)
    {
        SceneManager.SetActiveScene(scene);

    }
}

