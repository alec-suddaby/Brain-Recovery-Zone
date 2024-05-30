using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarmUpZoneDifficulty : MonoBehaviour
{

    [System.Serializable]
    public struct DifficultyLevel
    {
        public int difficultyLevel;
        public GameObject difficultyObject;
    }
    public List<DifficultyLevel> difficulties;
    [HideInInspector]
    public BellRinging bellRinging;
    public Text levelText;

    private void Awake()
    {
        int diffLevel = LoadLevelSettings.loadLevelSettings.DifficultyLevel;

        if(levelText != null )
            levelText.text = "Level " + diffLevel.ToString();

        foreach (DifficultyLevel level in difficulties)
        {
            if (level.difficultyLevel == diffLevel)
            {
                bellRinging = level.difficultyObject.GetComponent<BellRinging>();
                level.difficultyObject.SetActive(true);
                return;
            }
        }
    }
}
