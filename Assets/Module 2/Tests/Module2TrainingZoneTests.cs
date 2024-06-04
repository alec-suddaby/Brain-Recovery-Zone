using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class Module2TrainingZoneTests
{
    internal class ExpectedTracks
    {
        public int genericTracks;
        public int triggerTracks;

        internal ExpectedTracks(int genericTracks, int triggerTracks)
        {
            this.genericTracks = genericTracks;
            this.triggerTracks = triggerTracks;
        }
    }

    private float durationForgivenessSeconds = 0.1f;

    private IEnumerator LoadTrainingZone(int difficultyLevel, string levelName)
    {
        SceneManager.LoadScene("MainMenu");
        yield return new WaitForSecondsRealtime(0.5f);
        LoadLevelSettings.loadLevelSettings.SetDifficultyLevel(difficultyLevel);
        SceneManager.LoadScene(levelName);
    }

    private IEnumerator LoadTrainingZoneLevel1(int difficultyLevel)
    {
        yield return LoadTrainingZone(difficultyLevel, "Module2TrainingLevel1");
    }

    private IEnumerator LoadTrainingZoneLevel1Traffic(int difficultyLevel)
    {
        yield return LoadTrainingZone(difficultyLevel, "Module2TrainingLevel1Traffic");
    }

    private IEnumerator LoadTrainingZoneLevel2(int difficultyLevel)
    {
        yield return LoadTrainingZone(difficultyLevel, "Module2TrainingLevel2");
    }

    private IEnumerator LoadTrainingZoneLevel3(int difficultyLevel)
    {
        yield return LoadTrainingZone(difficultyLevel, "Module2TrainingLevel3");
    }

    private AudioIntroWithRandomTask GetTaskIntroAudio()
    {
        return GameObject.FindObjectOfType<AudioIntroWithRandomTask>(false);
    }

    private FixedGapAudioTask GetAudioTask()
    {
        return GameObject.FindObjectOfType<FixedGapAudioTask>(false);
    }

    private void CheckAudioTaskWithClipRange(FixedGapAudioTask task, int genericClips, int triggerClips)
    {
        int triggerClipsFound = task.audioTaskSounds.Where(x => x.isTriggerSound).Count();
        int genericClipsFound = task.audioTaskSounds.Where(x => !x.isTriggerSound).Count();
        int totalClips = task.audioTaskSounds.Count();

        Assert.IsTrue(triggerClipsFound <= triggerClips, $"Too many trigger clips for difficulty level {LoadLevelSettings.loadLevelSettings.DifficultyLevel}. Found {triggerClipsFound}, expected {triggerClips} on object {task.transform.name}");
        Assert.IsTrue(genericClipsFound >= genericClips, $"Too few generic clips for difficulty level {LoadLevelSettings.loadLevelSettings.DifficultyLevel}. Found {genericClipsFound}, expected {genericClips} on object {task.transform.name}");
        Assert.IsTrue(totalClips == triggerClips + genericClips, $"Incorrect number of generic clips for difficulty level {LoadLevelSettings.loadLevelSettings.DifficultyLevel}. Found {genericClipsFound}, expected {genericClips} on object {task.transform.name}");

        for (int i = 1; i < totalClips; i++)
        {
            float gapBetweenClips = Mathf.Abs(task.audioTaskSounds[i].playAtTimeSeconds - task.audioTaskSounds[i - 1].playAtTimeSeconds);

            Assert.IsTrue(Mathf.Abs(gapBetweenClips - task.timeBetweenAudio) < durationForgivenessSeconds, $"Incorrect gap between audio. Expected: {task.timeBetweenAudio}. Found: {gapBetweenClips}.");
        }
    }

    private void CheckAudioTaskWithExactClipCount(FixedGapAudioTask task, int genericClips, int triggerClips)
    {
        int triggerClipsFound = task.audioTaskSounds.Where(x => x.isTriggerSound).Count();
        int genericClipsFound = task.audioTaskSounds.Where(x => !x.isTriggerSound).Count();
        int totalClips = task.audioTaskSounds.Count();

        Assert.IsTrue(triggerClipsFound == triggerClips, $"Incorrect number of trigger clips for difficulty level {LoadLevelSettings.loadLevelSettings.DifficultyLevel}. Found {triggerClipsFound}, expected {triggerClips} on object {task.transform.name}");
        Assert.IsTrue(genericClipsFound == genericClips, $"Incorrect number of generic clips for difficulty level {LoadLevelSettings.loadLevelSettings.DifficultyLevel}. Found {genericClipsFound}, expected {genericClips} on object {task.transform.name}");
        Assert.IsTrue(totalClips == triggerClips + genericClips, $"Incorrect number of generic clips for difficulty level {LoadLevelSettings.loadLevelSettings.DifficultyLevel}. Found {genericClipsFound}, expected {genericClips} on object {task.transform.name}");

        for (int i = 1; i < totalClips; i++)
        {
            float gapBetweenClips = Mathf.Abs(task.audioTaskSounds[i].playAtTimeSeconds - task.audioTaskSounds[i - 1].playAtTimeSeconds);

            Assert.IsTrue(Mathf.Abs(gapBetweenClips - task.timeBetweenAudio) < durationForgivenessSeconds, $"Incorrect gap between audio. Expected: {task.timeBetweenAudio}. Found: {gapBetweenClips}.");
        }
    }

    [UnityTest]
    public IEnumerator Level1Test()
    {
        List<ExpectedTracks> expectedTracks = new() { new(3, 1), new(4, 1), new(6, 1), new(7, 1), new(9, 1), new(12, 1) };

        for (int i = 1; i <= 6; i++)
        {
            yield return LoadTrainingZoneLevel1(i);
            yield return new WaitForSecondsRealtime(0.5f);
            FixedGapAudioTask audioTask = GetAudioTask();
            audioTask.InitTask();          

            ExpectedTracks tracks = expectedTracks[i - 1];
            CheckAudioTaskWithClipRange(audioTask, tracks.genericTracks, tracks.triggerTracks);
        }
    }

    [UnityTest]
    public IEnumerator Level1TrafficTest()
    {
        List<ExpectedTracks> expectedTracks = new() { new(3, 1), new(4, 1), new(6, 1), new(7, 1), new(9, 1), new(12, 1) };

        for (int i = 1; i <= 6; i++)
        {
            yield return LoadTrainingZoneLevel1Traffic(i);
            yield return new WaitForSecondsRealtime(0.5f);
            FixedGapAudioTask audioTask = GetAudioTask();
            audioTask.InitTask();

            ExpectedTracks tracks = expectedTracks[i - 1];
            CheckAudioTaskWithClipRange(audioTask, tracks.genericTracks, tracks.triggerTracks);
        }
    }

    [UnityTest]
    public IEnumerator Level2Test()
    {
        List<ExpectedTracks> expectedTracks = new() { new(36, 12), new(32, 16), new(60, 12) };

        for (int i = 1; i <= 3; i++)
        {
            yield return LoadTrainingZoneLevel2(i);
            yield return new WaitForSecondsRealtime(0.5f);
            FixedGapAudioTask audioTask = GetAudioTask();
            audioTask.InitTask();

            ExpectedTracks tracks = expectedTracks[i - 1];
            CheckAudioTaskWithExactClipCount(audioTask, tracks.genericTracks, tracks.triggerTracks);

            yield return new WaitForSecondsRealtime(0.5f);

            Assert.IsTrue(audioTask.transform.GetComponentInChildren<AudioSource>().isPlaying, "Background audio not playing");
        }
    }

    [UnityTest]
    public IEnumerator Level3Test()
    {
        List<ExpectedTracks> expectedTracks = new() { new(36, 12), new(32, 16), new(60, 12) };

        for (int i = 1; i <= 3; i++)
        {
            yield return LoadTrainingZoneLevel3(i);
            yield return new WaitForSecondsRealtime(0.5f);
            FixedGapAudioTask audioTask = GetAudioTask();
            audioTask.InitTask();

            ExpectedTracks tracks = expectedTracks[i - 1];
            CheckAudioTaskWithExactClipCount(audioTask, tracks.genericTracks, tracks.triggerTracks);

            yield return new WaitForSecondsRealtime(0.5f);

            Assert.IsTrue(audioTask.transform.GetComponentInChildren<AudioSource>().isPlaying, "Background audio not playing");
        }
    }
}
