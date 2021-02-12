using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollContainerButtonWrapGeneration : MonoBehaviour
{   
    [SerializeField]
    private GameObject buttonWarp;

    [SerializeField]
    private float yPosition = 250;

    [SerializeField]
    public GameObject scrollBar;

    private GameObject[] buttonArray;

    // Start is called before the first frame update
    void Start()
    {
        transform.parent.gameObject.GetComponent<Mask>().enabled = true;

        // Gets all the gameObject that are a child of buttonWrap and adds to the buttonArray.
        buttonArray = new GameObject[buttonWarp.transform.childCount];
        for (int i = 0; i < buttonWarp.transform.childCount; i++)
        {
            buttonArray[i] = buttonWarp.transform.GetChild(i).gameObject;
        }

        // Enabling and Disabling the Scroll Rect based on number of buttons
        if (buttonArray.Length >= 4)
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
        if (buttonArray.Length == 0)
        {
            return;
        }
        else if (buttonArray.Length == 1)
        {
            // Get current gameObject RectTransform
            RectTransform currentGORT = gameObject.GetComponent<RectTransform>();
            // Get buttonArray[0] Object's RectTransform
            RectTransform buttonArray0RT = buttonArray[0].GetComponent<RectTransform>();
            // Calculate half of the Current gameObject's width
            float currentGOHalfWidth = currentGORT.sizeDelta.x / 2;
            // Set the only buttonArray object's x value to 1/2 of the current gameObject's width (aka, center)
            buttonArray0RT.localPosition = new Vector3 ( currentGOHalfWidth , yPosition, 0);
        }
        else if (buttonArray.Length == 2)
        {
            // Get current gameObject RectTransform
            RectTransform currentGORT = gameObject.GetComponent<RectTransform>();
            // Get buttonArray[0] Object's RectTransform
            RectTransform buttonArray0RT = buttonArray[0].GetComponent<RectTransform>();
            // Get buttonArray[1] Object's RectTransform
            RectTransform buttonArray1RT = buttonArray[1].GetComponent<RectTransform>();
            // Calculate half of the Current gameObject's width
            float currentGOHalfWidth = currentGORT.sizeDelta.x / 2;
            // Set the buttonArray[0] object's x value to the left of center
            buttonArray0RT.localPosition = new Vector3 ( currentGOHalfWidth - (buttonArray0RT.sizeDelta.x / 2) - 25 , yPosition, 0);
            // Set the buttonArray[1] object's x value to the left of center
            buttonArray1RT.localPosition = new Vector3 ( currentGOHalfWidth + (buttonArray1RT.sizeDelta.x / 2) + 25 , yPosition, 0);
        }
        else {

            // Setting the x Position for each button correctly
            for (int i = 0; i < buttonArray.Length; i++)
            {  
                RectTransform rt = buttonArray[i].GetComponent<RectTransform>();
                
                // Add count for 1 and 2 items and then have them align center

                if (buttonArray[i] == null)
                {
                    continue;
                }
                else if (buttonArray.Length == 0)
                { 
                    return;
                }
                // If i == 0 then set the first position at ( x = (width[i] / 2) + 145 )
                else if(i==0){
                    rt.localPosition = new Vector3( (rt.sizeDelta.x / 2) + 145 , yPosition , 0 );
                }            
                // For all after 0  ( x = [i-1]x + (width[i-1] / 2) + 50 + (width[i] / 2)  )
                else{
                    RectTransform rtm1 = buttonArray[i-1].GetComponent<RectTransform>();
                    rt.localPosition = new Vector3( rtm1.localPosition.x + (rtm1.sizeDelta.x / 2) + 50 + (rt.sizeDelta.x / 2) , yPosition , 0 );
                }  
            }

            // Get the Rect Transform of the buttonWrap
            RectTransform bwrt = buttonWarp.GetComponent<RectTransform>();
            // Then get the final button in the array
            GameObject lastButtonArray = buttonArray[buttonArray.Length - 1];
            // Then get the Rect Transform on that final button
            RectTransform lastButtonArrayRT = lastButtonArray.GetComponent<RectTransform>();
            // Calculate the width of the Button Wrap based off the last button's x position, width and + padding of 145
            bwrt.sizeDelta = new Vector2 ( lastButtonArrayRT.localPosition.x + (lastButtonArrayRT.sizeDelta.x / 2) + 145 , bwrt.sizeDelta.y );
            
        }
    }
}
