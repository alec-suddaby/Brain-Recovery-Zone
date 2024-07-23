using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bullet;
    public float bulletSpeed = 2f;
    public AudioSource audioSource;
    public AudioClip gunshot;

    public void Shoot(){
        audioSource.PlayOneShot(gunshot);

        GameObject b = (GameObject)Instantiate(bullet, transform.position, transform.rotation);
        Rigidbody rb = b.GetComponent<Rigidbody>();
        rb.velocity = b.transform.forward * bulletSpeed;

        Destroy(b, 5f);
    }
}
