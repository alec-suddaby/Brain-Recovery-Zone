using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonArray : MonoBehaviour
{
    public Color buttonNotSelected;
    public Color buttonSelected;
    public Button[] buttons;
    public Button defaultButton;
    public int selectedIndex;

    void Start(){
        if(defaultButton == null){
            return;
        }
        SelectNewButton(defaultButton);
    }

    public void SelectNewButton(Button button){
        foreach(Button b in buttons){
            b.image.color = buttonNotSelected;
        }

        button.image.color = buttonSelected;

        for(int i = 0; i < buttons.Length; i++){
            if(buttons[i] == button){
                selectedIndex = i;
                return;
            }
        }
    }

    public void SelectNewButton(int index){
        SelectNewButton(buttons[index]);
    }
}
