using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberSelector : MonoBehaviour
{

    public int minGuess;
    public int maxGuess;
    private int currentGuess;
    public Text numberGuessed;
    public int GetGuess{
        get => currentGuess;
    }

    void Start(){
        numberGuessed.text = currentGuess.ToString();
    }

    public void ChangeNumber(int change){
        currentGuess = Mathf.Clamp(currentGuess + change, minGuess, maxGuess);
        numberGuessed.text = currentGuess.ToString();
    }
}
