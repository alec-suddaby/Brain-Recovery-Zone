using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.UI;
using UnityEditor;

public class StationAnnouncements : MonoBehaviour
{

    public List<AudioClip> genericAnnouncements;
    public List<AudioClip> specialAnnouncements;
    public List<Announcement> announcements;
    public List<AudioClip> preAnnouncementAlerts;

    public InputDevice controller;

    private float startTime = 0;
    public int startDelay = 5;
    //public float lengthMins = 8;
    private float lengthSeconds = 0;

    public float correctButtonPressThresholdSeconds = 5f;

    public int numberOfGenericAnnouncements = 0;
    public int numberOfSpecialAnnouncements = 0;

    public float timeBetweenAnnouncements = 0f;

    private float preAnnouncementAlertLength = 0f;
    private float nextAnnouncement = 0;
    private List<float> buttonPressTimes = new List<float>();
    public Text buttonPressText;
    public Text correctButtonPressText;

    public Text timeRemainingText;

    public AudioSource audioSource;

    public UnityEvent completeSession;
    private bool complete;
    private int played = 0;

    private bool buttonPressed = false;

    public string AlerterSoundPrefName;
    public AudioClip AlerterSound;

    public Text taskExplanationText;
    public string taskExplanation;

    public SelfEvaluation selfEvaluation;
    public string levelName;
    public ChangePrefs reminderCanvas;
    private int correctButtonPresses = 0;

    [Range(0,1)]
    public float announcementPointInAudio = 0.2f;

    public bool fixedGapBetweenStartOfAnnouncements = false;
    public bool alerterSoundEnabled = true;
    public bool reminderEnabled = true;


    [System.Serializable]
    public struct Announcement{
        public AudioClip announcementAudio;
        public bool isSpecialAnnouncement;
        private List<float> timesPlayedAt;
        public float SetPlayTime{
            set{
                timesPlayedAt.Add(value);
            }
        }

        public List<float> GetPlayTimes{
            get{
                return timesPlayedAt;
            }
        }

        public Announcement(AudioClip audio, bool special){
            announcementAudio = audio;
            isSpecialAnnouncement = special;
            timesPlayedAt = new List<float>();
        }
    }

    public void SaveScore(){
        if(!gameObject.activeSelf){
            return;
        }
        selfEvaluation.SaveSelfEvaluation(levelName, PlayerPrefs.GetInt(AlerterSoundPrefName) == 1, reminderCanvas.GetBool(), correctButtonPresses, buttonPressTimes.Count, numberOfSpecialAnnouncements);
    }

    public T GetNextFromList<T>(List<T> list){
        T temp = list[0];
        list.RemoveAt(0);
        list.Add(temp);
        return temp;
    }

    void Start(){
        if(reminderEnabled)
            taskExplanationText.text = taskExplanation;
        if(alerterSoundEnabled && PlayerPrefs.HasKey(AlerterSoundPrefName) && PlayerPrefs.GetInt(AlerterSoundPrefName) == 1){
            preAnnouncementAlerts.Add(AlerterSound);
        }
        startTime = Time.time;
        var leftHandDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.LeftHand, leftHandDevices);
        ShuffleList<AudioClip>(genericAnnouncements);
        ShuffleList<AudioClip>(specialAnnouncements);
        if(leftHandDevices.Count > 0)
            controller = leftHandDevices[0];

        announcements = new List<Announcement>();
        for(int i = 0; i < numberOfGenericAnnouncements; i++){
            announcements.Add(new Announcement(GetNextFromList<AudioClip>(genericAnnouncements), false));
        }

        for(int i = 0; i < numberOfSpecialAnnouncements; i++){
            announcements.Add(new Announcement(GetNextFromList<AudioClip>(specialAnnouncements), true));
        }

        ShuffleList<Announcement>(announcements);

