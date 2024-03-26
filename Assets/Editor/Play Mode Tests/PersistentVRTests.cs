using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PersistentVRTests
{
    //[Test]
    //public void PersistentVRTestsSimplePasses()
    //{
    //    SceneManager.LoadScene(0);
    //    Assert.IsTrue(true);
    //}

    #region SceneLoader
    private SceneLoader sceneLoader;
    private const float postSceneLoadDelay = 0.05f;

    private List<Scene> GetOpenScenes()
    {
        List<Scene> openScenes = new List<Scene>();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            openScenes.Add(SceneManager.GetSceneAt(i));
        }

        return openScenes;
    }

    [UnityTest, Order(0)]
    public IEnumerator SceneLoaderLoadPersistentTest_Test105()
    {
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(0);
        loadScene.allowSceneActivation = true;

        while (!loadScene.isDone)
        {
            yield return null;
        }

        SceneLoader sceneLoader = GameObject.FindObjectOfType<SceneLoader>();
        sceneLoader.LoadPersistent();

        while (sceneLoader.IsLoading)
        {
            yield return null;
        }

        yield return new WaitForSecondsRealtime(postSceneLoadDelay);

        List<Scene> openScenes = GetOpenScenes();

        this.sceneLoader = sceneLoader;
        Assert.IsTrue(openScenes.Where(x => x.name == sceneLoader.mainMenuName).Count() == 1 && openScenes.Count == 2);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest, Order(1)]
    public IEnumerator SceneManagerLoadNewAdditiveScene_Test105()
    {
        List<Scene> openScenes = GetOpenScenes();

        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled || openScenes.Where(x => x.path == scene.path).Count() > 0)
            {
                continue;
            }

            sceneLoader.LoadNewScene(scene.path);

            while (sceneLoader.IsLoading)
            {
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(postSceneLoadDelay);

            List<Scene> currentlyOpenScenes = GetOpenScenes();

            Assert.IsTrue(currentlyOpenScenes.Where(x => x.path == scene.path).Count() == 1 && openScenes.Count == 2);

            break;
        }
    }
    #endregion
}
