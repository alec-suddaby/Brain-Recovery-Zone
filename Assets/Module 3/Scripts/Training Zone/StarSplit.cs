using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSplit : MonoBehaviour
{
    public bool hasSplit = false;
    public LayerMask raycastMask;
    public ShootingStar star;
    public Transform starDisplayObject;
    public GameObject dummyStar;
    public int numberOfDummyStars = 1;
    public float spawnDelay = 0.5f;
    void FixedUpdate(){
        if(hasSplit){
            return;
        }

        if(Physics.Raycast(transform.position, starDisplayObject.position - transform.position, Vector3.Distance(transform.position, starDisplayObject.position), raycastMask)){
            StartCoroutine("SpawnDummies");
            hasSplit = true;
        }
    }

    IEnumerator SpawnDummies(){
        yield return new WaitForSeconds(spawnDelay);

        for(int i = 0; i < numberOfDummyStars; i++){
            SpawnDummy();
        }
    }

    void SpawnDummy(){
        DummyStar newDummyStar = Instantiate(dummyStar, star.transform.position, star.transform.rotation).GetComponent<DummyStar>();
        newDummyStar.rotationSpeed = star.rotationSpeed;
        newDummyStar.MovementDirection = Quaternion.Euler(0,0,Random.Range(-45f, 45f)) * star.RotationAmount;
        newDummyStar.duration = star.duration;
    }
}
