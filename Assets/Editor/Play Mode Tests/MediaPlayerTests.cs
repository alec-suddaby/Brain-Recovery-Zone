using NUnit.Framework;
using RenderHeads.Media.AVProVideo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class MediaPlayerTests
{
    private MediaPlayer GetMediaPlayer()
    {
        GameObject gameObject = new GameObject();
        MediaPlayer mediaPlayer = gameObject.AddComponent<MediaPlayer>();
        Assert.IsTrue(mediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder, "MenuBG/00100.mp4", false));
        return mediaPlayer;
    }

    [Test]
    public void MediaPlayerLoadVideo_Test89()
    {
        GetMediaPlayer();
    }

    #region Media Controls Tests
    [UnityTest]
    public IEnumerator MediaPlayerControlsTest_Test90()
    {
        MediaPlayer mediaPlayer = GetMediaPlayer();

        yield return PlayTest(mediaPlayer);
        yield return PauseTest(mediaPlayer);
        SkipTest(mediaPlayer);
        StopTest(mediaPlayer);
    }

    private void SkipTest(MediaPlayer mediaPlayer)
    {
        if (mediaPlayer.Control.IsPlaying())
        {
            mediaPlayer.Control.Pause();
        }

        float time = mediaPlayer.Control.GetCurrentTimeMs();

        mediaPlayer.Control.Seek(time);

        Assert.IsTrue(mediaPlayer.Control.GetCurrentTimeMs() == time);
    }

    private IEnumerator PlayTest(MediaPlayer mediaPlayer)
    {
        if (mediaPlayer.Control.IsPlaying())
        {
            mediaPlayer.Control.Pause();
        }

        float time = mediaPlayer.Control.GetCurrentTimeMs();

        mediaPlayer.Control.Play();
        yield return new WaitForSecondsRealtime(1.1f);

        Assert.IsTrue(mediaPlayer.Control.GetCurrentTimeMs() - time > 1000);
    }

    private IEnumerator PauseTest(MediaPlayer mediaPlayer)
    {
        if (mediaPlayer.Control.IsPaused())
        {
            mediaPlayer.Control.Play();
        }

        float time = mediaPlayer.Control.GetCurrentTimeMs();

        mediaPlayer.Control.Pause();
        yield return new WaitForSecondsRealtime(1.1f);

        Assert.IsTrue(mediaPlayer.Control.GetCurrentTimeMs() - time < 100);
    }

    private void StopTest(MediaPlayer mediaPlayer)
    {
        Assert.IsTrue(mediaPlayer.Control.IsPlaying() ||  mediaPlayer.Control.IsPaused());
        mediaPlayer.Control.Stop();
        Assert.IsFalse(mediaPlayer.Control.IsPlaying() || mediaPlayer.Control.IsPaused());
    }
    #endregion

    [Test]
    public void MediaPlayerLoadVideo_Test91()
    {
        MediaPlayer mediaPlayer = GetMediaPlayer();

        mediaPlayer.Control.Play();
        mediaPlayer.Control.CloseVideo();

        Assert.IsTrue(mediaPlayer.Info.GetVideoHeight() == 0);
        Assert.IsTrue(mediaPlayer.Info.GetVideoWidth() == 0);
        Assert.IsTrue(mediaPlayer.Info.GetVideoFrameRate() == 0);
        Assert.IsTrue(!mediaPlayer.Control.CanPlay());
    }
}
