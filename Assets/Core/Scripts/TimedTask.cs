using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimedTask : MonoBehaviour
{
    public LevelName levelName = new LevelName();
    public UnityEvent taskBegun;
    public UnityEvent taskCompleted;

    public float taskLengthSeconds;
    public bool startOnSceneLoad = false;
    public bool timeLimitEnabled = true;

    public float GetTimeElapsed{
        get => timeElapsed;
    }
    protected float timeElapsed;
    protected bool beginTask = false;
    protected bool taskComplete = false;

    protected UnityEvent updateTick = new UnityEvent();
    public bool disableOnComplete = true;

    void Start(){
        Setup();
    }

    protected virtual void Setup(){
        if(startOnSceneLoad){
            InitTask();
        }
    }

    public virtual void InitTask(){
        beginTask = true;
        taskBegun.Invoke();
        updateTick.AddListener(Tick);
    }

    private void Update(){
        if(!beginTask){
            return;
        }
        updateTick.Invoke();
    }

    protected virtual void Tick(){
        if(!beginTask){
            return;
        }

        timeElapsed += Time.deltaTime;
        CheckTaskComplete();
    }

    protected virtual void CheckTaskComplete(){
        if(taskComplete || !timeLimitEnabled){
            return;
        }
        if(timeElapsed >= taskLengthSeconds){
            TaskCompleted();
        }
    }

    protected void TaskCompleted(){
        taskCompleted.Invoke();
        taskComplete = true;
        timeElapsed = float.MaxValue;
        if(disableOnComplete){
            this.enabled = false;
        }
    }
}
