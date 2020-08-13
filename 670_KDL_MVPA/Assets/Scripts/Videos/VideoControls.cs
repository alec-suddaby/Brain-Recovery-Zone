using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProVideo;
using UnityEngine.UI;

//-----------------------------------------------------------------------------
// Created by Jack Churchill
//-----------------------------------------------------------------------------

public class VideoControls : MonoBehaviour
{
    public MediaPlayer skyboxMediaPlayer;

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

	//public GameObject errorPanel;

	void Start()
	{
		//errorPanel.SetActive(false);
		
		skyboxMediaPlayer.Events.AddListener(OnVideoEvent);

		playButton.SetActive(true);
		pauseButton.SetActive(false);

		muteButton.SetActive(true);
		unmuteButton.SetActive(false);

		audioVolumeSlider.value = setAudioVolumeSliderValue;		
	}

	private void OnDestroy()
	{
		skyboxMediaPlayer.Events.RemoveListener(OnVideoEvent);
	}

	void Update()
	{				
		if (skyboxMediaPlayer && skyboxMediaPlayer.Info != null && skyboxMediaPlayer.Info.GetDurationMs() > 0f)
		{
			float time = skyboxMediaPlayer.Control.GetCurrentTimeMs();
			float duration = skyboxMediaPlayer.Info.GetDurationMs();
			float d = Mathf.Clamp(time / duration, 0.0f, 1.0f);

			// Debug.Log(string.Format("time: {0}, duration: {1}, d: {2}", time, duration, d));

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
	}

	// Callback function to handle events
	public void OnVideoEvent(MediaPlayer mp, MediaPlayerEvent.EventType et, ErrorCode errorCode)
	{
		switch (et)
		{
			case MediaPlayerEvent.EventType.Error:
			//errorPanel.SetActive(true);
			//OnPauseButton();
			break;
			case MediaPlayerEvent.EventType.ReadyToPlay:
			break;
			case MediaPlayerEvent.EventType.Started:
			break;
			case MediaPlayerEvent.EventType.FirstFrameReady:
			break;
			case MediaPlayerEvent.EventType.FinishedPlaying:
			GetComponent<VideoMenuManager>().BackToMenu();
			break;
		}

		Debug.Log("Event: " + et.ToString());
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
