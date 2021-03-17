using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;
using RenderHeads.Media.AVProVideo;
using UnityEngine.EventSystems;
using TMPro;

public class AttentionTrainingVideoControls : MonoBehaviour, IEventSystemHandler
{
 


    [Header("Media Player")]
    public MediaPlayer skyboxMediaPlayer;
	public TextMeshProUGUI timeCount;
	public TextMeshProUGUI timeDuration;

    //XR Toolkit controllers
    [SerializeField]
    private XRNode xRNode = XRNode.RightHand;
    private List<InputDevice> devices = new List<InputDevice>();
    private InputDevice device;

    //to avoid repeat readings
    private bool triggerButtonIsPressed;
    private bool secondaryButtonIsPressed;

	//menu panel history bool trigger
	public bool returningToMenu = false;

	//bool to show if the controller is inside or outside of the panel
	private bool pointerOverControls = false;
	private bool pointerDown = false;

	//Clickable Down Objects
	private Component[] pointerDetectionArray;
	private bool pointerDownSwitch = false;


    // Attention Training PB Count Links
    private AttentionTrainingPBCount attentionTrainingPBCount;

	


    void GetDevice()
    {
        InputDevices.GetDevicesAtXRNode(xRNode, devices);
        device = devices.FirstOrDefault();
    }

    void OnEnable()
    {
        if(!device.isValid)
        {
            GetDevice();
        }    
    }

    void Start()
    {
		// Get all gameobjects with the pointer down detection on
		pointerDetectionArray = GetComponentsInChildren<PointerDownDetection>();
		//Debug.Log("Number in down detection array " + pointerDetectionArray.Length);
		
		skyboxMediaPlayer.Events.AddListener(OnVideoEvent);

        // Get Attention TrainingPB Count
        attentionTrainingPBCount = FindObjectOfType<AttentionTrainingPBCount>();
	
    }

    void OnDestroy()
	{
		skyboxMediaPlayer.Events.RemoveListener(OnVideoEvent);
	}

    void Update()
    {
		if(!device.isValid)
        {
            GetDevice();
        }

        // capturing secondary button press and release
        bool secondaryButtonValue = false;

        // Oculus Secondary Button back button
		InputFeatureUsage<bool> secondaryButtonUsage = CommonUsages.secondaryButton;

		// Pico Menu button as back button
		//InputFeatureUsage<bool> secondaryButtonUsage = CommonUsages.menuButton;
        
        if (device.TryGetFeatureValue(secondaryButtonUsage, out secondaryButtonValue) && secondaryButtonValue && !secondaryButtonIsPressed)
        {
            //disabled the back button here
            secondaryButtonIsPressed = true;
            BackToAttentionTrainingMenu();
        }
        else if (!secondaryButtonValue && secondaryButtonIsPressed)
        {
            secondaryButtonIsPressed = false;
        }


		// time count
		if (skyboxMediaPlayer && skyboxMediaPlayer.Info != null && skyboxMediaPlayer.Info.GetDurationMs() > 0f)
            {
                //get current video time
                float time = skyboxMediaPlayer.Control.GetCurrentTimeMs()/1000;
 
                int timeInSecondsInt = (int)time;
                int minutes = (int)time / 60;
                int seconds = timeInSecondsInt - (minutes * 60);
                int milliseconds =  ( (int)skyboxMediaPlayer.Control.GetCurrentTimeMs() - ( timeInSecondsInt * 1000) ) / 10;
           
                //get video length
                float duration = skyboxMediaPlayer.Info.GetDurationMs()/1000;  
                                           
                int durationInSecondsInt = (int)duration;
                int minutesDuration = (int)duration / 60;
                int secondsDuration = durationInSecondsInt - (minutesDuration * 60);
 
                string durationOfVideo = minutesDuration.ToString("D2") + ":" + secondsDuration.ToString("D2");
                string currentTime = minutes.ToString("D2") + ":" + seconds.ToString("D2") + ":" + milliseconds.ToString("D2");
                //string videoDuration = (string.Format("{0} / {1}", currentTime, durationOfVideo));
 
                //this is for your text ui to display current time
				timeCount.text = currentTime;
                timeDuration.text = durationOfVideo;
            }
    }

    // Callback function to handle events
	public void OnVideoEvent(MediaPlayer mp, MediaPlayerEvent.EventType et, ErrorCode errorCode)
	{
		switch (et)
		{
			case MediaPlayerEvent.EventType.Error:
			break;
			case MediaPlayerEvent.EventType.ReadyToPlay:
			//Debug.Log("Ready to play");
			break;
			case MediaPlayerEvent.EventType.Started:
			//Debug.Log("Started");
			break;
			case MediaPlayerEvent.EventType.FirstFrameReady:
			//Debug.Log("First frame ready");
			break;
			case MediaPlayerEvent.EventType.FinishedPlaying:
			BackToMenu();
			break;
		}

		// Debug function below calls each video player function as it happens
		//Debug.Log("Event: " + et.ToString());
	}

    public void LoadScene(string level)
    {
        SceneLoader.Instance.LoadNewScene(level);
    }

    public void BackToMenu()
    {
		returningToMenu = true;
		SceneLoader.Instance.ReturnToMenu();
    }

    public void BackToAttentionTrainingMenu()
    {
        OnPauseButton();
        attentionTrainingPBCount.timerSaveTest = timeCount.text.ToString();
        returningToMenu = true;
        SceneLoader.Instance.LoadNewScene("02_Practice_02_AttentionTraining");
    }

	public void OnPauseButton()
	{
		skyboxMediaPlayer.Control.Pause();
	}
    
}

