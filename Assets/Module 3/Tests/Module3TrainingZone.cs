using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static DiceRollTask;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class Module3TrainingZone : MonoBehaviour
{
    private IEnumerator LoadScene(int difficultyLevel, string levelName)
    {
        SceneManager.LoadScene("MainMenu");
        yield return new WaitForSecondsRealtime(0.5f);
        LoadLevelSettings.loadLevelSettings.SetDifficultyLevel(difficultyLevel);
        SceneManager.LoadScene(levelName);
        yield return new WaitForSecondsRealtime(0.25f);
    }

    [UnityTest]
    public IEnumerator PlaneScanningAndFollowingTests()
    {
        Time.timeScale = 1f;

        yield return LoadScene(1, "Module3Training1");

        TaskCountdown countdown = GameObject.FindObjectOfType<TaskCountdown>();
        DestroyImmediate(countdown);

        PlaneScanningTask planeSpawner = GameObject.FindObjectOfType<PlaneScanningTask>();
        planeSpawner.InitTask();

        TriggerPressCounter triggerCounter = GameObject.FindObjectOfType<TriggerPressCounter>();

        float taskDuration = planeSpawner.taskLengthSeconds;

        bool taskComplete = false;
        planeSpawner.taskCompleted.AddListener(() => taskComplete = true);

        Time.timeScale = 10f;

        int targetPlanes = 0;
        List<PlaneMovementByDistance> planes = new List<PlaneMovementByDistance>();

        float timeElapsed = 0f;

        while (!taskComplete)
        {
            PlaneMovementByDistance[] currentPlanes = GameObject.FindObjectsOfType<PlaneMovementByDistance>().Where(x => x.isTarget).ToArray();

            for (int i = 0; i < currentPlanes.Length; i++)
            {
                if (!planes.Contains(currentPlanes[i]))
                {
                    targetPlanes++;
                    currentPlanes[i].fadingOut.AddListener(() => { triggerCounter.AddButtonPress(); });
                }
            }

            planes = currentPlanes.ToList();

            yield return new WaitForEndOfFrame();

            timeElapsed += Time.deltaTime;
        }

        Assert.IsTrue(Mathf.Abs(taskDuration - timeElapsed) < 2f, $"Task didn't finish at the expected time. Missed by {Mathf.Abs(planeSpawner.taskLengthSeconds - timeElapsed)}");
        Assert.IsTrue(planeSpawner.ExpectedTriggerPresses == targetPlanes, $"Expected: {planeSpawner.ExpectedTriggerPresses} planes. Counted: {targetPlanes} planes.");
        float score = planeSpawner.CheckScore();
        Assert.IsTrue(score == 100f, $"Incorrect score achieved. Expected 100%. Found {score}%");

        Time.timeScale = 1f;
    }


    [UnityTest]
    public IEnumerator VideoQuestionTaskTests()
    {
        Time.timeScale = 15f;

        //Have to ignore errors for video not loading correctly
        LogAssert.ignoreFailingMessages = true;
        //LogAssert.Expect(LogType.Error, "VideoPlayer cannot play url : file:///storage/emulated/0/BRZ_ASSETS/files/Module3TrainingLevel3.mp4\n\nCannot read file.");

        //LogAssert.Expect(LogType.Error, "VideoPlayer cannot play url : file:///storage/emulated/0/BRZ_ASSETS/files/Module3TrainingLevel3.mp4");

        yield return LoadScene(1, "Module3Training3");

        VideoQuestionTask videoQuestionTask = GameObject.FindObjectOfType<VideoQuestionTask>();
        QuestionDisplay questionDisplay = GameObject.FindObjectOfType<QuestionDisplay>();
        AnswerDisplay answerDisplay = GameObject.FindObjectOfType<AnswerDisplay>(true);

        int expectedScore = 0;

        bool taskComplete = false;
        videoQuestionTask.taskCompleted.AddListener(() => taskComplete = true);

        while (!taskComplete)
        {
            yield return new WaitForEndOfFrame();

            if (questionDisplay.mask.gameObject.activeSelf && !taskComplete)
            {
                questionDisplay.continueButton.onClick.Invoke();
                yield return new WaitForFixedUpdate();
            }

            if (!answerDisplay.mask.gameObject.activeSelf && !taskComplete)
            {
                continue;
            }

            yield return new WaitForSecondsRealtime(1);

            TimedQuestion currentQuestion = videoQuestionTask.CurrentQuestion;

            AnswerButton[] answerButtons = answerDisplay.transform.GetComponentsInChildren<AnswerButton>();

            bool answerCorrectly = UnityEngine.Random.Range(0f, 1f) > 0.5f;
            if(answerCorrectly)
            {
                expectedScore++;
            }

            AnswerButton buttonToClick = answerButtons.Where(x => x.Correct == answerCorrectly).OrderBy(x => UnityEngine.Random.Range(0f, 1f)).First();
            buttonToClick.GetComponent<Button>().onClick.Invoke();
                        

            yield return new WaitForSecondsRealtime(1);
        }

        Assert.IsTrue(videoQuestionTask.Score == expectedScore, $"Expected score: {expectedScore} did not match actual score: {videoQuestionTask.Score}");

        Time.timeScale = 1f;
    }


    [UnityTest]
    public IEnumerator TableTennisAceRallyTests()
    {
        float timeScale = 5f;
        Time.timeScale = timeScale;

        yield return LoadScene(1, "Module3TrainingPong");

        PongRally pongRally = GameObject.FindObjectOfType<PongRally>();
        PlayerController[] playerControllers = GameObject.FindObjectsOfType<PlayerController>();
        PongBall pongBall = GameObject.FindObjectOfType<PongBall>();

        int score = 0;
        pongRally.pointScored.AddListener(() => score++);

        int targetScore = 10;

        while(score < targetScore)
        {
            foreach(PlayerController playerController in playerControllers)
            {
                Vector3 paddlePosition = playerController.transform.position;
                paddlePosition.y = pongBall.transform.position.y;
                playerController.transform.position = paddlePosition;
            }

            yield return new WaitForFixedUpdate();
        }

        foreach (PlayerController playerController in playerControllers)
        {
            playerController.gameObject.SetActive(false);
        }

        bool gameOver = false;
        pongRally.gameOver.AddListener(() => gameOver = true);

        float timeElapsed = 0;

        while (!gameOver)
        {
            yield return new WaitForFixedUpdate();
            timeElapsed += Time.fixedDeltaTime / timeScale;

            if(timeElapsed > 5)
            {
                Assert.IsTrue(timeElapsed < 5, "Game did not end within expected time.");
                break;
            }
        }

        Assert.IsTrue(pongRally.Score == targetScore, $"Incorrect number of points scored. Scored: {pongRally.Score}");
    }
}
