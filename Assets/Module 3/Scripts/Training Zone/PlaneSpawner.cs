using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneSpawner : TimedTask
{
    public GameObject plane;

    public int numberOfPlanes = 5;

    public List<PlaneSpawn> spawnTimes;

    public float minSpawnDistance = 20;
    public float maxSpawnDistance = 100;
    public float minSpawnHeight = 2;
    public float maxSpawnHeight = 50;
    public float maxSpawnRotation = 180;

    public float minDistanceFinish = 50f;
    public float maxDistanceFinish = 165f;

    public float planeSpeed = 1f;
    public int timeBetweenPlanes;

    public class PlanePath{
        private Vector3 spawnPosition;
        public Vector3 StartPosition{
            get => spawnPosition;
        }
        private Quaternion spawnRotation;
        public Quaternion StartRotation{
            get => spawnRotation;
        }

        private List<PlaneMove> planeMoves;
        public List<PlaneMove> Moves{
            get => planeMoves;
        }

        private float planeDuration;
        public float Duration{
            get => planeDuration;
        }

        private float planeFinishDistance;
        public float FinishDistance{
            get => planeFinishDistance;
        }

        public PlanePath(Vector3 startPos, Quaternion startRot, List<PlaneMove> moves, float duration, float finishDistance){
            spawnPosition = startPos;
            spawnRotation = startRot;
            planeMoves = moves;
            planeDuration = duration;
            planeFinishDistance = finishDistance;
        }

    }

    [System.Serializable]
    public class PlaneMove{
        public float duration;
        public float Duration{
            get => duration;
        }
        public float rotation;
        public PlaneMove(float duration, float rotation){
            this.duration = duration;
            this.rotation = rotation;
        }
    }

    [System.Serializable]
    public class PlaneSpawn{
        public float startTime;
        public PlanePath PlanePath;

        public PlaneSpawn(float start, PlanePath path){
            startTime = start;
            PlanePath = path;
        }
    }

    public override void InitTask(){
        base.InitTask();

        updateTick.AddListener(UpdateCall);
        
        spawnTimes = new List<PlaneSpawn>();

        int duration = 0;
        for(int i = 0; i < numberOfPlanes; i++){
            PlanePath planePath = GeneratePlanePath();

            spawnTimes.Add(new PlaneSpawn(duration, planePath));
            duration += (int)planePath.Duration + timeBetweenPlanes;
        }

        taskLengthSeconds = spawnTimes[spawnTimes.Count - 1].startTime + spawnTimes[0].PlanePath.Duration + 2;
    }

    PlanePath GeneratePlanePath(){
        List<int> moveDurations = new List<int>();
        int numberOfMoves = Random.Range(3,6);

        for(int i = 0; i < numberOfMoves; i++){
            moveDurations.Add(Random.Range(10, 16));
        }

        Transform planeSimulator = Instantiate(new GameObject(), transform.position, transform.rotation).transform;

        Vector3 spawnOffset = new Vector3(0, Random.Range(minSpawnHeight, maxSpawnHeight), Random.Range(minSpawnDistance, maxSpawnDistance));
        spawnOffset = Quaternion.Euler(0, Random.Range(-maxSpawnRotation, maxSpawnRotation),0) * spawnOffset;
        planeSimulator.position = spawnOffset;

        planeSimulator.transform.LookAt(new Vector3(transform.position.x, planeSimulator.transform.position.y, transform.position.z));
        planeSimulator.transform.Rotate(new Vector3(0,Random.Range(-30f, 30f),0));
        
        Quaternion spawnRotation = planeSimulator.rotation;

        List<PlaneMove> planeMoves = new List<PlaneMove>();
        float simTime = -Time.fixedDeltaTime;
        float nextMoveChangeTime = 0f;

        float currentRotation = 0f;

        bool complete = false;
        while(!complete){
            simTime += Time.fixedDeltaTime;

            if(simTime > nextMoveChangeTime){
                moveDurations.RemoveAt(0);
                if(moveDurations.Count == 0){
                    complete = true;
                    break;
                }

                nextMoveChangeTime += moveDurations[0];

                currentRotation = (2f * (Vector2.SignedAngle(new Vector2(planeSimulator.forward.x, planeSimulator.forward.z), -new Vector2(planeSimulator.position.x, planeSimulator.position.z))) + Random.Range(-15f,15f));
                //Debug.Log("Angle Required: " + angleToPlayer + " Duration: " + moveDurations[0] + " Rotation Speed: " + currentRotation + " Fixed Delta Time: " + Time.fixedDeltaTime);


                planeMoves.Add(new PlaneMove(moveDurations[0], currentRotation));
            }

            if(simTime >= 0){
                planeSimulator.position += (planeSimulator.forward * planeSpeed);
                planeSimulator.Rotate(new Vector3(0f, currentRotation/moveDurations[0], 0f) * Time.fixedDeltaTime, Space.World);
            }
        }

        float moveAwayTime = 0f;
        float finishDistance = (planeSimulator.position.y - minSpawnHeight)/maxSpawnHeight;
        finishDistance *= maxDistanceFinish - minDistanceFinish;
        finishDistance += minDistanceFinish;

        while(Vector3.Distance(Vector3.zero, planeSimulator.position) < finishDistance){
            moveAwayTime += Time.fixedDeltaTime;
            planeSimulator.position += (planeSimulator.forward * planeSpeed);
        }

        PlanePath planePath = new PlanePath(spawnOffset, spawnRotation, planeMoves, moveAwayTime + simTime, finishDistance);
        
        Destroy(planeSimulator.gameObject);
        return planePath;
    }

    void UpdateCall(){
        if(spawnTimes.Count > 0 && timeElapsed > spawnTimes[0].startTime){
            PlaneMovement newPlane = Instantiate(plane, spawnTimes[0].PlanePath.StartPosition, spawnTimes[0].PlanePath.StartRotation).GetComponent<PlaneMovement>();
            newPlane.moves = spawnTimes[0].PlanePath.Moves;
            newPlane.speed = planeSpeed;
            newPlane.finishDistance = spawnTimes[0].PlanePath.FinishDistance;
            spawnTimes.RemoveAt(0);
        }
    }
    
}
