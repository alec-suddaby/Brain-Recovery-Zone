using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconSelector : MonoBehaviour
{
    private Icon[] icons;
    private List<Icon> remainingIcons = new List<Icon>();

    public int RemainingIconCount => remainingIcons.Count;

    public bool allowDeactivation = true;
    private bool iconSelected = false;
    public bool GetIconSelected{
        get => iconSelected;
    }

    public Icon.IconType CurrentIconType{
        get{
            return remainingIcons[0].GetIconType;
        }
    }

    void Awake(){
        int numberOfChildren = transform.childCount;
        for(int i = 0; i < numberOfChildren; i++){
            for(int x = 0; x < numberOfChildren; x++){
                float rand = Random.Range(0f, 1f);
                if(rand > 0.5f){
                    transform.GetChild(i).SetSiblingIndex(x);
                }                
            }
        }

        icons = GetComponentsInChildren<Icon>();

        foreach(Icon icon in icons){
            remainingIcons.Add(icon);
        }
    }

    public void Init(){
        remainingIcons[0].SetIconState = Icon.IconState.Selected;
    }

    public void Next(){
        if(remainingIcons.Count == 0){
            return;
        }
        remainingIcons[0].SetIconState = Icon.IconState.Normal;
        Icon tempIcon = remainingIcons[0];
        remainingIcons.RemoveAt(0);
        remainingIcons.Add(tempIcon);
        remainingIcons[0].SetIconState = Icon.IconState.Selected;
    }

    public bool DeactivateCurrent(bool autoSwitch, bool p_allowDeactivation){
        if(!allowDeactivation){
            return true;
        }

        iconSelected = false;

        remainingIcons[0].SetIconState = Icon.IconState.Deactivated;
        remainingIcons[0].PlayParticles();
        remainingIcons.RemoveAt(0);
        
        int completeAmount = p_allowDeactivation ? 1 : 0;
        if(remainingIcons.Count > completeAmount){
            if(autoSwitch){
                remainingIcons[0].SetIconState = Icon.IconState.Selected;
            }
            return true;
        }
        return false;
    }

    public void SelectIcon(Icon targetIcon){
        if(!remainingIcons.Contains(targetIcon)){
            return;
        }
        while(remainingIcons[0] != targetIcon){
            remainingIcons[0].SetIconState = Icon.IconState.Normal;
            Icon tempIcon = remainingIcons[0];
            remainingIcons.RemoveAt(0);
            remainingIcons.Add(tempIcon);
        }
        
        remainingIcons[0].SetIconState = Icon.IconState.Selected;
        iconSelected = true;
    }
}
