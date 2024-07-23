using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int maxPositionY = 800;
    public float speed = 2f;
    public Transform controller;
    void FixedUpdate()
    {
        float vertical = 0;

        #if UNITY_EDITOR
            vertical = Input.GetAxis("Vertical");
        #else
            float angle = -1f * (controller.localEulerAngles.x > 180 ? -360 + controller.localEulerAngles.x : controller.localEulerAngles.x);
            vertical = Mathf.Sin(Mathf.Deg2Rad * Mathf.Clamp(angle * 2, -90f, 90f));
        #endif
        
        Vector3 newPosition = transform.localPosition;
        newPosition.y += vertical * speed;
        newPosition.y = Mathf.Clamp(newPosition.y, -maxPositionY, maxPositionY);

        transform.localPosition = newPosition;
    }
}
