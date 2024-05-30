using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float startTime = 0f;

    public void StartTimer(){
        startTime = Time.time;
    }

    public string Score()
    {
        float timeRemaining = Time.time - startTime;
        int seconds = (int)(timeRemaining % 60);
        int mins = (int)((timeRemaining - seconds)/60);

        return Force2DigitNumber(mins) + ":" + Force2DigitNumber(seconds);
    }

        private string Force2DigitNumber(int num){
        string numberString = num.ToString();
        if(num < 10){
            numberString = "0" + numberString;
        }

        return numberString;
    }
}
