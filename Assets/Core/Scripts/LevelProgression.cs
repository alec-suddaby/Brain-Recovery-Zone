using Elixr.PlayerPreferences;
using UnityEngine;
using UnityEngine.Events;

public class LevelProgression : MonoBehaviour
{
    public PlayerPrefInt CurrentLevelPref;
    public PlayerPrefInt CurrentLevelStagePref;
    public PlayerPrefInt CurrentLevelStageProgressionPref;
    public int progressToReachNextStage = 1;
    public int stagesPerLevel = 3;
    public int maxLevel = 8;
    public UnityEvent StageIncreased;
    public UnityEvent LevelIncreased;

    public int GetCurrentLevel()
    {
        return CurrentLevelPref.Read();
    }

    public int GetCurrentStage()
    {
        return CurrentLevelStagePref.Read();
    }

    public void IncreaseProgression(int progress = 1)
    {
        int currentProgress = CurrentLevelStageProgressionPref.Read();
        currentProgress += progress;

        if (currentProgress >= progressToReachNextStage)
        {
            currentProgress = 0;
            IncreaseStage();
        }

        CurrentLevelStageProgressionPref.Write(currentProgress);
    }

    public void IncreaseStage()
    {
        int currentStage = GetCurrentStage();
        currentStage++;

        if (currentStage > stagesPerLevel)
        {
            currentStage = 1;
            IncreaseLevel();
            CurrentLevelStagePref.Write(currentStage);
            return;
        }

        CurrentLevelStagePref.Write(currentStage);
        StageIncreased.Invoke();
    }

    private void IncreaseLevel()
    {
        int currentLevel = GetCurrentLevel();
        if (currentLevel >= maxLevel)
        {
            return;
        }

        currentLevel++;
        CurrentLevelPref.Write(currentLevel);
        LevelIncreased.Invoke();
    }
}
