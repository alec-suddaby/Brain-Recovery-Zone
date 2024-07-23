using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public ParticleSystem hitEffect;
    public AudioSource audioSource;
    public AudioClip popSound;
    public void Hit(){
        hitEffect.Play();
        audioSource.PlayOneShot(popSound);
        gameObject.SetActive(false);
    }
}
