using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ZoneIn : MonoBehaviour
{
    public RawImage zoneInImage;
    public List<StarSignButton> starSigns;
    private StarSignButton currentSign;
    public StarSignButton CurrentSignButton => currentSign;

    private int score = 0;
    public int Score => score;

    public int rounds = 15;
    private int currentRound = 0;
    public int CurrentRound => currentRound;

    public Text roundText;
    public UnityEvent taskComplete;
    public TextMeshProUGUI scoreText;

    void Start()
    {
        NextImage();
    }

    
    public void CheckCorrect(Icon.IconType type){
        if(type == currentSign.starSign){
            Debug.Log("Correct");
            score++;
        }else{
            Debug.Log("Incorrect");
        }
        

        NextImage();
    }

    public void NextImage(){
        currentRound++;
        if(currentRound > rounds){
            roundText.text = "";
            taskComplete.Invoke();
            scoreText.text = (((float)score/(float)rounds) * 100f).ToString() + "%";
            return;
        }

        roundText.text = "Round " + currentRound.ToString();

        currentSign = starSigns[Random.Range(0, starSigns.Count)];

        zoneInImage.texture = currentSign.icon;
        zoneInImage.transform.localPosition = new Vector2(Random.Range(currentSign.minPos.x, currentSign.maxPos.x), Random.Range(currentSign.minPos.y, currentSign.maxPos.y));
    }
}
