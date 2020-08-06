using UnityEngine;
using UnityEngine.XR;

public class VRRenderScale : MonoBehaviour {
    public float vrScale = 1.5f;

    void Start () {
        XRSettings.eyeTextureResolutionScale = vrScale;
    }
}