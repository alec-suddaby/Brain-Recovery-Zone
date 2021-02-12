using UnityEngine;
using UnityEngine.XR;

public class XRRenderScale : MonoBehaviour 
{
    public float vrScale = 1.5f;

    void Start () {
        XRSettings.eyeTextureResolutionScale = vrScale;
    }
}