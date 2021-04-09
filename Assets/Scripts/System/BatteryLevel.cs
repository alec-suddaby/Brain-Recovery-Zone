using UnityEngine;
using TMPro;

public class BatteryLevel : MonoBehaviour
{
    public TextMeshProUGUI headsetBatteryText;
    //public TextMeshProUGUI controllerBatteryText;
    
    void Start()
    {
        headsetBatteryText.text = "Headset Battery: " + (SystemInfo.batteryLevel * 100) + "%";
        //controllerBatteryText.text = "Controller Battery: " + "--%";
    }
}
