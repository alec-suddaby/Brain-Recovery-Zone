using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StarSpawner : TaskCountdown
{
    public GameObject[] shootingStars;
    public GameObject dummyStar;

    public int numberOfStars = 5;

    public List<TargetSpawn> spawnTimes;

    public Vector2 minSpawnRotation;
    public Vector2 maxSpawnRotation;

    public float starSpeed = 1f;
    public int timeBetweenStars;

    public int minDuration = 5;
    public int maxDuration = 10;

    public float timeForSuccessfulTriggerPress = 5f;

    public TextMeshProUGUI scoreText;

    public IntervalBillboard highlighter;
    private bool highlighterDeleted = true;
    public bool highlighterEnabled = true;

    public float score = 0;

    public bool starsMustPassObstacle = false;

    private int lastSpawnedStarIndex = -1;

    public GameObject selectionCanvas;
    public float selectionCanvasDelay = 2.5f;

    public bool dummyStarsEnabled = true;

    public string blindSpotFileName = "BlindSpots";
    private List<BlindSpotTask> blindSpots;
    public float targetRotations = 25f;
    public Dictionary<EyePatch.Eye, List<Vector3>> spawnPositions;
    public Dictionary<EyePatch.Eye, List<Vector3>> targetPositions;
    public EyePatch.Eye currentEye;


    [System.Serializable]
    public class StarObstacle{
        public BlindSpotAssessment.TargetPosition targetPosition;
        public GameObject obstacle;
    }
    public List<StarObstacle> obstacles;

    [System.Serializable]
    public class TargetSpawn{
        public float startTime;
        public float duration;
        public Vector3 startRotation;
        public Vector3 direction;
        public float speed;

        public TargetSpawn(float start, float dur, Vector3 startRotation, Vector3 direction, float speed){
            startTime = start;
            duration = dur;
            this.startRotation = startRotation;
            this.direction = direction;
            this.speed = speed;

            while(ShootingStar.FinishesInBadPosition(startRotation, duration, direction.normalized * speed)){
                duration += 0.25f;
                // Debug.Log("Duration Increased");
            }
        }
    }

    private List<float> expectedTriggerPressTimes = new List<float>();

    public override void InitTask(){
        base.InitTask();

        minDuration *= 8;
        minDuration /= (int)starSpeed;

        maxDuration *= 8;
        maxDuration /= (int)starSpeed;

        if(PlayerPrefs.HasKey("Module3StarsEye"))
            currentEye = (EyePatch.Eye)PlayerPrefs.GetInt("Module3StarsEye");

        blindSpots = LoadBlindSpot.LoadBlindSpotAssessment(blindSpotFileName);
        GenerateKeyPoints();

        foreach(BlindSpotTask blindSpot in blindSpots){
            if(blindSpot.eye != currentEye){
                continue;
            }

            foreach(StarObstacle obstacle in obstacles){
                if(obstacle.targetPosition == blindSpot.targetPosition){
                    obstacle.obstacle.SetActive(blindSpot.isBlindSpot);
                    break;
                }
            }
        }

        updateTick.AddListener(UpdateCall);
        
        spawnTimes = new List<TargetSpawn>();

        int startTime = 0;
        for(int i = 0; i < numberOfStars; i++){
            int duration = Random.Range(minDuration, maxDuration + 1);

            Vector3 startPosition = spawnPositions[currentEye][Random.Range(0, spawnPositions[currentEye].Count)];
            Vector3 targetDirection = targetPositions[currentEye][Random.Range(0, targetPositions[currentEye].Count)];
            targetDirection.x -= startPosition.x;
            targetDirection.y -= startPosition.y;

            TargetSpawn targetSpawn = new TargetSpawn(startTime, duration, startPosition, targetDirection, starSpeed);
            spawnTimes.Add(targetSpawn);
            startTime += (int)targetSpawn.duration + timeBetweenStars;
        }

        taskLengthSeconds = spawnTimes[spawnTimes.Count - 1].startTime + spawnTimes[spawnTimes.Count - 1].duration + timeBetweenStars;
        taskCompleted.AddListener(CheckScore);

        highlighter.gameObject.SetActive(highlighterEnabled);
        score = 0f;

        if(selectionCanvas != null){
            selectionCanvas.SetActive(false);
        }

        if(dummyStarsEnabled){
            StartCoroutine("DummyStars");
            taskCompleted.AddListener(Complete);
        }
    }

    IEnumerator ShowSelectionCanvas(){
        yield return new WaitForSeconds(selectionCanvasDelay);

        selectionCanvas.SetActive(true);
    }

    void UpdateCall(){
        if(spawnTimes.Count > 0 && timeElapsed > spawnTimes[0].startTime){
            highlighterDeleted = false;
            
            if(selectionCanvas != null){
                selectionCanvas.SetActive(false);
                StartCoroutine("ShowSelectionCanvas");
            }

            lastSpawnedStarIndex = Random.Range(0, shootingStars.Length);
            ShootingStar star = Instantiate(shootingStars[lastSpawnedStarIndex], transform.position, transform.rotation).GetComponent<ShootingStar>();

            star.transform.Rotate(spawnTimes[0].startRotation);

            star.duration = spawnTimes[0].duration;
            star.Speed = starSpeed;            
            star.spawner = this;
            star.RotationAmount = spawnTimes[0].direction;

            highlighter.Target = star.trailObject;                

            expectedTriggerPressTimes.Add(spawnTimes[0].startTime + spawnTimes[0].duration);
            spawnTimes.RemoveAt(0);
        }

        if(spawnTimes.Count > 0 && timeElapsed > spawnTimes[0].startTime - (timeBetweenStars/2f) && !highlighterDeleted){
            highlighterDeleted = true;
            highlighter.EndCurrent();
        }
    }

    private void GenerateKeyPoints(){
        targetPositions = new Dictionary<EyePatch.Eye, List<Vector3>>();
        spawnPositions = new Dictionary<EyePatch.Eye, List<Vector3>>();
        foreach(BlindSpotTask blindSpot in blindSpots){
            if(blindSpot.isBlindSpot){
                if(!targetPositions.ContainsKey(blindSpot.eye)){
                    targetPositions[blindSpot.eye] = new List<Vector3>();
                }

                targetPositions[blindSpot.eye].Add(GetTargetRotation(blindSpot.targetPosition));

                continue;
            }

            if(!spawnPositions.ContainsKey(blindSpot.eye)){
                spawnPositions[blindSpot.eye] = new List<Vector3>();
            }

            spawnPositions[blindSpot.eye].Add(GetTargetRotation(blindSpot.targetPosition));
        }

        if(spawnPositions.Count == 0 || spawnPositions[currentEye] == null || spawnPositions[currentEye].Count == 0){
            spawnPositions = targetPositions;
        }else if(targetPositions.Count == 0 || targetPositions[currentEye] == null || targetPositions[currentEye].Count == 0){
            targetPositions = spawnPositions;
        }

        Debug.Log("Spawn positions: " + spawnPositions[currentEye].Count);
        Debug.Log("Target positions: " + targetPositions[currentEye].Count);

        // Debug.Log(spawnPositions[EyePatch.Eye.LeftEye].Count + " Left Eye Spawn Position(s)");
        // Debug.Log(spawnPositions[EyePatch.Eye.RightEye].Count + " Right Eye Spawn Position(s)");

        // Debug.Log(targetPositions[EyePatch.Eye.LeftEye].Count + " Left Eye Target Position(s)");
        // Debug.Log(targetPositions[EyePatch.Eye.RightEye].Count + " Right Eye Target Position(s)");
    }

    private Vector3 GetTargetRotation(BlindSpotAssessment.TargetPosition eye){
        float otherTargetRotations = Mathf.Sqrt( Mathf.Pow(targetRotations, 2)/2f);
        Vector3 rotation = new Vector3();
        switch(eye){
            case BlindSpotAssessment.TargetPosition.N:
                rotation.x = -targetRotations;
                break;
            case BlindSpotAssessment.TargetPosition.E:
                rotation.y = targetRotations;
                break;
            case BlindSpotAssessment.TargetPosition.S:
                rotation.x = targetRotations;
                break;
            case BlindSpotAssessment.TargetPosition.W:
                rotation.y = -targetRotations;
                break;
            case BlindSpotAssessment.TargetPosition.NE:
                rotation.x = -otherTargetRotations;
                rotation.y = otherTargetRotations;
                break;
            case BlindSpotAssessment.TargetPosition.SE:
                rotation.x = otherTargetRotations;
                rotation.y = otherTargetRotations;
                break;
            case BlindSpotAssessment.TargetPosition.SW:
                rotation.x = otherTargetRotations;
                rotation.y = -otherTargetRotations;
                break;
            case BlindSpotAssessment.TargetPosition.NW:
                rotation.x = -otherTargetRotations;
                rotation.y = -otherTargetRotations;
                break;
        }

        return rotation;
    }

    void CheckScore(){
        score /= (float)numberOfStars;
        score *= 100;
        scoreText.text = ((int)score).ToString() + "%";

        highlighter.EndCurrent();
    }

    public void StarType(int index){
        if(index == lastSpawnedStarIndex){
            score ++;
        }

        selectionCanvas.SetActive(false);
    }

    private void Complete(){
        StopCoroutine("DummyStars");
    }
    public IEnumerator DummyStars(){
        while(true){
            int duration = Random.Range(minDuration, maxDuration);

            Vector3 startPosition = spawnPositions[currentEye][Random.Range(0, spawnPositions[currentEye].Count)];
            Vector3 targetDirection = targetPositions[currentEye][Random.Range(0, targetPositions[currentEye].Count)];
            targetDirection.x -= startPosition.x;
            targetDirection.y -= startPosition.y;

            DummyStar currentDummyStar = Instantiate(dummyStar, transform.position, transform.rotation).GetComponent<DummyStar>();
            currentDummyStar.rotationSpeed = starSpeed;
            currentDummyStar.MovementDirection = targetDirection;
            currentDummyStar.duration = duration;
            currentDummyStar.gameObject.transform.Rotate(startPosition);

            yield return new WaitForSeconds(duration);
        }
    }
}
