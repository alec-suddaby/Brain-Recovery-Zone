using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class Module2WarmUpZoneTests
{
    private IEnumerator LoadWarmUpZone(int difficultyLevel)
    {
        SceneManager.LoadScene("MainMenu");
        yield return new WaitForSecondsRealtime(0.5f);
        LoadLevelSettings.loadLevelSettings.SetDifficultyLevel(difficultyLevel);
        SceneManager.LoadScene("Module2WarmUp");
    }

    private IEnumerator CheckDifficultyLevel(int difficultyLevel, bool forceExerciseStart = false)
    {
        Time.timeScale = 10;

        yield return LoadWarmUpZone(difficultyLevel);

        yield return new WaitForSecondsRealtime(1f);

        if (forceExerciseStart)
        {
            TaskCountdown taskCountdown = GameObject.FindObjectOfType<TaskCountdown>(false);
            taskCountdown.InitTask();
        }

        //Account for a task countdown of 6 seconds
        yield return new WaitForSeconds(7);

        FixedGapAudioTask fixedGapAudioTask = GameObject.FindObjectOfType<FixedGapAudioTask>(false);
        Assert.IsNotNull(fixedGapAudioTask);

        Assert.IsTrue(fixedGapAudioTask.audioTaskSounds.Count + fixedGapAudioTask.CompletedSounds.Count == fixedGapAudioTask.numberOfTriggerAudioClips + fixedGapAudioTask.numberOfGenericAudioClips);

        float lastAudioTime = float.MinValue;

        for(int i = 0; i < fixedGapAudioTask.audioTaskSounds.Count; i++)
        {
            float currentTime = fixedGapAudioTask.audioTaskSounds[i].playAtTimeSeconds;
            Assert.IsTrue(currentTime - lastAudioTime >= fixedGapAudioTask.timeBetweenAudio);
            lastAudioTime = currentTime;
        }

        Time.timeScale = 1;
    }

    [UnityTest]
    public IEnumerator WarmUpZoneLevel1()
    {
        yield return CheckDifficultyLevel(1);
    }

    [UnityTest]
    public IEnumerator WarmUpZoneLevel2()
    {
        yield return CheckDifficultyLevel(2);
    }

    [UnityTest]
    public IEnumerator WarmUpZoneLevel3()
    {
        yield return CheckDifficultyLevel(3);
    }

    [UnityTest]
    public IEnumerator WarmUpZoneLevel4()
    {
        yield return CheckDifficultyLevel(4);
    }

    [UnityTest]
    public IEnumerator WarmUpZoneLevel5()
    {
        yield return CheckDifficultyLevel(5);
    }

    [UnityTest]
    public IEnumerator WarmUpZoneLevel6()
    {
        yield return CheckDifficultyLevel(6, true);
    }

    [UnityTest]
    public IEnumerator WarmUpZoneLevel7()
    {
        yield return CheckDifficultyLevel(7, true);
    }

    [UnityTest]
    public IEnumerator WarmUpZoneLevel8()
    {
        yield return CheckDifficultyLevel(8, true);
    }

    [UnityTest]
    public IEnumerator WarmUpZoneLevel9()
    {
        yield return CheckDifficultyLevel(9, true);
    }
}
