using UnityEngine.UI;
using TMPro;
using System;

public class TaskCountdown : TimedTask
{
    
    public TextMeshPro countdownText;
    public Slider countdownSlider;
    public bool showCountdownInMins = false;

    public override void InitTask()
    {
        base.InitTask();
        countdownText.text = "";
        updateTick.AddListener(UpdateCountdownText);
        if(countdownSlider != null )
        {
            updateTick.AddListener(UpdateCountdownSlider);
        }
    }

    private void UpdateCountdownSlider()
    {
        countdownSlider.value = timeElapsed / taskLengthSeconds;
    }

    private void UpdateCountdownText(){
        float timeRemaining = taskLengthSeconds - timeElapsed;

        string timeText = ((int)timeRemaining).ToString();
        if(showCountdownInMins){
            int seconds = (int)(timeRemaining % 60);
            int mins = (int)((timeRemaining - seconds)/60);

            timeText = Force2DigitNumber(mins) + ":" + Force2DigitNumber(seconds);
        }

        countdownText.text = (int)timeRemaining > 0 ? timeText : "";
    }

    private string Force2DigitNumber(int num){
        string numberString = num.ToString();
        if(num < 10){
            numberString = "0" + numberString;
        }

        return numberString;
    }
}
