using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrimarySceneLoader : MonoBehaviour
{
    public Object openingScene;

    void Awake()
    {
        LoadScene();
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(openingScene.name, LoadSceneMode.Additive);
    }
}
