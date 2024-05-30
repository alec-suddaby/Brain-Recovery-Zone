using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;

public class CheckIfFileExists : MonoBehaviour
{
    public UnityEvent<bool> fileExists;
    public string fileName = "BlindSpots";


    void OnEnable(){
        #if UNITY_EDITOR
            string filePath = "C:\\Users\\Alec_\\Desktop\\" +  fileName.Replace(' ', '-') + ".csv";
        #else
            string filePath = Application.persistentDataPath + "/" +  fileName.Replace(' ', '-') + ".csv";
        #endif

        fileExists.Invoke(File.Exists(filePath));
    }
}
