using UnityEngine;

public class LoadLevelSettings : MonoBehaviour
{
    public static LoadLevelSettings loadLevelSettings;

    public int DifficultyLevel { get; private set; }
    public int Stage { get; set; } = 0;

    public int currentMenu = -1;

    private void Awake()
    {
        if (loadLevelSettings == null)
        {
            loadLevelSettings = this;
            DontDestroyOnLoad(this);
        }
    }

    public void SetDifficultyLevel(int difficultyLevel, int? stage = null)
    {
        loadLevelSettings.DifficultyLevel = difficultyLevel;
        if (stage != null)
        {
            loadLevelSettings.Stage = (int)stage;
        }
    }
}
