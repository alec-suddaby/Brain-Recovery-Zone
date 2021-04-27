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

public class VideoControlsManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
{
    [Header("Video Control Objects")]
    public GameObject videoControlsPanel;
    public GameObject errorPanel;
    
    [Header("Video Control Buttons")]
    public GameObject playButton;
	public GameObject pauseButton;

	public GameObject muteButton;
	public GameObject unmuteButton;
    private float previousVolume;

    public Slider videoSeekerSlider;
	private float setVideoSeekerSliderValue;
	private bool wasPlayingOnScrub;

	public Slider audioVolumeSlider;
	private float setAudioVolumeSliderValue = 0.7f;
	public GameObject audioVolumePanel;
	// Saved app volume
	//public DefaultAppVolume defaultAppVolumeComponent;

	[SerializeField]
	private Toggle loopToggle;

    [Header("Media Player")]
    public MediaPlayer skyboxMediaPlayer;
	public TextMeshProUGUI timeCount;
	public TextMeshProUGUI timeDuration;

    [Header("XR Controls")]
    public GameObject rotationOffset;

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

	//bool to alert if video controls are on
	public bool videoControlsOn = false;

	//bool to show if the controller is inside or outside of the panel
	private bool pointerOverControls = false;
	private bool pointerDown = false;

	//If the video controls should rotate with the headset
	public bool followHeadPosition = true;
	private float cameraYRotation;

	//Clickable Down Objects
	private Component[] pointerDetectionArray;
	private bool pointerDownSwitch = false;

	[Header("Standalone Function")]
	[Tooltip("Set this to true to set a custom back button location")]
	public bool setVideoStandalone = false;
	public string setBackScene;
	public StandaloneSingleMenuManager standaloneMenuManager;

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
		
		errorPanel.SetActive(false);
		
		skyboxMediaPlayer.Events.AddListener(OnVideoEvent);

		playButton.SetActive(true);
		pauseButton.SetActive(false);

		muteButton.SetActive(true);
		unmuteButton.SetActive(false);
		
		if(PlayerPrefs.GetInt("LoopVideo") == 1)
		{
			loopToggle.isOn = true;
		}
		else
		{
			loopToggle.isOn = false;
		}
		loopToggle.onValueChanged.AddListener(delegate {
			LoopToggleValueChanged(loopToggle);
		});


		audioVolumePanel.SetActive(false);

		//defaultAppVolumeComponent = FindObjectOfType<DefaultAppVolume>();
		//if(defaultAppVolumeComponent != null)
		//	setAudioVolumeSliderValue = defaultAppVolumeComponent.defaultAppVolume;

		setAudioVolumeSliderValue = PlayerPrefs.GetFloat("DefaultAppVolume");	
		

		//audioVolumeSlider.value = setAudioVolumeSliderValue;
		//skyboxMediaPlayer.Control.SetVolume(setAudioVolumeSliderValue);

		HideVideoControls();


		//returningToMenu = false;

		//Get Hand Selection
        masterHandSelection = FindObjectOfType<HandSelection>();
        xRNode = masterHandSelection.masterXRNode;	

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

        if (skyboxMediaPlayer && skyboxMediaPlayer.Info != null && skyboxMediaPlayer.Info.GetDurationMs() > 0f)
		{
			float time = skyboxMediaPlayer.Control.GetCurrentTimeMs();
			float duration = skyboxMediaPlayer.Info.GetDurationMs();
			float d = Mathf.Clamp(time / duration, 0.0f, 1.0f);

			// Debug.Log(string.Format("time: {0}, duration: {1}, d: {2}", time, duration, d));

            setVideoSeekerSliderValue = d;
			videoSeekerSlider.value = d;
		}

		if(skyboxMediaPlayer.Control.IsPlaying())
		{
			playButton.SetActive(false);
			pauseButton.SetActive(true);
		}
		else
		{
			playButton.SetActive(true);
			pauseButton.SetActive(false);
		}

