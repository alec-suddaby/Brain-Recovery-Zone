using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntervalBillboard : MonoBehaviour
{
    public int interval = 20;
    private Transform target;
    public Transform Target{
        set{
            target = value;
            SpawnNewHighlighter();
        }
    }
    public GameObject highlighter;
    private StarHighlighter starHighlighter;

    void Update()
    {
        transform.LookAt(target);
        Vector3 rot = transform.localEulerAngles;
        rot.x = (int)((rot.x + ((float)interval/2f))/(float)interval) * interval;
        rot.y = (int)((rot.y + ((float)interval/2f))/(float)interval) * interval;
        rot.z = (int)((rot.z + ((float)interval/2f))/(float)interval) * interval;
        transform.localEulerAngles = rot;

        if(starHighlighter != null && starHighlighter.transform.rotation != transform.rotation){
            SpawnNewHighlighter();
        }
    }

    void SpawnNewHighlighter(){
        EndCurrent();

        if(!gameObject.activeSelf){
            return;
        }

        starHighlighter = Instantiate(highlighter, transform.position, transform.rotation).GetComponent<StarHighlighter>();
    }

    public void EndCurrent(){
        if(starHighlighter != null){
            starHighlighter.StartFadeOut();
        }
    }
}
