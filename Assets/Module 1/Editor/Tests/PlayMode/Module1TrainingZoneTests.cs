using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class Module1TrainingZoneTests : MonoBehaviour
{
    private IEnumerator LoadWarmUpZone(int difficultyLevel)
    {
        SceneManager.LoadScene("MainMenu");
        yield return new WaitForSecondsRealtime(0.5f);
        LoadLevelSettings.loadLevelSettings.SetDifficultyLevel(difficultyLevel);
        SceneManager.LoadScene("Module1Training");
    }

    private IEnumerator CheckDifficultyLevel(int difficultyLevel, float expectedDuration)
    {
        Time.timeScale = 10;

        yield return LoadWarmUpZone(difficultyLevel);

        //Account for a task countdown of 6 seconds
        yield return new WaitForSeconds(7);

        FixedGapAudioTask fixedGapAudioTask = GameObject.FindObjectOfType<FixedGapAudioTask>(false);
        Assert.IsNotNull(fixedGapAudioTask);

        Assert.IsTrue(fixedGapAudioTask.genericAudio.Count > 0 && fixedGapAudioTask.triggerClips.Count > 0);
        Assert.IsTrue(Mathf.Abs(expectedDuration - fixedGapAudioTask.taskLengthSeconds) < 20f);

        Time.timeScale = 1;
    }

    [UnityTest]
    public IEnumerator TrainingZoneLevel1()
    {
        yield return CheckDifficultyLevel(1, 5 * 60);
    }

    [UnityTest]
    public IEnumerator TrainingZoneLevel2()
    {
        yield return CheckDifficultyLevel(2, 7 * 60);
    }

    [UnityTest]
    public IEnumerator TrainingZoneLevel3()
    {
        yield return CheckDifficultyLevel(3, 8 * 60);
    }

    [UnityTest]
    public IEnumerator TrainingZoneLevel4()
    {
        yield return CheckDifficultyLevel(4, 10 * 60);
    }
}
