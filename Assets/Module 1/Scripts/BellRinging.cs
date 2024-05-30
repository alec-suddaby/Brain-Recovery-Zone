using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BellRinging : MonoBehaviour
{
    public GameObject bellAudio;

    public List<int> ringTimes;
    private float startTime;
    public int startDelay = 5;
    public float programmeLength;
    public UnityEvent completeSession;
    private int numberOfRings;

    void Start(){
        startTime = Time.time + startDelay;
        numberOfRings = ringTimes.Count;
    }

    void Update(){
        if(ringTimes.Count > 0 && ringTimes[0] < Time.time - startTime){
            Destroy((GameObject)Instantiate(bellAudio), 4f);
            ringTimes.RemoveAt(0);
        }
        if(Time.time - startTime > programmeLength){
            completeSession.Invoke();
            Destroy(gameObject);
        }
    }

    public int NumberOfRings{
        get{
            return numberOfRings;
        }
    }
}
