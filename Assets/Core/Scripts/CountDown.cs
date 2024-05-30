using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    private float startTime;
    public float countDownTime;
    public Text countDownText;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        int timeRemaining = (int)(startTime + countDownTime - Time.time);

        countDownText.text = timeRemaining > 0 ? timeRemaining.ToString():"";
    }
}
