using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class AlerterSoundsTest
{
    private const string IntPrefName = "TestingInt";

    [Test]
    public void AlerterSoundAdded()
    {
        PlayerPrefs.SetInt(IntPrefName, 1);
        Assert.IsTrue(TestAlerterSound());

        PlayerPrefs.SetInt(IntPrefName, 0);
        Assert.IsTrue(!TestAlerterSound());
    }

    private bool TestAlerterSound()
    {
        GameObject gameObject = new GameObject();
        gameObject.SetActive(false);

        FixedGapAudioTask fixedGapAudioTask = gameObject.AddComponent<FixedGapAudioTask>();
        FixedGapAudioTask.AlerterSound alerterSound = new FixedGapAudioTask.AlerterSound();

        AddAlerterSound addAlerterSound = gameObject.AddComponent<AddAlerterSound>();
        addAlerterSound.task = fixedGapAudioTask;
        addAlerterSound.task.alerterSounds = new List<FixedGapAudioTask.AlerterSound>();
        addAlerterSound.taskSound = alerterSound;
        addAlerterSound.playerPref = IntPrefName;
        addAlerterSound.Start();

        return fixedGapAudioTask.alerterSounds.Contains(alerterSound);
    }
}
