using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using TMPro;

public class PlaneScanningTask : TaskCountdown
{
    public GameObject plane;
    public GameObject dummyPlane;

    public int numberOfPlanes = 5;

    public List<PlaneSpawn> spawnTimes;

    public float minSpawnDistance = 20;
    public float maxSpawnDistance = 100;
    public float minSpawnHeight = 2;
    public float maxSpawnHeight = 50;
    public float minSpawnRotation = 10;
    public float maxSpawnRotation = 180;

    public float minTurningDistance = 18;
    public float maxTurningDistance = 20;
    public float finishDistance = 50f;

    public float planeSpeed = 1f;
    public int timeBetweenPlanes;

    public int minPlaneDuration = 25;
    public int maxPlaneDuration = 35;

    public float timeForSuccessfulTriggerPress = 5f;

    public float dummyPlaneSpawnFrequency = 20f;
    public float dummyPlaneSpawnDuration = 40f;
    private float nextDummySpawnTime = 0f;
    public bool dummyPlanesEnabled = true;

    public TextMeshProUGUI scoreText;

    public TriggerPressCounter triggerCounter;

    [System.Serializable]
    public class PlaneSpawn{
        public float startTime;
        public float duration;

        public PlaneSpawn(float start, float dur){
            startTime = start;
            duration = dur;
        }
    }

    private List<float> expectedTriggerPressTimes = new List<float>();
    public int ExpectedTriggerPresses => expectedTriggerPressTimes.Count;

    public override void InitTask(){
        base.InitTask();

        updateTick.AddListener(UpdateCall);
        
        spawnTimes = new List<PlaneSpawn>();

        int startTime = 0;
        for(int i = 0; i < numberOfPlanes; i++){
            int duration = Random.Range(minPlaneDuration, maxPlaneDuration + 1);
            spawnTimes.Add(new PlaneSpawn(startTime, duration));
            startTime += duration + timeBetweenPlanes;
        }

        taskLengthSeconds = spawnTimes[spawnTimes.Count - 1].startTime + spawnTimes[0].duration + timeBetweenPlanes;
        taskCompleted.AddListener(SetScoreDisplay);
        nextDummySpawnTime = dummyPlaneSpawnFrequency/2f;
    }

    void UpdateCall(){
        if(spawnTimes.Count > 0 && timeElapsed > spawnTimes[0].startTime){
            PlaneMovementByDistance planeSimulator = SpawnNewPlane(plane);

            expectedTriggerPressTimes.Add(spawnTimes[0].startTime + spawnTimes[0].duration);
            planeSimulator.timeLimit = spawnTimes[0].duration;
            spawnTimes.RemoveAt(0);
        }

        if(dummyPlanesEnabled && nextDummySpawnTime < timeElapsed && taskLengthSeconds - timeElapsed > dummyPlaneSpawnDuration){
            nextDummySpawnTime += dummyPlaneSpawnFrequency;

            PlaneMovementByDistance dummy = SpawnNewPlane(dummyPlane);

            dummy.timeLimit = dummyPlaneSpawnDuration;
        }
    }

    PlaneMovementByDistance SpawnNewPlane(GameObject planeObject){
        PlaneMovementByDistance planeSimulator = Instantiate(planeObject, transform.position, transform.rotation).GetComponent<PlaneMovementByDistance>();

        Vector3 spawnOffset = new Vector3(0, Random.Range(minSpawnHeight, maxSpawnHeight), Random.Range(minSpawnDistance, maxSpawnDistance));

        float yRotation = Random.Range(minSpawnRotation, maxSpawnRotation);
        yRotation *= Random.Range(0f, 1f) > 0.5f ? -1 : 1;
        spawnOffset = Quaternion.Euler(0, yRotation,0) * spawnOffset;
        planeSimulator.transform.position += spawnOffset;

        planeSimulator.transform.LookAt(new Vector3(transform.position.x, planeSimulator.transform.position.y, transform.position.z));
        planeSimulator.transform.Rotate(new Vector3(0,Random.Range(-30f, 30f),0));

        planeSimulator.speed = planeSpeed;
        planeSimulator.finalDistance = finishDistance;
        planeSimulator.origin = transform;
        planeSimulator.minTurningDistance = minTurningDistance;
        planeSimulator.maxDistance = maxTurningDistance;

        return planeSimulator;
    }

    public void SetScoreDisplay()
    {
        scoreText.text = ((int)CheckScore()).ToString();
    }

    public float CheckScore(){
        float score = 0;
        List<float> triggerPresses = triggerCounter.GetButtonPresses;

        int numberOfTriggerPresses = triggerPresses.Count;
        for(int i = 0; i < expectedTriggerPressTimes.Count; i++){
            for(int x = 0; x < triggerPresses.Count; x++){
                if(Mathf.Abs(expectedTriggerPressTimes[i] - triggerPresses[x]) <= timeForSuccessfulTriggerPress){
                    score++;
                    triggerPresses.RemoveAt(x);
                    break;
                }
            }
        }

        score /= (float)numberOfTriggerPresses;
        score *= 100;
        return score;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, finishDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxTurningDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, minTurningDistance);
    }
}
