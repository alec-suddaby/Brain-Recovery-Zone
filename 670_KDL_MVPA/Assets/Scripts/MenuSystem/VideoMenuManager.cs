using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;
using RenderHeads.Media.AVProVideo;

public class VideoMenuManager : MonoBehaviour
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
	private float setAudioVolumeSliderValue = 1.0f;


    [Header("Media Player")]
    public MediaPlayer skyboxMediaPlayer;


    [Header("XR Controls")]
    public GameObject rotationOffset;

    //XR Toolkit controllers
    [SerializeField]
    private XRNode xRNode = XRNode.RightHand;
    private List<InputDevice> devices = new List<InputDevice>();
    private InputDevice device;

    //to avoid repeat readings
    private bool triggerButtonIsPressed;
    private bool secondaryButtonIsPressed;

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
        HideVideoControls();

        errorPanel.SetActive(false);
		
		skyboxMediaPlayer.Events.AddListener(OnVideoEvent);

		playButton.SetActive(true);
		pauseButton.SetActive(false);

		muteButton.SetActive(true);
		unmuteButton.SetActive(false);

		audioVolumeSlider.value = setAudioVolumeSliderValue;
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
        
        if (device.TryGetFeatureValue(triggerButtonUsage, out triggerButtonValue) && triggerButtonValue && !triggerButtonIsPressed)
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
        InputFeatureUsage<bool> secondaryButtonUsage = CommonUsages.secondaryButton;
        
        if (device.TryGetFeatureValue(secondaryButtonUsage, out secondaryButtonValue) && secondaryButtonValue && !secondaryButtonIsPressed)
        {
            //disabled the back button here
            secondaryButtonIsPressed = true;
            BackToMenu();
        }
        else if (!secondaryButtonValue && secondaryButtonIsPressed)
        {
            secondaryButtonIsPressed = false;
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
			break;
			case MediaPlayerEvent.EventType.Started:
			break;
			case MediaPlayerEvent.EventType.FirstFrameReady:
			break;
			case MediaPlayerEvent.EventType.FinishedPlaying:
			BackToMenu();
			break;
		}

		Debug.Log("Event: " + et.ToString());
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
        SceneLoader.Instance.ReturnToMenu();
    }

    public void HideVideoControls()
    {
        videoControlsPanel.SetActive(false);
    }

    void ShowVideoControls()
    {
        videoControlsPanel.SetActive(true);
        //rotationOffset.GetComponent<VideoControlsTrackRotate>().GetCameraRotatePosition();
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
