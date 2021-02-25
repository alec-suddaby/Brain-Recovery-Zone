using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollContainerButtonWrapGenerationVertical : MonoBehaviour
{   
    [SerializeField]
    private GameObject buttonWarp;

    private float xLeft = -355;
    private float xRight = 355;
    private float yStart = -20;
    private float yOffset = -285;

    [SerializeField]
    public GameObject scrollBar;

    private GameObject[] buttonArray;

    // Start is called before the first frame update
    void Start()
    {
        // Ensure the mask is enabled to hide the items
        transform.parent.gameObject.GetComponent<Mask>().enabled = true;

        // Gets all the gameObject that are a child of buttonWrap and adds to the buttonArray.
        buttonArray = new GameObject[buttonWarp.transform.childCount];
        for (int i = 0; i < buttonWarp.transform.childCount; i++)
        {
            buttonArray[i] = buttonWarp.transform.GetChild(i).gameObject;
        }

        // Enabling and Disabling the Scroll Rect based on number of buttons
        if (buttonArray.Length >= 5)
        {
            gameObject.GetComponent<ScrollRect>().enabled = true;
            scrollBar.SetActive(true);
        }
        else
        {
            gameObject.GetComponent<ScrollRect>().enabled = false;
            scrollBar.SetActive(false);
        }

        // Main if statement for positioning the buttons
        if (buttonArray.Length != 0)
        {
            // Setting the x and y Position for each button correctly
            for (int i = 0; i < buttonArray.Length; i++)
            {  
                RectTransform rt = buttonArray[i].GetComponent<RectTransform>();
            
                if (buttonArray[i] == null)
                {
                    continue;
                }
                else if (buttonArray.Length == 0)
                { 
                    return;
                }
                // If i == 0 then set the first position (left)
                else if(i==0){
                    rt.localPosition = new Vector3( xLeft, yStart, 0 );
                }
                // If i == 1 then set the first position (right)
                else if(i==1){
                    rt.localPosition = new Vector3( xRight, yStart, 0 );
                }          
                // For all even after 0 (left hand side)
                else if(i % 2 == 0){
                    RectTransform rtm2left = buttonArray[i-2].GetComponent<RectTransform>();
                    rt.localPosition = new Vector3( xLeft, rtm2left.localPosition.y + yOffset, 0 );
                }
                // For all odd after 0 (right hand side)
                else if(i % 2 != 0){
                    RectTransform rtm2right = buttonArray[i-2].GetComponent<RectTransform>();
                    rt.localPosition = new Vector3( xRight, rtm2right.localPosition.y + yOffset, 0 );
                }  
                  
            }

            // Setting the amount of scroll room there is

            // Get the Rect Transform of the buttonWrap
            RectTransform bwrt = buttonWarp.GetComponent<RectTransform>();
            // Then get the final button in the array
            GameObject lastButtonArray = buttonArray[buttonArray.Length - 1];
            // Then get the Rect Transform on that final button
            RectTransform lastButtonArrayRT = lastButtonArray.GetComponent<RectTransform>();
            // Calculate the height of the last button and adding half to the box height
            bwrt.sizeDelta = new Vector2 ( bwrt.sizeDelta.x, (-1 * lastButtonArrayRT.localPosition.y) + lastButtonArrayRT.sizeDelta.y + 95 );
        }
        
        else {
            return; 
        }
    }
}
