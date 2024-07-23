using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarSignButton : MonoBehaviour
{
    public Icon.IconType starSign;
    public Texture icon;
    public ZoneIn zoneInTask;
    public Vector2 minPos = new Vector2(-500, -500);
    public Vector2 maxPos = new Vector2(500, 500);

    public void Clicked(){
        Debug.Log(starSign);
        zoneInTask.CheckCorrect(starSign);
    }
}
