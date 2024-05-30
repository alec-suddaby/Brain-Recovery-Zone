using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountDownCanvasManager : MonoBehaviour
{
    [SerializeField] private TextMeshPro countdownText;
    [SerializeField] private TextMeshPro levelText;

    private void Awake()
    {
        levelText.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void SetDisplay(string timeRemaining, string level = null)
    {
        gameObject.SetActive(true);

        if(levelText != null)
        {
            levelText.text = level;
            levelText.gameObject.SetActive(true);
        }

        countdownText.text = timeRemaining;
    }

    public void CountdownComplete()
    {
        gameObject.SetActive(false);
    }
}
