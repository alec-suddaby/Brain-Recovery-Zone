using RenderHeads.Media.AVProVideo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalVideoLoader : MonoBehaviour
{
    [SerializeField] private string prefName;

    [SerializeField] private List<ConditionalVideo> conditionalVideos;

    [SerializeField] private MediaPlayer mediaPlayer;

    void Awake()
    {
        int videoId = PlayerPrefs.GetInt(prefName, 1);

        foreach(ConditionalVideo video in conditionalVideos)
        {
            if(video.PrefValue != videoId)
            {
                continue;
            }

            mediaPlayer.OpenVideoFromFile(mediaPlayer.m_VideoLocation, video.VideoPath);
        }
    }
}
