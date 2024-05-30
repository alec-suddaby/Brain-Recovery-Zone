using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveAudioClips : AudioCountTask
{
    public int maxClipsToRemove = 0;

    public override void InitTask()
    {
        int rand = Random.Range(0, maxClipsToRemove + 1);

        for(int i = 0; i < rand; i++){
            //Never remove last clip so task length is always correct
            int rand2 = Random.Range(0, audioTaskSounds.Count - 1);

            audioTaskSounds.RemoveAt(rand2);
        }

        base.InitTask();
    }
}
