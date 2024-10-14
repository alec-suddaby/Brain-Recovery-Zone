using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReminderCanvas : MonoBehaviour
{
    [SerializeField] private GameObject reminderCanvas;

    public void SetReminderState(bool display)
    {
        reminderCanvas.SetActive(display);
    }
}
