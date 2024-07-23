using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BlindSpotAssessment : MonoBehaviour
{
    public enum TargetPosition{
        Center,
        N,
        NE,
        E,
        SE,
        S,
        SW,
        W,
        NW
    }

    [System.Serializable]
    public class BlindSpotTarget{
        public GameObject target;
        public TargetPosition targetPosition;
        public Dictionary<string, int> hits;

        public BlindSpotTarget(){
            hits = new Dictionary<string, int>();
        }

        public void GetScore(string name){
            if(!hits.ContainsKey(name)){
                hits[name] = 0;
            }

            if(!target.activeSelf){
                hits[name]++;
            }
        }

        public void SetActive(){
            target.SetActive(true);
        }
    }

    public UnityEvent subroundStarted;
    public UnityEvent subroundEnded;

    public UnityEvent TaskComplete;
    public bool disableOnComplete = true;
    public List<BlindSpotTarget> targets;
    public int rounds = 3;
    private int roundsComplete = 0;
    public int minScore = 2;
    public List<EyePatch.Eye> visibleEyeSubrounds;
    public EyePatch eye;

    private int currentSubround = -1;

    private bool complete = false;

    public void GetScores(){
        foreach(BlindSpotTarget target in targets){
            target.GetScore(visibleEyeSubrounds[currentSubround].ToString());
        }
        subroundEnded.Invoke();
        Next();
    }

    public void Next(){
        if(complete){
            return;
        }

        currentSubround++;
        if(currentSubround >= visibleEyeSubrounds.Count){
            currentSubround = 0;
            roundsComplete++;

            if(roundsComplete >= rounds){
                TaskComplete.Invoke();
                complete = true;
                gameObject.SetActive(!disableOnComplete);
                return;
            }
        }

        StartCoroutine(ContinueTask());
    }

    #if UNITY_EDITOR
    void Update(){
        if(Input.GetKeyUp(KeyCode.Return)){
            GetScores();
        }
    }
    #endif

    IEnumerator ContinueTask(){
        
        for(int i = 0; i < targets.Count; i++){
            targets[i].target.SetActive(false);
        }
        
        Debug.Log(currentSubround);
        eye.SetEyePatch(visibleEyeSubrounds[currentSubround]);

        yield return new WaitForSeconds(2);

        for(int i = 0; i < targets.Count; i++){
            targets[i].target.SetActive(true);
        }
        
        subroundStarted.Invoke();
    }
}
