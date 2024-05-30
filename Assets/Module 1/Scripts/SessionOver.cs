using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SessionOver : MonoBehaviour
{
    public GameObject sessionFinishedCanvas;

    private int numberOfBellRings = 0;
    public int minBellRings = 0;
    public int maxBellRings = 0;

    public Text bellRingText;
    public Text actualNumberOfBellRings;
    public WarmUpZoneDifficulty difficulty;

    public GameObject correct;
    public GameObject incorrect;

    public void EnableCanvas(bool enable){
        sessionFinishedCanvas.SetActive(enable);

        ChangeBellRingGuess(0);
    }

    public void EnableCanvas(){
        sessionFinishedCanvas.SetActive(true);
    }

    public void ChangeBellRingGuess(int amount){
        numberOfBellRings = Mathf.Clamp(amount + numberOfBellRings, minBellRings, maxBellRings);
        bellRingText.text = numberOfBellRings.ToString();
    }

    public void CheckIfGuessIsCorrect(){
        actualNumberOfBellRings.text = ("The bells rang " + difficulty.bellRinging.NumberOfRings.ToString() + " times");
        if(numberOfBellRings == difficulty.bellRinging.NumberOfRings){
            correct.SetActive(true);
        }else{
            incorrect.SetActive(true);
        }
    }
}
