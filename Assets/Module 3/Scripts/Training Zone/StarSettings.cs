using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSettings : MonoBehaviour
{
    public StarSpawner starTask;
    public float minSpeed = 0.1f;
    public float speedIntervals = 0.1f;
    
    public int minStars = 5;
    public int starIntervals = 5;

    void Awake()
    {
        starTask.starSpeed = minSpeed + (PlayerPrefs.GetInt("Module3StarsSpeed") * speedIntervals);
        starTask.numberOfStars = minStars + (PlayerPrefs.GetInt("Module3StarsQuantity") * starIntervals);
        starTask.highlighterEnabled = PlayerPrefs.GetInt("Module3StarsToggle") == 1;
    }
}
