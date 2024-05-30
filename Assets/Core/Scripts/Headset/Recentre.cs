using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Recentre : MonoBehaviour
{
    public void RecentrePeripherals(){
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevices( devices );

        foreach(var device in devices){
            device.subsystem.TryRecenter();
        }

    }
}
