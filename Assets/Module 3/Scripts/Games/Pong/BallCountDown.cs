using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCountDown : TaskCountdown
{
    public override void InitTask()
    {
        updateTick.RemoveAllListeners();
        taskComplete = false;
        base.InitTask();
        timeElapsed = 0;
    }
}
