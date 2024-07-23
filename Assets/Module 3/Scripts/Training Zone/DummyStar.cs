using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyStar : MonoBehaviour
{
    public float duration;
    public float rotationSpeed = 10f;
    public ParticleSystem particles;

    private bool complete = false;

    public Vector3 MovementDirection{
        set{
            movementDirection = value.normalized * rotationSpeed;
        }
    }
    private Vector3 movementDirection;

    void FixedUpdate(){
        if(complete){
            return;
        }
        if(duration < 0){
            Destroy(gameObject, 6);
            complete = true;
            if(particles != null)
                particles.Stop();
            return;
        }

        duration -= Time.fixedDeltaTime;

        transform.eulerAngles += (movementDirection * Time.fixedDeltaTime);
    }
}
