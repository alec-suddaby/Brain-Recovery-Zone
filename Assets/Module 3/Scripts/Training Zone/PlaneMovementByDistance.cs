using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlaneMovementByDistance : MonoBehaviour
{
    public bool isTarget = false;

    public float speed = 0.01f;
    
    public float minTurningDistance = 30f;
    public float maxDistance = 40f;
    public float minTurningSpeed = 0.2f;
    public float maxTurningSpeed = 0.5f;
    private float turningSpeed = 0.5f;

    private bool rotating = false;
    private bool invertTurn = false;
    private float maxAngleToOrigin = 45f;

    public float finalDistance = 50f;

    public float timeLimit = 20f;
    private bool finished = false;
    private bool turned = false;
    private float turnStartTimeLimit = 0f;
    public float tiltAmount = 35f;
    public float turnSmoothness = 0.1f;
    private bool ending = false;
    public float fadeSpeed = 0.5f;
    public AudioSource audioSource;
    public Transform origin;

    private bool playerHasPressedTrigger = false;
    public ParticleSystem correctClickFeedback;

    public UnityEvent fadingOut = new();

    void Start(){
        ChangeParameters();
        StartCoroutine("FadeIn");
    }

    public void TriggerPressFeedback(){
        if(!finished || playerHasPressedTrigger){
            return;
        }

        playerHasPressedTrigger = true;

        if(correctClickFeedback != null){
            ParticleSystem correctClick = Instantiate(correctClickFeedback.gameObject, transform.position, transform.rotation).GetComponent<ParticleSystem>();
            correctClick.Play();
            Destroy(correctClick, 5f);
        }
    }

    void FixedUpdate()
    {
        if(finished){
            return;
        }
        transform.position += (transform.forward * speed);
        timeLimit -= Time.fixedDeltaTime;
        
        float originDistance = Vector3.Distance(origin.position, new Vector3(transform.position.x, 0 , transform.position.z));

        float rotationSpeed = (originDistance - minTurningDistance)/(maxDistance - minTurningDistance);
        rotationSpeed = rotationSpeed < 0 ? 0 : rotationSpeed;
        rotationSpeed *= maxTurningSpeed;
        rotationSpeed *= invertTurn ? -1f : 1f;
        
        float visualRoll = -rotationSpeed * tiltAmount;

        

        if(ending){
            visualRoll = 0f;
        }

        float signedZAngle = transform.eulerAngles.z > 180f ? -Mathf.Abs(360 - transform.eulerAngles.z) : transform.eulerAngles.z;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, MoveTowards(signedZAngle, visualRoll, turnSmoothness));

        if(!finished && Vector3.Distance(origin.position, new Vector3(transform.position.x, 0 , transform.position.z)) > finalDistance){
            finished = true;
            StartCoroutine("FadeOut");
        }

        float timeToEdge = DistanceToEdge()/(speed/Time.fixedDeltaTime);
        if(timeToEdge > timeLimit && turned){
            ending = true;
            return;
        }

        // Debug.Log("Distance to edge: " + DistanceToEdge() + "m");
        // Debug.Log("Speed: " + (speed/Time.fixedDeltaTime) + "m/s");
        // Debug.Log("Time to reach edge: " + DistanceToEdge()/(speed/Time.fixedDeltaTime) + "s");

        
        
        float angleToOrigin = Vector2.SignedAngle(new Vector2(transform.forward.x, transform.forward.z), new Vector2(origin.position.x, origin.position.z) - new Vector2(transform.position.x, transform.position.z));
        if(originDistance > minTurningDistance && Mathf.Abs(angleToOrigin) > maxAngleToOrigin){
            transform.Rotate(0,rotationSpeed, 0, Space.World);
            
            rotating = true;
        }else if(rotating){
            ChangeParameters();
        }
    }

    IEnumerator FadeIn(){
        float scale = 0f;
        while(scale < 1f){
            scale += fadeSpeed * Time.fixedDeltaTime;
            scale = Mathf.Clamp01(scale);
            transform.localScale = Vector3.one * scale;
            audioSource.volume = scale;
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator FadeOut(){
        fadingOut.Invoke();

        float scale = 1f;
        while(scale > 0f){
            scale -= fadeSpeed * Time.fixedDeltaTime;
            scale = Mathf.Clamp01(scale);
            transform.localScale = Vector3.one * scale;
            audioSource.volume = scale;
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(3.5f);
        Destroy(gameObject);
    }

    float MoveTowards(float from, float to, float speed){
        float result = from;
        if(to < from){
            speed *= -1;
        }

        result += speed;

        if(to < from){
            result = Mathf.Clamp(result, to, from);
            return result;
        }

        result = Mathf.Clamp(result, from, to);
        return result;
    }

    void ChangeParameters(){
        rotating = false;
        turned = true;
        //invertTurn = Random.Range(0f, 1f) >= 0.75f ? !invertTurn : invertTurn;
        if(Random.Range(0f, 1f) >= 0.75f){
            invertTurn = !invertTurn;
        }
        turningSpeed = Random.Range(minTurningSpeed, maxTurningSpeed);
        maxAngleToOrigin = Random.Range(10f, 80f);
        turnStartTimeLimit = timeLimit;
    }

    float DistanceToEdge(){
        float angleToOrigin = Vector2.SignedAngle(new Vector2(transform.forward.x, transform.forward.z), new Vector2(origin.position.x, origin.position.z)-new Vector2(transform.position.x, transform.position.z));
        float distanceToOrigin = Vector3.Distance(origin.position, new Vector3(transform.position.x, 0 , transform.position.z));

        float omega = Mathf.Rad2Deg * Mathf.Asin((distanceToOrigin * Mathf.Sin(angleToOrigin * Mathf.Deg2Rad))/finalDistance);
        float alpha = 180 - angleToOrigin - omega;

        return Mathf.Abs((finalDistance * Mathf.Sin(alpha * Mathf.Deg2Rad))/Mathf.Sin(angleToOrigin * Mathf.Deg2Rad));
    }
}
