using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class AttentionRehabilitationProgramme
{
    private IEnumerator LoadScene()
    {
        yield return SceneManager.LoadSceneAsync(StaticReferences.PersistentVRSceneName, LoadSceneMode.Single);
        yield return SceneManager.LoadSceneAsync("02_Practice_02_AttentionTraining", new LoadSceneParameters(LoadSceneMode.Additive));
    }


}
