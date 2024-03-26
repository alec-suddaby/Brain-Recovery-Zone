using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class LikertScaleEditModeTests
{
    [Test]
    public void LikertScaleJsonSerialisationTest_Test93()
    {
        StatisticsController.Instance.RecordLikertToJSON(1.5f, "Test Save", 8.5f);

        StatisticsController.BRZStatisticsList stats = StatisticsController.Instance.LoadStatistics();

        StatisticsController.BRZStatistic statistic = stats.statistics[stats.statistics.Count - 1];

        Assert.IsTrue(statistic.startRating == 1.5f && statistic.experienceName == "Test Save" && statistic.finishRating == 8.5f);
    }

    [Test]
    public void LikertScaleClearSave_Test96()
    {
        PlayerPrefs.SetString("LikertVideoTitle", "Test Value");

        LikertScaleInteractionManager likertScaleInteractionManager = new LikertScaleInteractionManager();
        likertScaleInteractionManager.LikertClearSave();

        Assert.IsTrue(PlayerPrefs.GetString("LikertVideoTitle") == "");
    }
}
