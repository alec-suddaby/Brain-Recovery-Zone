using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FixedGapAudioTask : AudioCountTask
{
    public List<AlerterSound> alerterSounds;
    public float timeBetweenAudio = 1f;
    public bool allowClipOverlap = true;

    public List<AudioClip> genericAudio;
    public int numberOfGenericAudioClips;
    public List<AudioClip> triggerClips;
    public int numberOfTriggerAudioClips;

    [System.Serializable]
    public class AlerterSound{
        [Range(0,1)]
        public float nextAudioStartPoint = 1f;

        public AudioClip alerterSound;
    }

    public override void InitTask()
    {
        ShuffleList<AudioClip>(genericAudio);
        ShuffleList<AudioClip>(triggerClips);

        AddFromListToAudioTasks(genericAudio, numberOfGenericAudioClips, false);
        AddFromListToAudioTasks(triggerClips, numberOfTriggerAudioClips, true);

        ShuffleList<AudioTaskSound>(audioTaskSounds);

        float alerterSoundsLength = 0;
        foreach(AlerterSound alerterSound in alerterSounds){
            alerterSoundsLength += alerterSound.alerterSound.length * alerterSound.nextAudioStartPoint;
        }

        for(int i = 0; i < audioTaskSounds.Count; i++){
            audioTaskSounds[i].playAtTimeSeconds = (i * timeBetweenAudio);
            if(i == 0){
                continue;
            }
            if(!allowClipOverlap){
                audioTaskSounds[i].playAtTimeSeconds = audioTaskSounds[i - 1].taskAudio.length + audioTaskSounds[i - 1].playAtTimeSeconds + timeBetweenAudio + alerterSoundsLength;
            }
        }

        base.InitTask();

        taskLengthSeconds += alerterSoundsLength;
    }

    private void AddFromListToAudioTasks(List<AudioClip> audioClips, int numberOfClips, bool isTargetSound){
        for(int i = 0; i < numberOfClips; i++){
            AudioClip a = GetNextFromList<AudioClip>(audioClips);

            audioTaskSounds.Add(new AudioTaskSound(a, isTargetSound));
        }
    }

    private void ShuffleList<T>(List<T> list){
        for(int i = 0; i < list.Count; i++){
            for(int j = 0; j < list.Count; j++){
                float swap = Random.Range(0f,1f);
                if(swap >= 0.5){
                    T t = list[j];
                    list[j] = list[i];
                    list[i] = t;
                }
            }
        }
    }

    public T GetNextFromList<T>(List<T> list){
        T temp = list[0];
        list.RemoveAt(0);
        list.Add(temp);
        return temp;
    }

    protected override void PlayAudioClip(AudioClip clip)
    {
        StartCoroutine(playAudioClips(clip));
    }

    private IEnumerator playAudioClips(AudioClip clip){
        foreach(AlerterSound alerter in alerterSounds){
            audioSource.PlayOneShot(alerter.alerterSound);
            yield return new WaitForSeconds(alerter.alerterSound.length * alerter.nextAudioStartPoint);
        }

        audioSource.PlayOneShot(clip);
    }
}
