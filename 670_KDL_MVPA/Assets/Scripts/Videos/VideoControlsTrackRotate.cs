using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoControlsTrackRotate : MonoBehaviour
{    
    private float cameraYRotation;

    float smooth = 1.0f;

    // Get main camera

    // Set Main camera y rorate to self y rotate

    // Update is called once per frame
    public void GetCameraRotatePosition()
    {
        cameraYRotation = Camera.main.transform.eulerAngles.y;
        Quaternion target = Quaternion.Euler(0, cameraYRotation, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth );
    }
}
