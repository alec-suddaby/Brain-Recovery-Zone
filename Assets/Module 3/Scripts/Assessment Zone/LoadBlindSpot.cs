using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static class LoadBlindSpot
{

    public static List<BlindSpotTask> LoadBlindSpotAssessment(string fileName){
        List<BlindSpotTask> blindSpotTargets = new List<BlindSpotTask>();
        #if UNITY_EDITOR
            string filePath = "C:\\Users\\Alec_\\Desktop\\" +  fileName.Replace(' ', '-') + ".csv";
        #else
            string filePath = Application.persistentDataPath + "/" +  fileName.Replace(' ', '-') + ".csv";
        #endif

        string[] lines = File.ReadAllLines(filePath);

        for(int i = 1; i < lines.Length; i++){
            string[] line = lines[i].Split(',');

            BlindSpotTask blindSpotTarget = new BlindSpotTask();
            blindSpotTarget.targetPosition = (BlindSpotAssessment.TargetPosition)Enum.Parse(typeof(BlindSpotAssessment.TargetPosition),line[1]);
            blindSpotTarget.eye = (EyePatch.Eye)Enum.Parse(typeof(EyePatch.Eye),line[0]);
            blindSpotTarget.isBlindSpot = !bool.Parse(line[2]);

            blindSpotTargets.Add(blindSpotTarget);
        }

        return blindSpotTargets;
    }
}
