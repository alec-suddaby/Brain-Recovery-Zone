using UnityEngine;

public class PlayMenuAudio : MonoBehaviour
{
    public void PlayMenuAudioClip(AudioClip clip)
    {
        AudioTrigger audioTrigger;
        if (audioTrigger = FindObjectOfType<AudioTrigger>().GetComponent<AudioTrigger>())
        {
            audioTrigger.PlayAudio(clip);
        }
    }
}
