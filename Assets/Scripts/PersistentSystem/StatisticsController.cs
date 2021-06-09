using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class StatisticsController : Singleton<StatisticsController>
{
    [Serializable]
    public class BRZStatisticsList
    {
        public List<BRZStatistic> statistics = new List<BRZStatistic>();
    }

    [Serializable]
    public class BRZStatistic
    {
        public string experienceName;
        public float startRating;
        public float finishRating;
        public string timeStamp;
    }

    private static BRZStatistic currentStatistic = null;
    private static BRZStatisticsList statisticsList = null;
    private static string fileLocation;
    private static readonly string fileName = "statistics.json";

    private void Start()
    {
        
        //LoadStatistics();
    }

    private void LoadStatistics()
    {
        Debug.Log("LoadStatistics Called in StatisticsController");

        // Check if editor
        if (Application.isEditor){fileLocation = Application.persistentDataPath;}
        else{fileLocation = "/mnt/sdcard/BRZ_ASSETS/data/";}
        Debug.Log("JSON File Path Location: " + fileLocation);

        if(!File.Exists(fileLocation + fileName) || !Directory.Exists(fileLocation))
        {
            Debug.Log("ERROR: JSON file or directory not found");
            statisticsList = new BRZStatisticsList();
            return;
        }
        string jsonList = File.ReadAllText(fileLocation + fileName);
        Debug.Log(jsonList);
        if (jsonList == null || jsonList.Trim().Equals(""))
        {
            statisticsList = new BRZStatisticsList();
        }
        else
        {
            statisticsList = JsonUtility.FromJson<BRZStatisticsList>(jsonList);
        }
    }

    private void SaveStatistics()
    {
        
        string statsString = JsonUtility.ToJson(statisticsList);
        File.WriteAllText(fileLocation + fileName, statsString);
    }
/*
    public void RegisterStartValue(int startValue, string experienceName)
    {
        if(statisticsList == null)
        {
            LoadStatistics();
        }

        currentStatistic = new BRZStatistic();
        currentStatistic.startRating = startValue;
        currentStatistic.experienceName = experienceName;
        currentStatistic.timeStamp = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

        statisticsList.statistics.Add(currentStatistic);
    }

    public int RegisterEndValue(int endValue)
    {
        if(currentStatistic == null)
        {
            Debug.Log("current statistic null");
        }
        currentStatistic.finishRating = endValue;
        SaveStatistics();
        return currentStatistic.finishRating - currentStatistic.startRating; 
    }
*/
    public void RecordLikertToJSON(float startValue, string experienceName, float endValue)
    {
        if(statisticsList == null)
        {
            LoadStatistics();
        }

        currentStatistic = new BRZStatistic();
        currentStatistic.startRating = startValue;
        currentStatistic.experienceName = experienceName;
        currentStatistic.timeStamp = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        currentStatistic.finishRating = endValue;
        statisticsList.statistics.Add(currentStatistic);

        SaveStatistics();
    }

    
}   
