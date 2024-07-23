using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMovement : MonoBehaviour
{
    public List<PlaneSpawner.PlaneMove> moves;
    public float speed = 0f;

    float timeElapsed = 0f;
    float nextStartTime = 0f;
    public float finishDistance = 0f;
    public float zRotationSpeed = 2f;

    void FixedUpdate()
    {
        transform.position += (transform.forward * speed);

        Vector3 rotation = transform.localEulerAngles;
        rotation.z = moves.Count > 0 ? -(moves[0].rotation/360f) * 40f : 0f;
        transform.localEulerAngles = rotation;        

        if(moves.Count == 0){
            if(Vector3.Distance(Vector3.zero, transform.position) >= finishDistance){
                Destroy(gameObject);
            }
            return;
        }

        nextStartTime -= Time.fixedDeltaTime;
        
        if(nextStartTime <= 0f){
            moves.RemoveAt(0);
            if(moves.Count > 0){
                nextStartTime = moves[0].Duration;
                // Debug.Log("Incorrect by: " + Vector2.SignedAngle(new Vector2(transform.forward.x, transform.forward.z), -new Vector2(transform.position.x, transform.position.z)).ToString() + " Duration: " + nextStartTime + " Correction Angle: " + ( moves[0].rotation ));
                // Debug.Log("Actual Duration " + moves.Count.ToString() + ": " + moves[0].duration);
            }
            //transform.Rotate(new Vector3(0f, Vector2.SignedAngle(new Vector2(transform.forward.x, transform.forward.z), -new Vector2(transform.position.x, transform.position.z)), 0f), Space.World);
            // Vector3 lookAt = Vector3.zero;
            // lookAt.y = transform.position.y;
            // transform.LookAt(lookAt);           
        }

        
        transform.Rotate(new Vector3(0f, moves[0].rotation/moves[0].Duration, 0f) * Time.fixedDeltaTime, Space.World);
    }
}