        preAnnouncementAlertLength = 0;
        foreach(AudioClip clip in preAnnouncementAlerts){
            preAnnouncementAlertLength += clip.length;
        }
        preAnnouncementAlertLength -= preAnnouncementAlerts.Count > 0 ? 5.5f : 0;

        lengthSeconds = 0;
        if(!fixedGapBetweenStartOfAnnouncements){
            foreach(Announcement announcement in announcements){
                lengthSeconds += announcement.announcementAudio.length;
            }
            
            
            lengthSeconds += preAnnouncementAlertLength * (announcements.Count - 1);
            lengthSeconds += 5f; 
        }

        lengthSeconds += ((fixedGapBetweenStartOfAnnouncements ? 1 : 0) + announcements.Count) * timeBetweenAnnouncements;

        //timeBetweenAnnouncements = lengthSeconds/(float)(numberOfGenericAnnouncements + numberOfSpecialAnnouncements);

        nextAnnouncement = startTime + startDelay;
    }

    void ShuffleList<T>(List<T> audioList){
        for(int i = 0; i < audioList.Count; i++){
            for(int j = 0; j < audioList.Count; j++){
                if(Random.Range(0f, 1f) > 0.5f){
                    T temp = audioList[i];
                    audioList[i] = audioList[j];
                    audioList[j] = temp;
                }
            }
        }
    }


    void AnnouncementsComplete(){
        completeSession.Invoke();

        correctButtonPresses = 0;
        foreach(Announcement announcement in announcements){
            foreach(float audioPlayTime in announcement.GetPlayTimes){
                foreach(float buttonPressedTime in buttonPressTimes){
                    if(buttonPressedTime - audioPlayTime > 0 && buttonPressedTime - audioPlayTime < (announcement.announcementAudio.length * announcementPointInAudio) + correctButtonPressThresholdSeconds){
                        correctButtonPresses++;
                        break;
                    }
                }
            }
        }

        correctButtonPressText.text = "Correct Button Presses: " + correctButtonPresses.ToString();
        buttonPressText.text = "Total Button Presses: " + buttonPressTimes.Count.ToString();
    }

    void Update(){
        if(Time.time >= startTime + startDelay + lengthSeconds){
            if(!complete){
                AnnouncementsComplete();
                complete = true;
            }
            return;
        }
        float timeRemaining = Mathf.Clamp(startTime + startDelay + lengthSeconds - Time.time, 0f, float.MaxValue);
        int seconds = (int)(timeRemaining % 60);
        int minutes = (int)(timeRemaining/60f);
        timeRemainingText.text = (minutes < 10 ? "0" + minutes.ToString() : minutes.ToString()) + ":" + (seconds < 10 ? "0" + seconds.ToString() : seconds.ToString());

        if(controller != null){
            bool triggerValue;
            if (controller.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerValue) && triggerValue)
            {
                if(!buttonPressed){
                    buttonPressed = true;
                    buttonPressTimes.Add(Time.time);
                    buttonPressText.text = buttonPressTimes.Count.ToString();
                }
            }else{
                buttonPressed = false;
            }
        }

        if(played >= announcements.Count){
            return;
        }
        if(nextAnnouncement < Time.time){
            nextAnnouncement += (fixedGapBetweenStartOfAnnouncements ? 0 : announcements[0].announcementAudio.length) + timeBetweenAnnouncements + preAnnouncementAlertLength;
            played += 1;
            StartCoroutine(PlayAnnouncement());
        }
    }

    IEnumerator PlayAnnouncement(){
        bool first = true;
        foreach(AudioClip clip in preAnnouncementAlerts){
            audioSource.PlayOneShot(clip);
            yield return new WaitForSeconds(first ? Mathf.Clamp(clip.length - 5.5f, 0, float.MaxValue) : clip.length);
            first = false;
        }

        Announcement current = GetNextFromList<Announcement>(announcements);
        if(current.isSpecialAnnouncement)
            current.SetPlayTime = Time.time;
        audioSource.PlayOneShot(current.announcementAudio);
    }
}