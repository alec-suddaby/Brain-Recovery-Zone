﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonAudio : MonoBehaviour
{
    public List<Button> buttons;
    public GameObject[] btn;
    public AudioClip clickSound;
    private GameObject buttonArrayObject;

    [Range(0.0f,1.0f)]
    public float audioVolume = 0.5f;
    private AudioSource source { get { return GetComponent<AudioSource>(); } }
    
    void Start()
    {
        gameObject.AddComponent<AudioSource>();
        source.clip = clickSound;
        source.playOnAwake = false;
        
        CollectButtonArray();
    }

    void Update()
    {
        source.volume = audioVolume;
        
        for (int i = 0; i < buttons.Count; i++)
        {  
            buttons[i].onClick.AddListener( () => PlaySound());
            //return;
        }
    }

    public void OnSceneLoaded()
    {
        //Debug.Log("#####SCENE CHANGE");
        CollectButtonArray();
    }

    public void ClearButtonArray()
    {
        btn = null;
        buttons.Clear();
    }

    void CollectButtonArray()
    {
        ClearButtonArray();

        btn = GameObject.FindGameObjectWithTag("ButtonArray").GetComponent<buttonArrayScript>().buttonArray;

        /*
            // This is to search for gameobjects with 'button' tag. Depricated now
            // Make an array of all the GameObjects with the tag 'button'
            //btn = GameObject.FindGameObjectsWithTag("button");
        */

        AddButtonsToList();
    }

    void AddButtonsToList()
    {
        // Iterate through the array of 'btn' and add them to the 'buttons' list
         
        for (int i = 0; i < btn.Length; i++)
        {
            // Adding the current 'btn' to the 'buttons' list
            // Get the Button component from each of the gameobjects
            buttons.Add(btn[i].GetComponent<Button>());
        }

        Debug.Log("Button Array Length: " + btn.Length);
    }

    void PlaySound()
    {
        if (!source.isPlaying)
        {
            source.PlayOneShot(clickSound);
        }
        else
        {
            return;
        }
    }
}
