using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelectorTest : MonoBehaviour
{
    public Button[] buttons;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Button button in buttons){
            button.Select();
        }
    }
}
