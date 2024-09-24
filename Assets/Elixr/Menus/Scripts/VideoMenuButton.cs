using Elixr.MenuSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

[RequireComponent(typeof(Button))]
public class VideoMenuButton : MonoBehaviour
{
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Description;

    [SerializeField] private TextMeshProUGUI VideoDuration;
    [SerializeField] private int videoDurationMins;
    [SerializeField] private string videoPostfix = "mins";
    [SerializeField] private ProceduralImage videoIconDisplay;

    public virtual void SetupButton(VideoDescription videoDescription)
    {
        if (Title != null)
        {
            Title.text = videoDescription.Title;
        }

        if (Description != null)
        {
            Description.text = videoDescription.Description;
        }

        if (VideoDuration != null)
        {
            VideoDuration.text = $"{videoDescription.DurationMins} {videoPostfix}";
        }

        if (videoDescription.Icon != null)
        {
            videoIconDisplay.sprite = videoDescription.Icon;
        }
    }

    public void ButtonClicked()
    {
        
    }
}
