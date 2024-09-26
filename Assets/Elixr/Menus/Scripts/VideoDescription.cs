using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Video", menuName = "Elixr/ID/Video", order = 0)]
public class VideoDescription : ScriptableObject
{
    [SerializeField] private string title;
    public string Title => title;

    [SerializeField] private string description;
    public string Description => description;

    [SerializeField] private int durationMins;
    public int DurationMins => durationMins;

    [SerializeField] private Sprite icon;
    public Sprite Icon => icon;

    [SerializeField] private string videoFilePath;
    public string VideoFilePath => videoFilePath;

    [SerializeField] private bool showLikertScale = true;
    public bool ShowLikertScale => showLikertScale;

    [SerializeField] private string videoScene;
    public string VideoScene => videoScene;
}
