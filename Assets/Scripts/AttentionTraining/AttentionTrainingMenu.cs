using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AttentionTrainingMenu : MonoBehaviour
{
    public TextMeshProUGUI mildText;

    private AttentionTrainingPBCount attentionTrainingPBCount;
    
    // Start is called before the first frame update
    void Start()
    {
        attentionTrainingPBCount = FindObjectOfType<AttentionTrainingPBCount>();

        if(attentionTrainingPBCount != null)
        {
            PopulatePBs();
        }
    }

    void PopulatePBs()
    {
        mildText.text = attentionTrainingPBCount.timerSaveTest;
    }
}
