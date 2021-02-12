using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCanvasMainCamera : MonoBehaviour
{
    private List<Canvas> allCanvas = new List<Canvas>();

    
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Canvas[] canvas = Resources.FindObjectsOfTypeAll<Canvas>();
        //Debug.Log(allCanvas.Count);
    }
}
