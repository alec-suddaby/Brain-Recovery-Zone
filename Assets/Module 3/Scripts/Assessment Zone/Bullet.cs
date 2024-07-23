using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 previousPosition;
    private bool destroy = false;
    public LayerMask targetLayer;

    void Awake(){
        previousPosition = transform.position;
    }

    void Update(){
        RaycastHit hit;
        if(Physics.Raycast(previousPosition, transform.position - previousPosition, out hit, Vector3.Distance(previousPosition, transform.position), targetLayer)){
            Target target = hit.transform.GetComponent<Target>();
            target.Hit();            

            transform.position = hit.point;
            destroy = true;
        }

        previousPosition = transform.position;
    }

    void LateUpdate(){
        if(!destroy){
            return;
        }
        Destroy(gameObject);
    }
}
