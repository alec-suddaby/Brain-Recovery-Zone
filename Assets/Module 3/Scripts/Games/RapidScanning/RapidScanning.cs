using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class RapidScanning : MonoBehaviour
{
    public UnityEvent taskComplete;

    public Texture targetIcon;
    public List<Texture> similarIcons;
    public List<IconSwitcher> images;

    private bool containsTarget;
    
    [Range(0f,1f)]
    public float targetFrequency = 0.5f;

    private float score = 0;
    public float Score => score;

    public int numberOfRounds = 15;
    private int currentRound = 1;
    public int CurrentRound => currentRound;

    public Text roundText;
    public TextMeshProUGUI scoreText;

    void Start(){
        Next();
    }

    public void CheckAnswer(bool answer){
        if(answer == containsTarget){
            score++;
        }

        if(currentRound < numberOfRounds){
            currentRound++;
            Next();
            return;
        }

        scoreText.text = (((float)score/(float)numberOfRounds) * 100f).ToString() + "%";
        taskComplete.Invoke();
    }

    public void Next(){
        
        roundText.text = "Round " + currentRound.ToString(); 

        containsTarget = Random.Range(0f, 1f) <= targetFrequency;

        List<Texture> currentTextures = new List<Texture>();

        if(containsTarget){
            currentTextures.Add(targetIcon);
        }

        List<Texture> possibleTextures = similarIcons.ToList();
        while(currentTextures.Count < images.Count){
            int index = Random.Range(0, possibleTextures.Count);
            currentTextures.Add(possibleTextures[index]);
            possibleTextures.RemoveAt(index);
        }

        Shuffle<Texture>(currentTextures);

        for(int i = 0; i < currentTextures.Count; i++){
            images[i].SetIcon(currentTextures[i]);
        }
    }

    void Shuffle<T>(List<T> l){
        for(int i = 0; i < l.Count; i++){
            for(int j = 0; j < l.Count; j++){
                if(i == j){
                    continue;
                }
                
                if(Random.Range(0f,1f) > 0.5f){
                    T temp = l[j];
                    l[j] = l[i];
                    l[i] = temp;
                }
            }
        }
    }
}
