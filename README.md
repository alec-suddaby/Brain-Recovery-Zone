## 670_KDL_MVPA
Brain Recovery Zone VR App MVP

### Scene Navigation within Unity
The way I have developed this is to have a PersistentVR scene which hold the XR Rig and key XR Interaction elements that need to remain consistent throughout the app. By doing this, it also allows for scripts to continue unning between scene loads and save information into strings to save between scenes.

Therefore, each scene is loaded addativley using the SceneLoader prefab and script.

Whilst in Unity Editor, to change scenes, go to the Scenes menu at the top of the screen and change scenes there. To edit which scenes are viewed there, edit the SceneMenu script within the Editor folder in Assets.

### Accessing videos
Because the amount of videos seen in the BRZ is around 10GB, they are installed separatley on the headset currently. Therefore only the menu background video is featured in the Streaming Assets folder, the rest are linked to on the headset locally. This link is file://mnt/sdcard/BrainRecoveryZoneVideos/01Discover/01brain.mp4 for Oculus and Pico. After some R&D from Jack, it is possble to add these to the /Android/OBB/com.visualise.brainrecoveryzone folder on the headset. Then encrypt them by offsetting the bits in command line and referencing that offset in AVPro video within Unity.

###
Launching in Kiosk Mode on the Pico. Download this file: https://fwo3dh6l8i.feishu.cn/file/boxcnHBKK3suaJwsw8emHo5g8Ih and change the package name to: com.visualise.brainrecoveryzone and then save it on the InternalStorage on the pico headset. Follow these instructions here for more information http://static.appstore.picovr.com/docs/KioskMode/chapter_one.html

### Version history

0.#.#
MVP Testing Product

0.0.#
Building out the menu system

0.1.#
Adding in the video players and video assets
