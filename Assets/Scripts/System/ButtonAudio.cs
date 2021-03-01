using System.Collections;
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
        
    }


    public void OnSceneLoaded()
    {
        CollectButtonArray();
    }

    public void ClearButtonArray()
    {
        btn = null;
        buttons.Clear();
    }

    void CollectButtonArray()
    {
        Debug.Log("Collect Botton Array");

        ClearButtonArray();

        //Set btn to the array drawn up in the ButtonArray
        //btn = GameObject.FindGameObjectWithTag("ButtonArray").GetComponent<buttonArrayScript>().buttonArray;

        // Make an array of all the GameObjects with the tag 'button'
        btn = GameObject.FindGameObjectsWithTag("button");
        
        if (btn.Length != 0)
            AddButtonsToList();
        else
            Debug.Log("Can't find any Buttons to attach audio to");
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

        source.volume = audioVolume;
        
        for (int i = 0; i < buttons.Count; i++)
        {  
            buttons[i].onClick.AddListener( () => PlaySound());
            //return;
        }

        Debug.Log("Button Array Length: " + btn.Length);
    }

    void PlaySound()
    {
        source.PlayOneShot(clickSound);
        
        /*
        if (!source.isPlaying)
        {
            source.PlayOneShot(clickSound);
        }
        else
        {
            return;
        }
        */
    }
}
