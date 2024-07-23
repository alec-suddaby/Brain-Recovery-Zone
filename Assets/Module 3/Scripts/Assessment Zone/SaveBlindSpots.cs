using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveBlindSpots : MonoBehaviour
{
    public BlindSpotAssessment blindSpotAssessment;
    public string fileName = "BlindSpots";

    public void Save(){

        #if UNITY_EDITOR
            string filePath = "C:\\Users\\Alec_\\Desktop\\" +  fileName.Replace(' ', '-') + ".csv";
        #else
            string filePath = Application.persistentDataPath + "/" +  fileName.Replace(' ', '-') + ".csv";
        #endif
        

        File.WriteAllLines(filePath, new List<string>(){ ("Side,Position,Visible") });

        foreach(BlindSpotAssessment.BlindSpotTarget target in blindSpotAssessment.targets){
            Dictionary<string, int>.KeyCollection keys = target.hits.Keys;
            foreach(string key in keys){
                File.AppendAllLines(filePath, new List<string>(){ ($"{key},{target.targetPosition.ToString()},{(target.hits[key] >= blindSpotAssessment.minScore).ToString()}") });
            }
        }

        Debug.Log("File Saved");
    }
}
