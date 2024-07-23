using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PongBall : MonoBehaviour
{
    [System.Serializable]
    public class BallDifficulty{
        public int difficultyLevel;
        public float speed;
        public float speedIncrease = 0.1f;
        public float maxSpeed;
    }

    public List<BallDifficulty> ballDifficulties;

    private Rigidbody2D rb;
    private bool goingRight;
    public bool IsGoingRight{
        get => rb.velocity.x > 0;
    }

    public float speed = 5f;
    public float speedIncrease = 0.1f;
    public float maxSpeed = 8f;
    private float currentBallSpeed;

    public bool opponentInGame = true;

    public int maxPositionX = 1250;

    [Range(0,70)]
    public float maxBallAngle = 70f;

    
    private Vector2 startPosition;

    public float maxCollisionDistanceWithPaddle = 190f;
    
    public UnityEvent collided;

    public UnityEvent goalScoredLeft;
    public UnityEvent goalScoredRight;

    public Vector2 CurrentVelocity{
        set{
            currentVelocity = value;
        }
    }
    private Vector2 currentVelocity;

    public AudioSource source;
    public AudioClip bounce;

    public bool loadDifficultyPrefs = true;

    private bool visible = true;
    public bool Visibile { 
        get{
            return visible;
        }

        set{
            visible = value;
            VisibilityChanged.Invoke(value);
        }
    }

    public UnityEvent<bool> VisibilityChanged;

    private bool sfx = false;

    void Start()
    {
        startPosition = transform.localPosition;
        rb = GetComponent<Rigidbody2D>();
        currentBallSpeed = speed;

        sfx = PlayerPrefs.GetInt("PongSFX", 0) == 1;

        if(!loadDifficultyPrefs){
            return;
        }

        int difficultyLevel = PlayerPrefs.GetInt("Module3TableTennisAceDifficulty", 2);
        foreach(BallDifficulty difficulty in ballDifficulties){
            if(difficulty.difficultyLevel != difficultyLevel){
                continue;
            }

            speed = difficulty.speed;
            maxSpeed = difficulty.maxSpeed;
            speedIncrease = difficulty.speedIncrease;
        }
    }

    public void SetGoingRight(bool goingRight)
    {
        this.goingRight = goingRight;
    }

    public void SetSpeed(bool angle)
    {
        float currentSpeed = currentBallSpeed * (goingRight ? 1f : -1f);

        float launchAngle = angle ? Random.Range(-maxBallAngle, maxBallAngle) : 0;

        currentVelocity = new Vector2(Mathf.Cos(launchAngle * Mathf.Deg2Rad), Mathf.Sin(launchAngle * Mathf.Deg2Rad)) * currentSpeed;
    }

    public void SetSpeed(Vector3 collisionPosition)
    {
        float currentSpeed = currentBallSpeed * (goingRight ? 1f : -1f);

        float launchAngle = Mathf.Clamp(((transform.localPosition.y - collisionPosition.y)/maxCollisionDistanceWithPaddle) * -maxBallAngle, -maxBallAngle, maxBallAngle);

        if(transform.position.x < 0){
            launchAngle *= -1;
        }

        currentVelocity = new Vector2(Mathf.Cos(launchAngle * Mathf.Deg2Rad), Mathf.Sin(launchAngle * Mathf.Deg2Rad)) * currentSpeed;
    }

    void FixedUpdate(){
        if(transform.localPosition.x < -maxPositionX || transform.localPosition.x > maxPositionX){
            rb.velocity = Vector2.zero;
            currentVelocity = Vector2.zero;

            currentBallSpeed = speed;

            if(transform.localPosition.x > 0){
                goalScoredRight.Invoke();
                goingRight = true;
            }else{
                goalScoredLeft.Invoke();
                goingRight = false;
            }
            
            transform.localPosition = startPosition;
            return;
        }

        rb.velocity = currentVelocity;
    }

    void OnCollisionEnter2D(Collision2D col){
        if(col.transform.tag != "Paddle"){
            currentVelocity.y *= -1;
            return;
        }

        collided.Invoke();

        if((transform.localPosition.x < 0 && col.transform.localPosition.x > transform.localPosition.x) || (transform.localPosition.x > 0 && col.transform.localPosition.x < transform.localPosition.x)){
            return;
        }

        currentVelocity.x *= -1;
        goingRight = transform.localPosition.x < 0;

        currentBallSpeed += speedIncrease;

        if(sfx)
            source.PlayOneShot(bounce);

        if(transform.localPosition.x < 0 && opponentInGame){
            SetSpeed(true);
            return;
        }

        SetSpeed(col.transform.localPosition);
    }

    void OnCollisionStay(Collision col){
        Vector2 temp = (new Vector2(transform.position.x, transform.position.y) - new Vector2(col.contacts[0].point.x, col.contacts[0].point.y)) * 0.1f;
        Vector3 newPos = new Vector3(transform.position.x - temp.x, transform.position.y - temp.y, transform.position.z);
        transform.position = newPos;
    }
}
