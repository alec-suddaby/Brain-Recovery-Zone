using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConditionalVideo
{
    [SerializeField] private int prefValue;
    public int PrefValue => prefValue;

    [SerializeField] private string videoPath;
    public string VideoPath => videoPath;
}