        //Setup back button again

        // capturing trigger button press and release
        bool triggerButtonValue = false;
        InputFeatureUsage<bool> triggerButtonUsage = CommonUsages.triggerButton;
        
        if (device.TryGetFeatureValue(triggerButtonUsage, out triggerButtonValue) && triggerButtonValue && !triggerButtonIsPressed && videoControlsOn == true && pointerOverControls == false && pointerDownSwitch == false)
        {
			//disabled the back button here
            triggerButtonIsPressed = true;
            HideVideoControls();
        }
		else if (device.TryGetFeatureValue(triggerButtonUsage, out triggerButtonValue) && triggerButtonValue && !triggerButtonIsPressed && videoControlsOn == false)
        {
            //disabled the back button here
            triggerButtonIsPressed = true;
            ShowVideoControls();
        } 
        else if (!triggerButtonValue && triggerButtonIsPressed /*add here an && trigger is in raycast target area */)
        {
            triggerButtonIsPressed = false;
        }

        // capturing secondary button press and release
        bool secondaryButtonValue = false;

        // Oculus Secondary Button back button
		//InputFeatureUsage<bool> secondaryButtonUsage = CommonUsages.secondaryButton;

		// Pico Menu button as back button
		InputFeatureUsage<bool> secondaryButtonUsage = CommonUsages.menuButton;
        
        if (device.TryGetFeatureValue(secondaryButtonUsage, out secondaryButtonValue) && secondaryButtonValue && !secondaryButtonIsPressed && !returningToMenu)
        {
            //disabled the back button here
            secondaryButtonIsPressed = true;
			Debug.Log("Back menu video controls");
            BackToMenu();
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
           
                //get video length
                float duration = skyboxMediaPlayer.Info.GetDurationMs()/1000;  
                                           
                int durationInSecondsInt = (int)duration;
                int minutesDuration = (int)duration / 60;
                int secondsDuration = durationInSecondsInt - (minutesDuration * 60);
 
                string durationOfVideo = minutesDuration.ToString("D2") + ":" + secondsDuration.ToString("D2");
                string currentTime = minutes.ToString("D2") + ":" + seconds.ToString("D2");
                //string videoDuration = (string.Format("{0} / {1}", currentTime, durationOfVideo));
 
                //this is for your text ui to display current time
				timeCount.text = currentTime;
                timeDuration.text = durationOfVideo;
            }

