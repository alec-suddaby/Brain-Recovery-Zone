using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCanvasCamera : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }
}
