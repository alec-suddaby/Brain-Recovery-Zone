using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReminderText : MonoBehaviour
{
    public TextMeshProUGUI reminderText;
    public string playerPref = "";
    public string reminder = "";

    private bool alwaysOn = false;

    public void SetReminderText(string reminder)
    {
        this.reminder = reminder;
        reminderText.text = this.reminder;
    }

    public void Init()
    {
        GameObject.FindObjectOfType<ButtonPressed>().OnButtonPressed.AddListener(SetReminder);

        alwaysOn = !PlayerPrefs.HasKey(playerPref) || PlayerPrefs.GetInt(playerPref) == 1;

        SetReminderText(reminder);
        SetReminder();
    }

    public void InitWithoutPrefs()
    {
        InitWithoutPrefs(reminder);
    }

    public void InitWithoutPrefs(string reminder)
    {
        SetReminderText(reminder);

        GameObject.FindObjectOfType<ButtonPressed>().OnButtonPressed.AddListener(SetReminder);
        alwaysOn = false;
    }

    private void SetReminder()
    {
        if (alwaysOn)
        {
            GameObject.FindObjectOfType<ReminderCanvas>().SetReminderState(true);
            reminderText.text = reminder;
        }
    }

    public void SetReminder(bool state)
    {
        GameObject.FindObjectOfType<ReminderCanvas>().SetReminderState(state || alwaysOn);
    }
}
