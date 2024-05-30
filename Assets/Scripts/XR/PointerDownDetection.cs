using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerDownDetection : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IEventSystemHandler
{
    public bool down = false;
    public bool up = true;
    
    public void OnPointerDown(PointerEventData eventData)
	{
		down = true;
        up = false;
        //Debug.Log("Down");
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		down = false;
        up = true;
        //Debug.Log("Up");
	}
}
