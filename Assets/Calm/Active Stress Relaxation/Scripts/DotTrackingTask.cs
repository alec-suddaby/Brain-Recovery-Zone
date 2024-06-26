using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotTrackingTask : TaskCountdown
{
    [SerializeField] private Transform dotTransform;
    [SerializeField] private float yRotationLimit = 45f;
    [SerializeField] private int repsPerSet = 24;
    [SerializeField] private float timeBetweenSets = 10f;
    [SerializeField] private float moveDuration = 1f;
    [SerializeField] private AnimationCurve movementSmoothness;

    public override void InitTask()
    {
        base.InitTask();

        StartCoroutine(DotMovement());
        taskCompleted.AddListener(() => {
            StopAllCoroutines();
        });
    }

    IEnumerator DotMovement()
    {
        int setReps = 0;

        while (!taskComplete)
        {
            yield return StartCoroutine(MoveDot(true));
            yield return StartCoroutine(MoveDot(false));

            setReps++;

            if(setReps >= repsPerSet)
            {
                setReps = 0;
                yield return new WaitForSeconds(timeBetweenSets);
            }
        }
    }

    IEnumerator MoveDot(bool right)
    {
        Quaternion targetRotation = Quaternion.Euler(0f, right ? yRotationLimit : - yRotationLimit, 0f);
        Quaternion oppositeRotation = Quaternion.Euler(0f, right ? - yRotationLimit : yRotationLimit, 0f);
        float timeElapsed = 0f;

        while(timeElapsed < moveDuration)
        {
            float t = movementSmoothness.Evaluate(timeElapsed/moveDuration);
            dotTransform.rotation = Quaternion.Lerp(oppositeRotation, targetRotation, t);
            yield return new WaitForEndOfFrame();
            timeElapsed += Time.deltaTime;
        }
    }
}
