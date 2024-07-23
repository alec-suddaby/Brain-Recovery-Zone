using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class DiceRollTask : MonoBehaviour
{
    [System.Serializable]
    public struct AdditionalDice{
        public ReplayDice dice;
        public DieType type;
        public int introductoryRound;
    }

    public enum DieType{
        Master,
        Comparison
    }

    public List<AdditionalDice> additionalDice;

    public List<ReplayDice> masterDice;
    public List<ReplayDice> comparisonDice;
    [Range(0,1)]
    public float matchingDiceFrequency = 0.5f;
    private bool matching = false;
    private int roundsCompleted = 0;
    public int RoundsCompleted => roundsCompleted;
    public int rounds = 5;
    private int score = 0;
    public int Score => score;

    public UnityEvent taskComplete;

    public GameObject completeCanvas;
    private bool started = false;

    public UnityEvent rollStarted;
    public UnityEvent rollComplete;

    public TextMeshProUGUI scoreText;

    public string singleDieText = "die";
    public string multipleDiceText = "dice";
    public TextMeshProUGUI inGameMessage;

    void Start(){
        taskComplete.AddListener(DisplayScore);
    }

    void DisplayScore(){
        scoreText.text = (((float)score/(float)rounds) * 100f).ToString() + "%";
    }

    void LateUpdate(){
        if(roundsCompleted == rounds || !started){
            return;
        }
        foreach(ReplayDice dice in masterDice){
            if(!dice.complete){
                return;
            }
        }

        foreach(ReplayDice dice in comparisonDice){
            if(!dice.complete){
                return;
            }
        }
        if(!completeCanvas.activeSelf){
            rollComplete.Invoke();
        }

        completeCanvas.SetActive(true);
    }

    public void Next(){
        List<ReplayDice> tempComparison = new List<ReplayDice>();
        List<ReplayDice> tempMaster = new List<ReplayDice>();
        foreach(AdditionalDice additionalDie in additionalDice){
            if(roundsCompleted != additionalDie.introductoryRound - 1){
                continue;
            }

            switch(additionalDie.type){
                case DieType.Comparison:
                    comparisonDice.Add(additionalDie.dice);
                    break;
                case DieType.Master:
                    masterDice.Add(additionalDie.dice);
                    break;
            }
        }

        List<int> masterDiceValues = new List<int>();
        List<int> comparisonDiceValues = new List<int>();

        inGameMessage.text = masterDice.Count > 1 ? multipleDiceText : singleDieText;

        while(masterDiceValues.Count < masterDice.Count){
            int randomFace = Random.Range(0, 6);
            if(!masterDiceValues.Contains(randomFace)){
                masterDiceValues.Add(randomFace);
            }
        }

        float match = Random.Range(0f, 1f);
        matching = match < matchingDiceFrequency;
        if(matching){
            comparisonDiceValues.Add(masterDiceValues[0]);
        }

        while(comparisonDiceValues.Count < comparisonDice.Count){
            int randomFace = Random.Range(0, 6);
            if(!matching && masterDiceValues.Contains(randomFace)){
                continue;
            }
            comparisonDiceValues.Add(randomFace);
        }

        for(int i = 0; i < comparisonDiceValues.Count; i++){
            for(int j = 0; j < comparisonDiceValues.Count - i; j++){
                if(i == j){
                    continue;
                }

                if(Random.Range(0f,1f) > 0.5f){
                    int temp = comparisonDiceValues[i];
                    comparisonDiceValues[i] = comparisonDiceValues[j];
                    comparisonDiceValues[j] = temp;
                }
            }
        }

        RollDice(masterDice, masterDiceValues.ToArray());
        RollDice(comparisonDice, comparisonDiceValues.ToArray());
        rollStarted.Invoke();
        started = true;
    }

    public void CheckCorrect(bool isMatching){
        if(roundsCompleted == rounds){
            return;
        }

        if(isMatching == matching){
            score++;
        }

        roundsCompleted++;

        if(roundsCompleted == rounds){
            taskComplete.Invoke();
            return;
        }

        Next();
    }

    void RollDice(List<ReplayDice> dice, int[] values){
        #if UNITY_EDITOR
            string storageLocation = (Application.dataPath + Path.AltDirectorySeparatorChar + "Data/DiceRolls/" + dice.Count);
        #else
            string storageLocation = "/storage/emulated/0/BRZ_ASSETS/files/Dice/" + dice.Count;      
        #endif

        string[] files = Directory.GetFiles(storageLocation);  
        
        string selectedFile = files[Random.Range(0, files.Length)];
        string[] path = selectedFile.Split(new char[] {'\\','/'});
        string[] fileName = path[path.Length - 1].Split(new char[]{'d'});

        for(int i = 0; i < dice.Count; i++){
            dice[i].gameObject.SetActive(true);
            dice[i].LoadDiceRoll(storageLocation + "//" + fileName[0] + "d" + i + ".json", values[i]);
        }
        Debug.Log(selectedFile);
    }
}
