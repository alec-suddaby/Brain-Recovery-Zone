using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneSettings : MonoBehaviour
{
    public PlaneScanningTask planeScanningTask;
    public float minSpeed = 0.1f;
    public float speedIntervals = 0.1f;
    
    public int minPlanes = 5;
    public int planeIntervals = 5;

    void Awake()
    {
        planeScanningTask.planeSpeed = minSpeed + (PlayerPrefs.GetInt("Module3PlanesSpeed") * speedIntervals);
        planeScanningTask.numberOfPlanes = minPlanes + (PlayerPrefs.GetInt("Module3PlanesQuantity") * planeIntervals);
        planeScanningTask.dummyPlanesEnabled = PlayerPrefs.GetInt("Module3PlanesToggle") == 1;
    }
}
