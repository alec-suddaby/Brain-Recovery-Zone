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
    private string currentTime;

    //XR Toolkit controllers
    private XRNode xRNode = XRNode.RightHand;
    private List<InputDevice> devices = new List<InputDevice>();
    private InputDevice device;
    private HandSelection masterHandSelection;

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

    [Header("Attention Training Level")]
    public bool mildLevel = false;
    public bool moderateLevel = false;
    public bool severeLevel = false;
    private bool dataSaved = false;


    // Check Audio Toggle
    private bool isMute = false;
    private string muteIndication;
	

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
		returningToMenu = false;
        
        // Get all gameobjects with the pointer down detection on
		pointerDetectionArray = GetComponentsInChildren<PointerDownDetection>();
		//Debug.Log("Number in down detection array " + pointerDetectionArray.Length);
		
		skyboxMediaPlayer.Events.AddListener(OnVideoEvent);

        //Get Hand Selection
        masterHandSelection = FindObjectOfType<HandSelection>();
        xRNode = masterHandSelection.masterXRNode;

        CheckAudioToggle();
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
                currentTime = minutes.ToString("D2") + ":" + seconds.ToString("D2") + ":" + milliseconds.ToString("D2");
                //string videoDuration = (string.Format("{0} / {1}", currentTime, durationOfVideo));
 
                //this is for your text ui to display current time
				timeCount.text = "Video Current: " + currentTime;
                timeDuration.text = "Video Duration: " + durationOfVideo;
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
			if(PlayerPrefs.GetInt("PlayAttentionTrainingMute") == 1){OnMute();}
            else{OnUnmute();}
			break;
			case MediaPlayerEvent.EventType.FinishedPlaying:
			BackToAttentionTrainingMenu();
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
        // Set loading to true
        returningToMenu = true;
        // Pause the game
        OnPauseButton();

        // Check where to save data to
        if (mildLevel && !dataSaved){mildSaveData();}
        else if (moderateLevel && !dataSaved){moderateSaveData();}
        else if (severeLevel && !dataSaved){severeSaveData();}
        else{Debug.Log("Error saving Attention Training time");}
        // Load back to the Attention Training Menu
        SceneLoader.Instance.LoadNewScene("02_Practice_02_AttentionTraining");
    }


	public void OnPauseButton()
	{
		skyboxMediaPlayer.Control.Pause();
	}

    private void mildSaveData()
    {        
        //Save time string to player prefs
        PlayerPrefs.SetString("MildLastResult", currentTime + muteIndication);

        // Get the current count value
        int mildSavedListCount = PlayerPrefs.GetInt("MildCount"); 
        // Save the latest result string to player prefs
        PlayerPrefs.SetString("MildResults" + mildSavedListCount, currentTime + muteIndication);
        // Add one more number to the count
        mildSavedListCount = mildSavedListCount + 1;
        // Save the new count back to player prefs
        PlayerPrefs.SetInt("MildCount", mildSavedListCount);
        dataSaved = true;
    }
    private void moderateSaveData()
    {
        //Save time string to player prefs
        PlayerPrefs.SetString("ModerateLastResult", currentTime + muteIndication);

        // Get the current count value
        int moderateSavedListCount = PlayerPrefs.GetInt("ModerateCount"); 
        // Save the latest result string to player prefs
        PlayerPrefs.SetString("ModerateResults" + moderateSavedListCount, currentTime + muteIndication);
        // Add one more number to the count
        moderateSavedListCount = moderateSavedListCount + 1;
        // Save the new count back to player prefs
        PlayerPrefs.SetInt("ModerateCount", moderateSavedListCount);
        dataSaved = true;
    }
    private void severeSaveData()
    {
        //Save time string to player prefs
        PlayerPrefs.SetString("SevereLastResult", currentTime + muteIndication);

        // Get the current count value
        int severeSavedListCount = PlayerPrefs.GetInt("SevereCount"); 
        // Save the latest result string to player prefs
        PlayerPrefs.SetString("SevereResults" + severeSavedListCount, currentTime + muteIndication);
        // Add one more number to the count
        severeSavedListCount = severeSavedListCount + 1;
        // Save the new count back to player prefs
        PlayerPrefs.SetInt("SevereCount", severeSavedListCount);
        dataSaved = true;
    }

    private void OnMute()
	{
		skyboxMediaPlayer.Control.MuteAudio(true);
	}

	private void OnUnmute()
	{
		skyboxMediaPlayer.Control.MuteAudio(false);
	}

    private void CheckAudioToggle()
    {
        if(PlayerPrefs.GetInt("PlayAttentionTrainingMute") == 1)
        {
            isMute = true;
            muteIndication = "<sprite index=[1]>";
        }
        else if (PlayerPrefs.GetInt("PlayAttentionTrainingMute") == 0)
        {
            isMute = false;
            muteIndication = "<sprite index=[0]>";
        }
        else
        {
            isMute = true;
            muteIndication = " [Err]";
            Debug.Log("Error deteching the Attention Training audio toggle state");
        }
    }
    
}

