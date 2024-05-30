using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinkers : MonoBehaviour
{
    public float fadeStartRotation = 5f;
    public float maxRotation = 7f;
    public MeshRenderer meshRenderer;
    public Transform rotationTarget;

    void Update(){
        float currentRotation = Mathf.Sqrt( Mathf.Pow(rotationTarget.rotation.x, 2f) + Mathf.Pow(rotationTarget.rotation.y, 2f) );
        currentRotation *= Mathf.Rad2Deg;
        currentRotation *= 2f;

        Color meshColour = meshRenderer.material.color;

        meshColour.a = Mathf.Clamp( currentRotation - fadeStartRotation , 0f, float.MaxValue);
        meshColour.a = meshColour.a/(maxRotation - fadeStartRotation);

        meshRenderer.material.color = meshColour;
    }
}
