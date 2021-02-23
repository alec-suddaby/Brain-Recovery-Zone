﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonAudio : MonoBehaviour
{
    

    //add a listener for every time a scene is loaded async
    //then gather all the buttons and attach listeners
    // then trigger the play sound


    
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

        //Set btn to the array drawn up in the ButtonArray
        //btn = GameObject.FindGameObjectWithTag("ButtonArray").GetComponent<buttonArrayScript>().buttonArray;

        // Make an array of all the GameObjects with the tag 'button'
        btn = GameObject.FindGameObjectsWithTag("button");
        

        AddButtonsToList();
    }

    void AddButtonsToList()
    {
        // Iterate through the array of 'btn' and add them to the 'buttons' list
         
        for (int i = 0; i < btn.Length; i++)
        {
            if(btn[i] == null)
            {
                continue;
            }
            
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
