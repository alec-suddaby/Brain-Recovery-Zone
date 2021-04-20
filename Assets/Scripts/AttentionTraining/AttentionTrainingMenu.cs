using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AttentionTrainingMenu : MonoBehaviour
{
    // Text areas
    public TextMeshProUGUI level1ResultsTextBox;
    public TextMeshProUGUI level2ResultsTextBox;
    public TextMeshProUGUI level3ResultsTextBox;

    // Lists
    private List<string> level1ResultsList = new List<string>();
    private List<string> level2ResultsList = new List<string>();
    private List<string> level3ResultsList = new List<string>();

    // Last items added
    private string level1ResultLastAttempt;
    private string level2ResultLastAttempt;
    private string level3ResultLastAttempt;

    // Null Count
    private int level1NullCount = 0;
    private int level2NullCount = 0;
    private int level3NullCount = 0;

    [Header("Audio Button")]
    public GameObject enableAudioButton;
    public GameObject disableAudioButton;

    // Start is called before the first frame update
    void Start()
    {
        // Get last attempt
        Level1ResultsLastAttempt();
        Level2ResultsLastAttempt();
        Level3ResultsLastAttempt();

        LoadResults();

        // Sort the lists
        Level1ResultsSort();
        Level2ResultsSort();
        Level3ResultsSort();
        
        // Topup any values that need to be null
        Level1ResultsLengthCheck();
        Level2ResultsLengthCheck();
        Level3ResultsLengthCheck();
  
        // Print Lists
        Level1ResultsListPrint();
        Level2ResultsListPrint();
        Level3ResultsListPrint();

        SaveReults();

        if(PlayerPrefs.GetInt("PlayAttentionTrainingMute") == 0)
        {
            EnableAudioPlaySound();
        }
        else if(PlayerPrefs.GetInt("PlayAttentionTrainingMute") == 1)
        {
            DisableAudioPlayMute();
        }
        else
        {
            EnableAudioPlaySound();
        }
    }

    // Get the last result or set it to --:--:--, and then add to the list
    private void Level1ResultsLastAttempt()
    {
        level1ResultLastAttempt = PlayerPrefs.GetString("Level1LastResult");

        if(level1ResultLastAttempt == "")
        {
            level1ResultLastAttempt = "--:--:--";
        }
    }
    private void Level2ResultsLastAttempt()
    {
        level2ResultLastAttempt = PlayerPrefs.GetString("Level2LastResult");

        if(level2ResultLastAttempt == "")
        {
            level2ResultLastAttempt = "--:--:--";
        }
    }
    private void Level3ResultsLastAttempt()
    {
        level3ResultLastAttempt = PlayerPrefs.GetString("Level3LastResult");

        if(level3ResultLastAttempt == "")
        {
            level3ResultLastAttempt = "--:--:--";
        }
    }
    
    // Sort each list from low to high then swap from high to low
    private void Level1ResultsSort()
    {
        level1ResultsList.Sort();

        List<string> reverseLevel1ResultsList = new List<string>();
        for (int i = level1ResultsList.Count; i --> 0;)
        {
            reverseLevel1ResultsList.Add(level1ResultsList[i]);
        }

        level1ResultsList = reverseLevel1ResultsList;
    }
    private void Level2ResultsSort()
    {
        level2ResultsList.Sort();

        List<string> reverseLevel2ResultsList = new List<string>();
        for (int i = level2ResultsList.Count; i --> 0;)
        {
            reverseLevel2ResultsList.Add(level2ResultsList[i]);
        }

        level2ResultsList = reverseLevel2ResultsList;
    }
    private void Level3ResultsSort()
    {
        level3ResultsList.Sort();

        List<string> reverseLevel3ResultsList = new List<string>();
        for (int i = level3ResultsList.Count; i --> 0;)
        {
            reverseLevel3ResultsList.Add(level3ResultsList[i]);
        }

        level3ResultsList = reverseLevel3ResultsList;
    }

    // Check the number of items in the list and for any item that isn't there, add the --:--:-- value into the list to top it up to at least 5
    private void Level1ResultsLengthCheck()
    {
        if(level1ResultsList.Count == 0)
        {
            level1ResultsList.Add("--:--:--");
            level1ResultsList.Add("--:--:--");
            level1ResultsList.Add("--:--:--");
            level1ResultsList.Add("--:--:--");
            level1ResultsList.Add("--:--:--");
        }
        else if(level1ResultsList.Count == 1)
        {
            level1ResultsList.Add("--:--:--");
            level1ResultsList.Add("--:--:--");
            level1ResultsList.Add("--:--:--");
            level1ResultsList.Add("--:--:--");
        }
        else if(level1ResultsList.Count == 2)
        {
            level1ResultsList.Add("--:--:--");
            level1ResultsList.Add("--:--:--");
            level1ResultsList.Add("--:--:--");
        }
        else if(level1ResultsList.Count == 3)
        {
            level1ResultsList.Add("--:--:--");
            level1ResultsList.Add("--:--:--");
        }
        else if(level1ResultsList.Count == 4)
        {
            level1ResultsList.Add("--:--:--");
        }
    }
    private void Level2ResultsLengthCheck()
    {
        if(level2ResultsList.Count == 0)
        {
            level2ResultsList.Add("--:--:--");
            level2ResultsList.Add("--:--:--");
            level2ResultsList.Add("--:--:--");
            level2ResultsList.Add("--:--:--");
            level2ResultsList.Add("--:--:--");
        }
        else if(level2ResultsList.Count == 1)
        {
            level2ResultsList.Add("--:--:--");
            level2ResultsList.Add("--:--:--");
            level2ResultsList.Add("--:--:--");
            level2ResultsList.Add("--:--:--");
        }
        else if(level2ResultsList.Count == 2)
        {
            level2ResultsList.Add("--:--:--");
            level2ResultsList.Add("--:--:--");
            level2ResultsList.Add("--:--:--");
        }
        else if(level2ResultsList.Count == 3)
        {
            level2ResultsList.Add("--:--:--");
            level2ResultsList.Add("--:--:--");
        }
        else if(level2ResultsList.Count == 4)
        {
            level2ResultsList.Add("--:--:--");
        }
    }
    private void Level3ResultsLengthCheck()
    {
        if(level3ResultsList.Count == 0)
        {
            level3ResultsList.Add("--:--:--");
            level3ResultsList.Add("--:--:--");
            level3ResultsList.Add("--:--:--");
            level3ResultsList.Add("--:--:--");
            level3ResultsList.Add("--:--:--");
        }
        else if(level3ResultsList.Count == 1)
        {
            level3ResultsList.Add("--:--:--");
            level3ResultsList.Add("--:--:--");
            level3ResultsList.Add("--:--:--");
            level3ResultsList.Add("--:--:--");
        }
        else if(level3ResultsList.Count == 2)
        {
            level3ResultsList.Add("--:--:--");
            level3ResultsList.Add("--:--:--");
            level3ResultsList.Add("--:--:--");
        }
        else if(level3ResultsList.Count == 3)
        {
            level3ResultsList.Add("--:--:--");
            level3ResultsList.Add("--:--:--");
        }
        else if(level3ResultsList.Count == 4)
        {
            level3ResultsList.Add("--:--:--");
        }
    }

    // Print the list top 5 and the latest values that was added
    private void Level1ResultsListPrint()
    {
        level1ResultsTextBox.text = 
        "Last attempt" + "\n" +
        level1ResultLastAttempt + "\n" + "\n" + "\n" +
        "Personal Best" + "\n" +
        level1ResultsList[0] + "\n" +
        level1ResultsList[1] + "\n" +
        level1ResultsList[2] + "\n" +
        level1ResultsList[3] + "\n" +
        level1ResultsList[4] + "\n"
        ;
    }
    private void Level2ResultsListPrint()
    {
        level2ResultsTextBox.text = 
        "Last attempt" + "\n" +
        level2ResultLastAttempt + "\n" + "\n" + "\n" +
        "Personal Best" + "\n" +
        level2ResultsList[0] + "\n" +
        level2ResultsList[1] + "\n" +
        level2ResultsList[2] + "\n" +
        level2ResultsList[3] + "\n" +
        level2ResultsList[4] + "\n"
        ;
    }
    private void Level3ResultsListPrint()
    {
        level3ResultsTextBox.text = 
        "Last attempt" + "\n" +
        level3ResultLastAttempt + "\n" + "\n" + "\n" +
        "Personal Best" + "\n" +
        level3ResultsList[0] + "\n" +
        level3ResultsList[1] + "\n" +
        level3ResultsList[2] + "\n" +
        level3ResultsList[3] + "\n" +
        level3ResultsList[4] + "\n"
        ;
    }
    
    // Save the results to player prefs
    public void SaveReults()
    {
        SaveLevel1Results();
        SaveLevel2Results();
        SaveLevel3Results();
    }
    private void SaveLevel1Results()
    {
        for(int i = 0; i < level1ResultsList.Count; i++)
        {
            // Don't add the null string to the Player Prefs
            if(level1ResultsList[i] != "--:--:--")
            {
                PlayerPrefs.SetString("Level1Results" + i, level1ResultsList[i]);
            }
            else
            {
                // Count the amount of null strings
                level1NullCount = level1NullCount + 1;
            }
        }

        // Check if the count is == 0 because if you subtract the null count from 0 you get a negative count and results won't save
        if (level1ResultsList.Count != 0)
        {
            // Ensure the null strings are subtracted from the value of the count
            int level1CountSave =  level1ResultsList.Count - level1NullCount;
            PlayerPrefs.SetInt("Level1Count", level1CountSave);
        }
        else
        {
            PlayerPrefs.SetInt("Level1Count", 0);
        }
    }
    private void SaveLevel2Results()
    {
        for(int i = 0; i < level2ResultsList.Count; i++)
        {
            // Don't add the null string to the Player Prefs
            if(level2ResultsList[i] != "--:--:--")
            {
                PlayerPrefs.SetString("Level2Results" + i, level2ResultsList[i]);
            }
            else
            {
                // Count the amount of null strings
                level2NullCount = level2NullCount + 1;
            }
        }

        // Check if the count is == 0 because if you subtract the null count from 0 you get a negative count and results won't save
        if (level2ResultsList.Count != 0)
        {
            // Ensure the null strings are subtracted from the value of the count
            int level2CountSave =  level2ResultsList.Count - level2NullCount;
            PlayerPrefs.SetInt("Level2Count", level2CountSave);
        }
        else
        {
            PlayerPrefs.SetInt("Level2Count", 0);
        }
    }
    private void SaveLevel3Results()
    {
        for(int i = 0; i < level3ResultsList.Count; i++)
        {
            // Don't add the null string to the Player Prefs
            if(level3ResultsList[i] != "--:--:--")
            {
                PlayerPrefs.SetString("Level3Results" + i, level3ResultsList[i]);
            }
            else
            {
                // Count the amount of null strings
                level3NullCount = level3NullCount + 1;
            }
        }

        // Check if the count is == 0 because if you subtract the null count from 0 you get a negative count and results won't save
        if (level3ResultsList.Count != 0)
        {
            // Ensure the null strings are subtracted from the value of the count
            int level3CountSave =  level3ResultsList.Count - level3NullCount;
            PlayerPrefs.SetInt("Level3Count", level3CountSave);
        }
        else
        {
            PlayerPrefs.SetInt("Level3Count", 0);
        }
    }

    // Load results from player prefs. If there are no resuts, nothing will load.
    public void LoadResults()
    {
        LoadLevel1Results();
        LoadLevel2Results();
        LoadLevel3Results();
    }
    private void LoadLevel1Results()
    {
        level1ResultsList.Clear();
        int level1SavedListCount = PlayerPrefs.GetInt("Level1Count");

        for(int i = 0; i < level1SavedListCount; i++)
        {
            string level1Result = PlayerPrefs.GetString("Level1Results" + i);
            level1ResultsList.Add(level1Result);
        }

        //Debug.Log("load end count: " + level1ResultsList.Count);
    }
    private void LoadLevel2Results()
    {
        level2ResultsList.Clear();
        int level2SavedListCount = PlayerPrefs.GetInt("Level2Count");

        for(int i = 0; i < level2SavedListCount; i++)
        {
            string level2Result = PlayerPrefs.GetString("Level2Results" + i);
            level2ResultsList.Add(level2Result);
        }

        //Debug.Log("load end level2 count: " + level2ResultsList.Count);
    }
    private void LoadLevel3Results()
    {
        level3ResultsList.Clear();
        int level3SavedListCount = PlayerPrefs.GetInt("Level3Count");

        for(int i = 0; i < level3SavedListCount; i++)
        {
            string level3Result = PlayerPrefs.GetString("Level3Results" + i);
            level3ResultsList.Add(level3Result);
        }

        //Debug.Log(level3ResultsList.Count);
    }

    // Reset results from player prefs
    public void ResetResults()
    {
        ResetLevel1Results();
        ResetLevel2Results();
        ResetLevel3Results();
        
        // Get last attempt
        Level1ResultsLastAttempt();
        Level2ResultsLastAttempt();
        Level3ResultsLastAttempt();

        // Reload the results
        LoadResults();

        // Set any null values
        Level1ResultsLengthCheck();
        Level2ResultsLengthCheck();
        Level3ResultsLengthCheck();

        // Print Lists
        Level1ResultsListPrint();
        Level2ResultsListPrint();
        Level3ResultsListPrint();

        SaveReults();
    }
    private void ResetLevel1Results()
    {
        int level1SavedListCount = PlayerPrefs.GetInt("Level1Count");

        for(int i = 0; i < level1SavedListCount; i++)
        {
            PlayerPrefs.DeleteKey("Level1Results" + i);
        }
        
        PlayerPrefs.DeleteKey("Level1LastResult");
        PlayerPrefs.DeleteKey("Level1Count");
        PlayerPrefs.SetInt("Level1Count", 0);
        level1NullCount = 0;
    }
    private void ResetLevel2Results()
    {
        int level2SavedListCount = PlayerPrefs.GetInt("Level2Count");

        for(int i = 0; i < level2SavedListCount; i++)
        {
            PlayerPrefs.DeleteKey("Level2Results" + i);
        }
        
        PlayerPrefs.DeleteKey("Level2LastResult");
        PlayerPrefs.DeleteKey("MorderateCount");
        PlayerPrefs.SetInt("Level2Count", 0);
        level2NullCount = 0;
    }
    private void ResetLevel3Results()
    {
        int level3SavedListCount = PlayerPrefs.GetInt("Level3Count");

        for(int i = 0; i < level3SavedListCount; i++)
        {
            PlayerPrefs.DeleteKey("Level3Results" + i);
        }
        
        PlayerPrefs.DeleteKey("Level3LastResult");
        PlayerPrefs.DeleteKey("Level3Count");
        PlayerPrefs.SetInt("Level3Count", 0);
        level3NullCount = 0;
    }

    public void EnableAudioPlaySound()
    {
        PlayerPrefs.SetInt("PlayAttentionTrainingMute", 0);
        enableAudioButton.SetActive(false);
        disableAudioButton.SetActive(true);
    }

    public void DisableAudioPlayMute()
    {
        PlayerPrefs.SetInt("PlayAttentionTrainingMute", 1);
        enableAudioButton.SetActive(true);
        disableAudioButton.SetActive(false);
    }
}
