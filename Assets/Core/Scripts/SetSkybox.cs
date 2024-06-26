using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSkybox : MonoBehaviour
{
    public Material material;

    void Start()
    {
        Skybox skybox = FindObjectOfType<Skybox>();
        skybox.material = material;
    }
}
