using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.XR;

public class Panel : MonoBehaviour
{
    private Canvas canvas = null;
    private MenuManager menuManager = null;

    //public GameObject scrollBar;
    public GameObject buttonWrap;
    public ScrollRect currentScrollRect;

    public float scrollRateModifier = 30f;

    [Header("XR Controller")]
    //XR Toolkit controllers
    [SerializeField]
    private XRNode xRNode = XRNode.RightHand;
    private List<InputDevice> devices = new List<InputDevice>();
    private InputDevice device;

    //to avoid repeat readings
    private bool primary2DAxisIsChosen;
    private Vector2 primary2DAxisValue = Vector2.zero;
    private Vector2 prevPrimary2DAxisValue;
    

    void GetDevice()
    {
        InputDevices.GetDevicesAtXRNode(xRNode, devices);
        device = devices.FirstOrDefault();
    }

    private void Awake()
    {
        canvas = GetComponent<Canvas>();

        // Setting the scrollrect back to 0 if there is one and canvas is active
        if (currentScrollRect && canvas)
        {
            currentScrollRect.horizontalNormalizedPosition = 0;
            currentScrollRect.verticalNormalizedPosition = 0;
        }
    }

    void Update()
    {   
        if(!device.isValid)
        {
            GetDevice();
        }

        if(currentScrollRect && canvas.isActiveAndEnabled == true) 
        {
            ScrollAxis();
            //Debug.Log("value y " + primary2DAxisValue.y);
            //Debug.Log("primary2DAxisIsChosen " + primary2DAxisIsChosen);
            //Debug.Log("scroll rect v: " + currentScrollRect.verticalNormalizedPosition);
        }
    }

    public void Setup(MenuManager menuManger)
    {
        this.menuManager = menuManager;
        Hide();
    }

    public void Show()
    {
        canvas.enabled = true;

        //int tween or co routine
        // can work for canvas groups to transition here
    }

    public void Hide()
    {
        canvas.enabled = false;
    }

    void ScrollAxis()
    {
        // Capturing primary 2D Axis changes and release
        // Oculus scroll
        InputFeatureUsage<Vector2> primary2DAxisUsage = CommonUsages.primary2DAxis;

        // Make sure the value is not zero and that it has changed
        if (primary2DAxisValue != prevPrimary2DAxisValue)
        {
            primary2DAxisIsChosen = false;
        }
       
        //Oculus
        //primary2DAxisValue != Vector2.zero
        //Pico
        //primary2DAxisValue.y != -1

        if (device.TryGetFeatureValue(primary2DAxisUsage, out primary2DAxisValue) && primary2DAxisValue.y != -1 /*&& !primary2DAxisIsChosen*/)
        {
            prevPrimary2DAxisValue = primary2DAxisValue;
            primary2DAxisIsChosen = true;  
        }
        else if (primary2DAxisValue.y == -1 /*&& primary2DAxisIsChosen*/)
        {
            prevPrimary2DAxisValue = primary2DAxisValue;
            primary2DAxisIsChosen = false;
        }

        
        // Pico hack here
        // For whatever read the scrolling would default to -1 on the y axis rather than 0, So here i am checking to see if the value is -1 from the above functions then adding 1 to the y value if it is needed.
        if(buttonWrap && currentScrollRect && primary2DAxisIsChosen == false)
        {
            Vector2 totalWidth = buttonWrap.GetComponent<RectTransform>().sizeDelta;
            Vector2 targetValue = new Vector2( (primary2DAxisValue.x * scrollRateModifier) / totalWidth.x , ((primary2DAxisValue.y + 1 ) * scrollRateModifier) / totalWidth.y ) ;
            //Vector2 targetPercentage = new Vector2 (targetValue/totalWidth; 

            currentScrollRect.horizontalNormalizedPosition = currentScrollRect.horizontalNormalizedPosition + targetValue.x;
            currentScrollRect.verticalNormalizedPosition = currentScrollRect.verticalNormalizedPosition + targetValue.y;
        } else if(buttonWrap && currentScrollRect && primary2DAxisIsChosen == true)
        {
            Vector2 totalWidth = buttonWrap.GetComponent<RectTransform>().sizeDelta;
            Vector2 targetValue = new Vector2( (primary2DAxisValue.x * scrollRateModifier) / totalWidth.x , (primary2DAxisValue.y * scrollRateModifier) / totalWidth.y ) ;
            //Vector2 targetPercentage = new Vector2 (targetValue/totalWidth; 

            currentScrollRect.horizontalNormalizedPosition = currentScrollRect.horizontalNormalizedPosition + targetValue.x;
            currentScrollRect.verticalNormalizedPosition = currentScrollRect.verticalNormalizedPosition + targetValue.y;
        }

        // Oculus
        /*
        if(buttonWrap && currentScrollRect)
        {
            Vector2 totalWidth = buttonWrap.GetComponent<RectTransform>().sizeDelta;
            Vector2 targetValue = new Vector2( (primary2DAxisValue.x * scrollRateModifier) / totalWidth.x , (primary2DAxisValue.y * scrollRateModifier) / totalWidth.y ) ;
            //Vector2 targetPercentage = new Vector2 (targetValue/totalWidth; 

            currentScrollRect.horizontalNormalizedPosition = currentScrollRect.horizontalNormalizedPosition + targetValue.x;
            currentScrollRect.verticalNormalizedPosition = currentScrollRect.verticalNormalizedPosition + targetValue.y;
        }
        */
    }
}
