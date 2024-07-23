using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class ReplayDice : MonoBehaviour
{
    int count = 0;

    public DicePositions dicePositions;
    [HideInInspector]public Transform targetFace;

    public Transform[] faces;
    // Start is called before the first frame update
    int randomRollNo;

    public Transform die;

    public bool complete = false;
    
    public AudioClip bounceSound;
    private AudioSource audioSource; 

    private Rigidbody rb;
    private BoxCollider col;

    public float pitchVolumeDecreaseOnBounce = 0.1f;

    private int currentFace = 0;
    public int CurrentFace => currentFace;

    void Awake(){
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();
    }

    public void SetState(bool animate){
        if(!gameObject.activeSelf){
            return;
        }
        rb.isKinematic = animate;
        rb.useGravity = !animate;

        col.enabled = !animate;
    }

    public void LoadDiceRoll(string rollName, int face){
        SetState(true);
        count = 0;
        string path = rollName;
        targetFace = faces[face];
        complete = false;

        audioSource.pitch = 1f;
        audioSource.volume = 1f;

        using(StreamReader reader = new StreamReader(path)){
            string json = reader.ReadToEnd();
            Debug.Log(json);
            dicePositions = JsonUtility.FromJson<DicePositions>(json);
        }

        currentFace = face;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(complete){
            return;
        }
        if(dicePositions.roll.Count <= count){
            complete = true;
            SetState(false);
            return;
        }

        die.localPosition = dicePositions.roll[count].GetPosition;
        die.rotation = dicePositions.roll[count].GetRotation;
        die.rotation *= dicePositions.roll[dicePositions.roll.Count - 1].GetTopFaceRotationOffset;
        die.rotation *=  new Quaternion(-targetFace.localRotation.x, targetFace.localRotation.y, -targetFace.localRotation.z,targetFace.localRotation.w);
        
        if(dicePositions.roll[count].WasCollision){
            audioSource.PlayOneShot(bounceSound);
            audioSource.pitch -= pitchVolumeDecreaseOnBounce;
            audioSource.volume -= pitchVolumeDecreaseOnBounce;
        }
        count++;
    }
}