			if(skyboxMediaPlayer.Control.IsLooping() != loopToggle.isOn)
			{
				skyboxMediaPlayer.Control.SetLooping(loopToggle.isOn);
			}
    }

	void LoopToggleValueChanged(Toggle change)
	{
		skyboxMediaPlayer.Control.SetLooping(loopToggle.isOn);
	}

	public void OnPointerEnter(PointerEventData eventData)
    {  
        pointerOverControls = true;
    }

	public void OnPointerExit(PointerEventData eventData)
    {		
		if (pointerDown == true)
		{
			pointerOverControls = true;
		}
		else
		{ 
			pointerOverControls = false;
		}
	}    

    // Callback function to handle events
	public void OnVideoEvent(MediaPlayer mp, MediaPlayerEvent.EventType et, ErrorCode errorCode)
	{
		switch (et)
		{
			case MediaPlayerEvent.EventType.Error:
			errorPanel.SetActive(true);
			OnPauseButton();
			break;
			case MediaPlayerEvent.EventType.ReadyToPlay:
			Debug.Log("Ready to play");
			break;
			case MediaPlayerEvent.EventType.Started:
			Debug.Log("Started");
			break;
			case MediaPlayerEvent.EventType.FirstFrameReady:
			Debug.Log("First frame ready");
			skyboxMediaPlayer.Control.SetVolume(setAudioVolumeSliderValue);
			if(/*defaultAppVolumeComponent != null && */ PlayerPrefs.GetInt("PlayMute") == 1){OnMuteButton();}
			audioVolumeSlider.value = setAudioVolumeSliderValue;
			break;
			case MediaPlayerEvent.EventType.FinishedPlaying:
			BackToMenu();
			break;
		}

		// Debug function below calls each video player function as it happens
		//Debug.Log("Event: " + et.ToString());
	}

    public void GoToPrevious()
    {        
        //write some script that will send you back to the previous menu structure you were on
        return;

    }

    public void LoadScene(string level)
    {
        SceneLoader.Instance.LoadNewScene(level);
    }

    public void BackToMenu()
    {
		returningToMenu = true;
		
		if(setVideoStandalone == true && standaloneMenuManager == null)
		{
			SceneLoader.Instance.LoadNewScene(setBackScene);
		}
		else if(setVideoStandalone == true && standaloneMenuManager != null)
		{
			skyboxMediaPlayer.Control.Stop();
			skyboxMediaPlayer.Control.Rewind();
			skyboxMediaPlayer.CloseVideo();
			HideVideoControls();
			standaloneMenuManager.CloseVideoPlane();
		}
		else
		{
			SceneLoader.Instance.ReturnToMenu();
		}

		returningToMenu = false;		
    }

    public void HideVideoControls()
    {
        // Setting bool for in clicking down
		foreach(PointerDownDetection pointerDetection in pointerDetectionArray)
		{
			if(pointerDetection.down == true)
			{
				return;
				// by returning here, if there is one that is down == true then it won't progress to the following actions
			}
		}
		
		videoControlsPanel.SetActive(false);
		videoControlsOn = false;
    }

    void ShowVideoControls()
    {
        //if the follow head position bool is true then
		if (followHeadPosition == true)
		{
			// Could include up and down rotation to this as well
			cameraYRotation = Camera.main.transform.eulerAngles.y;
			rotationOffset.transform.rotation = Quaternion.Euler(0,cameraYRotation,0);
		}
		else
		{
			rotationOffset.transform.rotation = Quaternion.Euler(0,0,0);
		}

		if (!errorPanel.activeInHierarchy)
		{
			videoControlsPanel.SetActive(true);
			videoControlsOn = true;
		}
    }

    public void OnMuteButton()
	{
		previousVolume = skyboxMediaPlayer.Control.GetVolume();
		skyboxMediaPlayer.Control.MuteAudio(true);
		audioVolumeSlider.value = 0.0f;

		muteButton.SetActive(false);
		unmuteButton.SetActive(true);
	}

	public void OnUnmuteButton()
	{
		skyboxMediaPlayer.Control.MuteAudio(false);
		audioVolumeSlider.value = previousVolume;
		skyboxMediaPlayer.Control.SetVolume(audioVolumeSlider.value);

		muteButton.SetActive(true);
		unmuteButton.SetActive(false);
	}

	public void OnPlayButton()
	{
		skyboxMediaPlayer.Control.Play();
	}

	public void OnPauseButton()
	{
		skyboxMediaPlayer.Control.Pause();
	}

	public void OnVideoSeekSlider()
	{
		if (videoSeekerSlider && videoSeekerSlider.value != setVideoSeekerSliderValue)
		{
			skyboxMediaPlayer.Control.Seek(videoSeekerSlider.value * skyboxMediaPlayer.Info.GetDurationMs());
		}
	}

	public void OnVideoSliderDown()
	{
		wasPlayingOnScrub = skyboxMediaPlayer.Control.IsPlaying();
		if( wasPlayingOnScrub )
		{
			skyboxMediaPlayer.Control.Pause();
		}
		OnVideoSeekSlider();
	}
	
	public void OnVideoSliderUp()
	{
		if(wasPlayingOnScrub)
		{
			skyboxMediaPlayer.Control.Play();
			wasPlayingOnScrub = false;
		}			
	}

	public void OnAudioVolumeSlider()
	{
		skyboxMediaPlayer.Control.SetVolume(audioVolumeSlider.value);
	}
    
}
