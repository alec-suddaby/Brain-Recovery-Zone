using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AudioVolumePanelHoverShow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
{
    public GameObject audioVolumePanelChild;

    //Clickable Down Objects
	private Component[] audioPointerDetectionArray;
	private bool audioPointerDownSwitch = false;
    
    void Start()
    {
		// Get all gameobjects with the pointer down detection on
		audioPointerDetectionArray = GetComponentsInChildren<PointerDownDetection>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {  
        audioVolumePanelChild.SetActive(true);
    }

	public void OnPointerExit(PointerEventData eventData)
    {		
		// Setting bool for in clicking down
		foreach(PointerDownDetection audioPointerDetection in audioPointerDetectionArray)
		{
			if(audioPointerDetection.down == true)
			{
				return;
				// by returning here, if there is one that is down == true then it won't progress to the following actions
			}
		}
        
        audioVolumePanelChild.SetActive(false);
	} 
}
