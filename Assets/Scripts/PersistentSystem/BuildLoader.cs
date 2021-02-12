using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildLoader : MonoBehaviour
{
    public string firstSceneToAdd;
    
    private void Awake()
    {
        if (!Application.isEditor)
            LoadPersistent();
    }

    private void LoadPersistent()
    {
        SceneManager.LoadSceneAsync(firstSceneToAdd, LoadSceneMode.Additive);
    }
}
