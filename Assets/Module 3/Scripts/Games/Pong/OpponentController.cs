using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentController : MonoBehaviour
{
    [System.Serializable]
    public class AIDifficulty{
        public int difficultyLevel;
        public float speed;
    }

    public List<AIDifficulty> aIDifficulties;


    public float speed = 0.1f;
    public PongBall ball;

    private Vector3 startPosition;

    public float maxPositionY = 500f;

    private bool ballVisible = true;

    void Start(){
        startPosition = transform.localPosition;
        int difficultyLevel = PlayerPrefs.GetInt("Module3TableTennisAceDifficulty", 2);
        foreach(AIDifficulty difficulty in aIDifficulties){
            if(difficulty.difficultyLevel != difficultyLevel){
                continue;
            }

            speed = difficulty.speed;
        }
    }

    void FixedUpdate()
    {
        float targetY = ball.IsGoingRight || !ballVisible ? 0 : Mathf.Clamp(ball.transform.localPosition.y, -maxPositionY, maxPositionY);
        transform.localPosition = new Vector3(startPosition.x, Mathf.SmoothStep(transform.localPosition.y, targetY, speed), startPosition.z);
    }

    public void ResetPosition(){
        transform.localPosition = startPosition;
    }

    public void BallVisibilityChanged(bool visible){
        ballVisible = visible;
    }
}
