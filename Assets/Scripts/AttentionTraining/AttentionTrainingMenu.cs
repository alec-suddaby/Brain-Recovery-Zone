using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AttentionTrainingMenu : MonoBehaviour
{
    // Text areas
    public TextMeshProUGUI mildResultsTextBox;
    public TextMeshProUGUI moderateResultsTextBox;
    public TextMeshProUGUI severeResultsTextBox;

    // Lists
    [SerializeField] private List<string> mildResultsList = new List<string>() {"00:01:00", "01:00:00" , "00:00:00", "00:00:01"};
    [SerializeField] private List<string> moderateResultsList = new List<string>() {"00:50:00", "03:00:00"};
    [SerializeField] private List<string> severeResultsList = new List<string>();

    // Last items added
    private string mildResultLastAttempt;
    private string moderateResultLastAttempt;
    private string severeResultLastAttempt;

    // Null Count
    private int mildNullCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        LoadResults();
        
        // Get last attempt
        MildResultsLastAttempt();
        ModerateResultsLastAttempt();
        SevereResultsLastAttempt();
        
        // Sort the lists
        MildResultsSort();
        ModerateResultsSort();
        SevereResultsSort();
        
        // Topup any values that need to be null
        MildResultsLengthCheck();
        ModerateResultsLengthCheck();
        SevereResultsLengthCheck();
  
        // Print Lists
        MildResultsListPrint();
        ModerateResultsListPrint();
        SevereResultsListPrint();

        SaveReults();
    }

    // Catch the last attempt that was added to the list
    private void MildResultsLastAttempt()
    {
        int mildResultsListCount = mildResultsList.Count;
        if(mildResultsListCount > 0)
        {
            mildResultLastAttempt = mildResultsList[mildResultsListCount - 1];
        }
        else {
            mildResultLastAttempt = "--:--:--";
        }
    }
    private void ModerateResultsLastAttempt()
    {
        int moderateResultsListCount = moderateResultsList.Count;
        if(moderateResultsListCount > 0)
        {
            moderateResultLastAttempt = moderateResultsList[moderateResultsListCount - 1];
        }
        else {
            moderateResultLastAttempt = "--:--:--";
        }
    }
    private void SevereResultsLastAttempt()
    {
        int severeResultsListCount = severeResultsList.Count;
        if (severeResultsListCount > 0)
        {
            severeResultLastAttempt = severeResultsList[severeResultsListCount - 1];
        }
        else {
            severeResultLastAttempt = "--:--:--";
        }
    }
    
    // Sort each list
    private void MildResultsSort()
    {
        mildResultsList.Sort();
    }
    private void ModerateResultsSort()
    {
        moderateResultsList.Sort();
    }
    private void SevereResultsSort()
    {
        severeResultsList.Sort();
    }

    // Check the number of items in the list and for any item that isn't there, add the --:--:-- value into the list to top it up to at least 5
    private void MildResultsLengthCheck()
    {
        if(mildResultsList.Count == 0)
        {
            mildResultsList.Add("--:--:--");
            mildResultsList.Add("--:--:--");
            mildResultsList.Add("--:--:--");
            mildResultsList.Add("--:--:--");
            mildResultsList.Add("--:--:--");
        }
        else if(mildResultsList.Count == 1)
        {
            mildResultsList.Add("--:--:--");
            mildResultsList.Add("--:--:--");
            mildResultsList.Add("--:--:--");
            mildResultsList.Add("--:--:--");
        }
        else if(mildResultsList.Count == 2)
        {
            mildResultsList.Add("--:--:--");
            mildResultsList.Add("--:--:--");
            mildResultsList.Add("--:--:--");
        }
        else if(mildResultsList.Count == 3)
        {
            mildResultsList.Add("--:--:--");
            mildResultsList.Add("--:--:--");
        }
        else if(mildResultsList.Count == 4)
        {
            mildResultsList.Add("--:--:--");
        }
    }
    private void ModerateResultsLengthCheck()
    {
        if(moderateResultsList.Count == 0)
        {
            moderateResultsList.Add("--:--:--");
            moderateResultsList.Add("--:--:--");
            moderateResultsList.Add("--:--:--");
            moderateResultsList.Add("--:--:--");
            moderateResultsList.Add("--:--:--");
        }
        else if(moderateResultsList.Count == 1)
        {
            moderateResultsList.Add("--:--:--");
            moderateResultsList.Add("--:--:--");
            moderateResultsList.Add("--:--:--");
            moderateResultsList.Add("--:--:--");
        }
        else if(moderateResultsList.Count == 2)
        {
            moderateResultsList.Add("--:--:--");
            moderateResultsList.Add("--:--:--");
            moderateResultsList.Add("--:--:--");
        }
        else if(moderateResultsList.Count == 3)
        {
            moderateResultsList.Add("--:--:--");
            moderateResultsList.Add("--:--:--");
        }
        else if(moderateResultsList.Count == 4)
        {
            moderateResultsList.Add("--:--:--");
        }
    }
    private void SevereResultsLengthCheck()
    {
        if(severeResultsList.Count == 0)
        {
            severeResultsList.Add("--:--:--");
            severeResultsList.Add("--:--:--");
            severeResultsList.Add("--:--:--");
            severeResultsList.Add("--:--:--");
            severeResultsList.Add("--:--:--");
        }
        else if(severeResultsList.Count == 1)
        {
            severeResultsList.Add("--:--:--");
            severeResultsList.Add("--:--:--");
            severeResultsList.Add("--:--:--");
            severeResultsList.Add("--:--:--");
        }
        else if(severeResultsList.Count == 2)
        {
            severeResultsList.Add("--:--:--");
            severeResultsList.Add("--:--:--");
            severeResultsList.Add("--:--:--");
        }
        else if(severeResultsList.Count == 3)
        {
            severeResultsList.Add("--:--:--");
            severeResultsList.Add("--:--:--");
        }
        else if(severeResultsList.Count == 4)
        {
            severeResultsList.Add("--:--:--");
        }
    }

    // Print the list top 5 and the latest values that was added
    private void MildResultsListPrint()
    {
        mildResultsTextBox.text = 
        "Last attempt" + "\n" +
        mildResultLastAttempt + "\n" + "\n" + "\n" +
        "Personal Best" + "\n" +
        mildResultsList[0] + "\n" +
        mildResultsList[1] + "\n" +
        mildResultsList[2] + "\n" +
        mildResultsList[3] + "\n" +
        mildResultsList[4] + "\n"
        ;
    }
    private void ModerateResultsListPrint()
    {
        moderateResultsTextBox.text = 
        "Last attempt" + "\n" +
        moderateResultLastAttempt + "\n" + "\n" + "\n" +
        "Personal Best" + "\n" +
        moderateResultsList[0] + "\n" +
        moderateResultsList[1] + "\n" +
        moderateResultsList[2] + "\n" +
        moderateResultsList[3] + "\n" +
        moderateResultsList[4] + "\n"
        ;
    }
    private void SevereResultsListPrint()
    {
        severeResultsTextBox.text = 
        "Last attempt" + "\n" +
        severeResultLastAttempt + "\n" + "\n" + "\n" +
        "Personal Best" + "\n" +
        severeResultsList[0] + "\n" +
        severeResultsList[1] + "\n" +
        severeResultsList[2] + "\n" +
        severeResultsList[3] + "\n" +
        severeResultsList[4] + "\n"
        ;
    }
    
    // Save the results to player prefs
    public void SaveReults()
    {
        SaveMildResults();
        SaveModerateResults();
        SaveSevereResults();
    }
    private void SaveMildResults()
    {
        for(int i = 0; i < mildResultsList.Count; i++)
        {
            // Don't add the null string to the Player Prefs
            if(mildResultsList[i] != "--:--:--")
            {
                PlayerPrefs.SetString("MildResults" + i, mildResultsList[i]);
            }
            else
            {
                // Count the amount of null strings
                mildNullCount = mildNullCount + 1;
            }
        }

        // Ensure the null strings are subtracted from the value of the count
        int mildCountSave =  mildResultsList.Count - mildNullCount;
        PlayerPrefs.SetInt("MildCount", mildCountSave);
    }
    private void SaveModerateResults()
    {
        for(int i = 0; i < moderateResultsList.Count; i++)
        {
            PlayerPrefs.SetString("ModerateResults" + i, moderateResultsList[i]);
        }

        PlayerPrefs.SetInt("ModerateCount", moderateResultsList.Count);
    }
    private void SaveSevereResults()
    {
        for(int i = 0; i < severeResultsList.Count; i++)
        {
            PlayerPrefs.SetString("SevereResults" + i, severeResultsList[i]);
        }

        PlayerPrefs.SetInt("SevereCount", severeResultsList.Count);
    }

    // Load results from player prefs. If there are no resuts, nothing will load.
    public void LoadResults()
    {
        LoadMildResults();
        LoadModerateResults();
        LoadSevereResults();
    }
    private void LoadMildResults()
    {
        mildResultsList.Clear();
        int mildSavedListCount = PlayerPrefs.GetInt("MildCount");

        for(int i = 0; i < mildSavedListCount; i++)
        {
            string mildResult = PlayerPrefs.GetString("MildResults" + i);
            mildResultsList.Add(mildResult);
        }
    }
    private void LoadModerateResults()
    {
        moderateResultsList.Clear();
        int moderateSavedListCount = PlayerPrefs.GetInt("ModerateCount");

        for(int i = 0; i < moderateSavedListCount; i++)
        {
            string moderateResult = PlayerPrefs.GetString("ModerateResults" + i);
            moderateResultsList.Add(moderateResult);
        }
    }
    private void LoadSevereResults()
    {
        severeResultsList.Clear();
        int severeSavedListCount = PlayerPrefs.GetInt("SevereCount");

        for(int i = 0; i < severeSavedListCount; i++)
        {
            string severeResult = PlayerPrefs.GetString("SevereResults" + i);
            severeResultsList.Add(severeResult);
        }
    }

    // Reset results from player prefs
    public void ResetResults()
    {
        ResetMildResults();
        ResetModerateResults();
        ResetSevereResults();
    }
    private void ResetMildResults()
    {
        int mildSavedListCount = PlayerPrefs.GetInt("MildCount");

        for(int i = 0; i < mildSavedListCount; i++)
        {
            PlayerPrefs.DeleteKey("MildResults" + i);
        }
        
        PlayerPrefs.DeleteKey("MildCount");
    }
    private void ResetModerateResults()
    {
        int moderateSavedListCount = PlayerPrefs.GetInt("ModerateCount");

        for(int i = 0; i < moderateSavedListCount; i++)
        {
            PlayerPrefs.DeleteKey("ModerateResults" + i);
        }
        
        PlayerPrefs.DeleteKey("ModerateCount");
    }
    private void ResetSevereResults()
    {
        int severeSavedListCount = PlayerPrefs.GetInt("SevereCount");

        for(int i = 0; i < severeSavedListCount; i++)
        {
            PlayerPrefs.DeleteKey("SevereResults" + i);
        }
        
        PlayerPrefs.DeleteKey("SevereCount");
    }
}
