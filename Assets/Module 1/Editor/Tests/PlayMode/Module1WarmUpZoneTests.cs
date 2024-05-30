using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class Module1WarmUpZoneTests
{
    private IEnumerator LoadWarmUpZone(int difficultyLevel)
    {
        SceneManager.LoadScene("MainMenu");
        yield return new WaitForSecondsRealtime(0.5f);
        LoadLevelSettings.loadLevelSettings.SetDifficultyLevel(difficultyLevel);
        SceneManager.LoadScene("Module1WarmUp");
    }

    private IEnumerator CheckDifficultyLevel(int difficultyLevel, int maximumAudioCues, float expectedDuration)
    {
        Time.timeScale = 10;

        yield return LoadWarmUpZone(difficultyLevel);

        //Account for a task countdown of 6 seconds
        yield return new WaitForSeconds(7);

        RemoveAudioClips removeAudioClips = GameObject.FindObjectOfType<RemoveAudioClips>(false);
        Assert.IsNotNull(removeAudioClips);

        Assert.IsTrue(removeAudioClips.audioTaskSounds.Count <= maximumAudioCues && removeAudioClips.audioTaskSounds.Count > 0);
        Assert.IsTrue(Mathf.Abs(expectedDuration - removeAudioClips.taskLengthSeconds) < 2f);

        Time.timeScale = 1;
    }

    [UnityTest]
    public IEnumerator WarmUpZoneLevel1()
    {
        yield return CheckDifficultyLevel(1, 8, 35);
    }

    [UnityTest]
    public IEnumerator WarmUpZoneLevel2()
    {
        yield return CheckDifficultyLevel(2, 10, 47);
    }

    [UnityTest]
    public IEnumerator WarmUpZoneLevel3()
    {
        yield return CheckDifficultyLevel(3, 12, 57);
    }

    [UnityTest]
    public IEnumerator WarmUpZoneLevel4()
    {
        yield return CheckDifficultyLevel(4, 15, 64);
    }
}
