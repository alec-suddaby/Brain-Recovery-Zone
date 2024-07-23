using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootingStar : MonoBehaviour
{
    public float rotationSpeed = 10f;
    private Vector3 rotationAmount;
    public Vector3 RotationAmount{
        get => rotationAmount;
        
        set{
            rotationAmount = value.normalized * rotationSpeed;
        }
    }
    public Transform trailObject;
    public Button button;

    public float Speed{
        set {
            rotationSpeed = value;
        }
    }

    public static bool FinishesInBadPosition(Vector3 startRotation, float duration, Vector3 rotationDirection){
        Vector3 finishRotation = FinishRotation(startRotation, duration, rotationDirection);
        
        // for(int i = 0; i < aviodPositions.Length; i++){
        //     Vector3 angularDifference = new Vector3(Mathf.Abs(transform.eulerAngles.x - aviodPositions[i].x), Mathf.Abs(transform.eulerAngles.y - aviodPositions[i].y), Mathf.Abs(transform.eulerAngles.z - aviodPositions[i].z));
        //     if(angularDifference.magnitude < minAngleFromObstacle){
        //         return true;
        //     }
        // }

        RaycastHit hit;
        if(Physics.Raycast(Vector3.zero, Quaternion.Euler(finishRotation) * Vector3.forward, out hit)){
            // Debug.Log(hit.transform.name);
            return true;
        }

        return false;
    }

    private static Vector3 FinishRotation(Vector3 rotation, float duration, Vector3 rotationAmount){

        for(float i = 0f; i <= duration; i += Time.fixedDeltaTime){
            rotation += rotationAmount * Time.fixedDeltaTime;
        }

        return rotation;
    }

    public float duration = 20f;
    private bool finished;
    public ParticleSystem starSystem;
    public StarSpawner spawner;

    void Awake(){
        button.gameObject.SetActive(false);
    }
    void FixedUpdate()
    {
        if(duration <= 0){
            if(!finished){
                finished = true;
                starSystem.Play();
                button.gameObject.SetActive(true);
                Destroy(gameObject, 6);
            }
            return;
        }
        duration -= Time.fixedDeltaTime;
        transform.eulerAngles += (rotationAmount * Time.fixedDeltaTime);
    }

    public void Clicked(){
        spawner.score++;
        button.interactable = false;
    }
}
