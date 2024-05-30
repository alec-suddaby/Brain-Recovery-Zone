using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    private bool changingScene = false;

    public void SwitchScene(string sceneName)
    {
        SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();
        sceneLoader.LoadNewScene(sceneName);
    }

    private IEnumerator DelayedSwitchScene(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
