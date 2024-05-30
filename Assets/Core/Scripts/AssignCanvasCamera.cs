using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class AssignCanvasCamera : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }
}
