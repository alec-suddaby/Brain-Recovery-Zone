using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerParticleSystem : MonoBehaviour
{
    public ParticleSystem particleSystem;

    public void PlayParticleSystem(){
        particleSystem.Play();
    }
}
