using Elixr.MenuSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoLoaderHandler : MonoBehaviour
{
    public Text titleText;

    public Text descriptionText;

    public Transform buttonParent;

    public VideoMenuButton buttonPrefab;

    public GridLayoutGroup buttonLayout;

    public GameObject audioButton;

    private AudioClip descriptionAudio;

    public virtual void SetupMenu(VideoLoader videoLoader)
    {
        descriptionAudio = videoLoader.descriptionAudio;
        audioButton.SetActive(descriptionAudio != null);

        titleText.text = videoLoader.title;
        descriptionText.text = videoLoader.description;

        if (videoLoader.Videos.Count == 4)
        {
            buttonLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            buttonLayout.constraintCount = 2;
        }

        videoLoader.Videos.ForEach(video =>
        {
            VideoMenuButton button = Instantiate(buttonPrefab.gameObject, buttonParent).GetComponent<VideoMenuButton>();
            button.SetupButton(video);
        });
    }

    public void PlayAudioDescription()
    {
        AudioTrigger audioTrigger;
        if (audioTrigger = FindObjectOfType<AudioTrigger>().GetComponent<AudioTrigger>())
        {
            audioTrigger.PlayAudio(descriptionAudio);
        }
    }
}
