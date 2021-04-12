using UnityEngine;
using System.IO;

public class TestTextFileWriting : MonoBehaviour
{
    void Start()
    {
        if(!Application.isEditor)
        {   
            Debug.Log("Start write to text file test");

            //Path of the file
            string path = Application.dataPath + "/mnt/sdcard/BrainRecoveryZoneVideos/Files/LogTest.txt";

            //Create file if it doesnt exist
            if (!File.Exists(path))
            {
                File.WriteAllText(path, "Login Test Log \n\n");
            }

            //Content of the file
            string content = "Login date: " + System.DateTime.Now + "\n";

            //Add some text to file
            File.AppendAllText(path, content);

            Debug.Log("End write to text file test");
        }
    }
}
