using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using static DiceRollTask;

public class Module3WarmupZoneTests
{
    private IEnumerator LoadWarmupZone(int difficultyLevel, string levelName)
    {
        SceneManager.LoadScene("MainMenu");
        yield return new WaitForSecondsRealtime(0.5f);
        LoadLevelSettings.loadLevelSettings.SetDifficultyLevel(difficultyLevel);
        SceneManager.LoadScene(levelName);
        yield return new WaitForSecondsRealtime(0.25f);
    }

    [UnityTest]
    public IEnumerator DiceRollTests()
    {
        yield return LoadWarmupZone(1, "Module3DiceRoll");

        DiceRollTask diceRollTask = GameObject.FindObjectOfType<DiceRollTask>();

        GameObject.DestroyImmediate(diceRollTask.GetComponent<TaskCountdown>());

        int additionalDice = diceRollTask.additionalDice.Count;
        int masterDice = diceRollTask.masterDice.Count;
        int comparisonDice = diceRollTask.comparisonDice.Count;
        int totalDice = additionalDice + masterDice + comparisonDice;

        diceRollTask.Next();
        
        while(diceRollTask.RoundsCompleted < diceRollTask.rounds)
        {
            int i = diceRollTask.RoundsCompleted;

            int previousScore = diceRollTask.Score;

            bool matching = false;
            foreach(ReplayDice masterDie in diceRollTask.masterDice)
            {
                if(diceRollTask.comparisonDice.Where(x => x.CurrentFace == masterDie.CurrentFace).Count() > 0)
                {
                    matching = true;
                    break;
                }
            }

            diceRollTask.CheckCorrect(matching);

            //Check correct response is handled appropriately
            Assert.IsTrue(previousScore < diceRollTask.Score, "Score did not increase following correct answer.");

            //Check additional dice are being added when required
            foreach (AdditionalDice additionalDie in diceRollTask.additionalDice)
            {
                if(additionalDie.introductoryRound > i + 2)
                {
                    Assert.IsFalse(diceRollTask.masterDice.Contains(additionalDie.dice) || diceRollTask.comparisonDice.Contains(additionalDie.dice), $"An additional dice was found in either the master dice list or the comparison dice list prematurely. Round {i + 1}");
                    continue;
                }

                if(additionalDie.type == DieType.Master)
                {
                    Assert.IsTrue(diceRollTask.masterDice.Contains(additionalDie.dice), $"Additional die not correctly added to master dice list. Round {i + 1}");
                    continue;
                }

                Assert.IsTrue(diceRollTask.comparisonDice.Contains(additionalDie.dice), $"Additional die not correctly added to comparison dice list. Round {i + 1}");
            }

            //Ensure total number of dice doesn't change
            if (additionalDice != diceRollTask.additionalDice.Count)
            {
                additionalDice = diceRollTask.additionalDice.Count;

                Assert.IsTrue(diceRollTask.masterDice.Count > masterDice || diceRollTask.comparisonDice.Count > comparisonDice, "One or more additional dice where incorrectly moved.");

                masterDice = diceRollTask.masterDice.Count;
                comparisonDice = diceRollTask.comparisonDice.Count;

                Assert.IsTrue(additionalDice + masterDice + comparisonDice == totalDice, "The total number of dice changed");
            }
        }

        yield return LoadWarmupZone(1, "Module3DiceRoll");

        diceRollTask = GameObject.FindObjectOfType<DiceRollTask>();

        GameObject.DestroyImmediate(diceRollTask.GetComponent<TaskCountdown>());

        //Ensure incorrect answers are handled correctly
        for (int i = 0; i < diceRollTask.rounds; i++)
        {
            int previousScore = diceRollTask.Score;
            bool matching = false;
            foreach (ReplayDice masterDie in diceRollTask.masterDice)
            {
                if (diceRollTask.comparisonDice.Where(x => x.CurrentFace == masterDie.CurrentFace).Count() > 0)
                {
                    matching = true;
                    break;
                }
            }

            diceRollTask.CheckCorrect(!matching);

            //Check correct response is handled appropriately
            Assert.IsTrue(previousScore <= diceRollTask.Score, "Task score increased following incorrect answer");
        }
    }

    private IEnumerator ObjectScanningAndMatchingTests_CorrectAnswers(int difficultyLevel)
    {
        //Test correct selection
        yield return LoadWarmupZone(difficultyLevel, "Module3ObjectScanningAndMatching");

        ObjectScanningAndMatching objectScanningAndMatching = GameObject.FindObjectOfType<ObjectScanningAndMatching>();

        foreach (Icon icon in objectScanningAndMatching.mainSelector.GetComponentsInChildren<Icon>())
        {
            //Last match is automatically completed
            if (objectScanningAndMatching.mainSelector.RemainingIconCount == 0)
            {
                break;
            }

            objectScanningAndMatching.mainSelector.SelectIcon(icon);

            Icon otherIcon = objectScanningAndMatching.userMatchingSelector.GetComponentsInChildren<Icon>().First(x => x.GetIconType == icon.GetIconType);
            objectScanningAndMatching.userMatchingSelector.SelectIcon(otherIcon);

            objectScanningAndMatching.CheckForMatch();
        }

        Assert.IsTrue(objectScanningAndMatching.mainSelector.RemainingIconCount == 0 && (objectScanningAndMatching.userMatchingSelector.RemainingIconCount == 0 || !objectScanningAndMatching.allowDeactivation), $"An option was not matched when it should have been. Difficulty Level: {difficultyLevel}. Remaining main icon count: {objectScanningAndMatching.mainSelector.RemainingIconCount}. Remaining user icon count: {objectScanningAndMatching.userMatchingSelector.RemainingIconCount}");
    }

    private IEnumerator ObjectScanningAndMatchingTests_IncorrectAnswers(int difficultyLevel)
    {
        //Test incorrect selection doesn't work
        yield return LoadWarmupZone(difficultyLevel, "Module3ObjectScanningAndMatching");

        ObjectScanningAndMatching objectScanningAndMatching = GameObject.FindObjectOfType<ObjectScanningAndMatching>();
        objectScanningAndMatching.autoSwitch = false;

        int startMainIconCount = objectScanningAndMatching.mainSelector.RemainingIconCount;
        int startUserIconCount = objectScanningAndMatching.userMatchingSelector.RemainingIconCount;

        foreach (Icon icon in objectScanningAndMatching.mainSelector.GetComponentsInChildren<Icon>())
        {
            foreach (Icon userIcon in objectScanningAndMatching.userMatchingSelector.GetComponentsInChildren<Icon>())
            {
                if (userIcon.GetIconType == icon.GetIconType)
                {
                    continue;
                }

                objectScanningAndMatching.userMatchingSelector.SelectIcon(userIcon);
                break;
            }

            //Last match is automatically completed
            if (objectScanningAndMatching.mainSelector.RemainingIconCount == 0)
            {
                break;
            }

            objectScanningAndMatching.mainSelector.SelectIcon(icon);

            foreach (Icon userIcon in objectScanningAndMatching.userMatchingSelector.GetComponentsInChildren<Icon>())
            {
                if (userIcon.GetIconType == icon.GetIconType)
                {
                    continue;
                }

                objectScanningAndMatching.userMatchingSelector.SelectIcon(userIcon);
                objectScanningAndMatching.CheckForMatch();
            }
        }

        Assert.IsTrue(objectScanningAndMatching.mainSelector.RemainingIconCount == startMainIconCount && objectScanningAndMatching.userMatchingSelector.RemainingIconCount == startUserIconCount, $"An option was correctly matched when it should not have been. Difficulty Level: {difficultyLevel}");
    }

    [UnityTest]
    public IEnumerator ObjectScanningAndMatchingTests()
    {
        for(int i = 1; i <= 8; i++)
        {
            yield return ObjectScanningAndMatchingTests_CorrectAnswers(i);
            yield return ObjectScanningAndMatchingTests_IncorrectAnswers(i);
        }
    }

    [UnityTest]
    public IEnumerator ZoneInTests()
    {
        yield return LoadWarmupZone(1, "Module3ZoneIn");
        GameObject.Destroy(GameObject.FindObjectOfType<TaskCountdown>());

        ZoneIn zoneInTask = GameObject.FindObjectOfType<ZoneIn>(true);
        zoneInTask.gameObject.SetActive(true);

        yield return new WaitForFixedUpdate();

        int halfRounds = zoneInTask.rounds/2;

        for(int i = 0; i < halfRounds; i++)
        {
            int currentScore = zoneInTask.Score;
            int currentRound = zoneInTask.CurrentRound;

            zoneInTask.CurrentSignButton.Clicked();

            Assert.IsTrue(zoneInTask.Score > currentScore, "Score did not increase after correct answer");
            Assert.IsTrue(zoneInTask.CurrentRound == currentRound + 1, "Round number did not increment correctly for correct answer");
            Assert.IsTrue(zoneInTask.zoneInImage.texture == zoneInTask.CurrentSignButton.icon, "Target icon didn't change correctly following a correct answer");
            Assert.IsTrue(zoneInTask.roundText.text.Split(" ", StringSplitOptions.RemoveEmptyEntries).Contains(zoneInTask.CurrentRound.ToString()), "Round number incorrectly displayed");
        }

        for (int i = halfRounds; i < zoneInTask.rounds - 1; i++)
        {
            int currentScore = zoneInTask.Score;
            int currentRound = zoneInTask.CurrentRound;

            zoneInTask.starSigns.First(x => x != zoneInTask.CurrentSignButton).Clicked();

            Assert.IsTrue(zoneInTask.Score == currentScore, "Score changed after incorrect answer");
            Assert.IsTrue(zoneInTask.CurrentRound == currentRound + 1, "Round number did not increment correctly for incorrect answer");
            Assert.IsTrue(zoneInTask.zoneInImage.texture == zoneInTask.CurrentSignButton.icon, "Target icon didn't change correctly following an icorrect answer");
        }
    }

    [UnityTest]
    public IEnumerator RapidScanningTests()
    {
        yield return LoadWarmupZone(1, "Module3RapidScanning");
        GameObject.Destroy(GameObject.FindObjectOfType<TaskCountdown>());

        RapidScanning rapidScanning = GameObject.FindObjectOfType<RapidScanning>(true);
        rapidScanning.gameObject.SetActive(true);

        yield return new WaitForFixedUpdate();

        int halfRounds = rapidScanning.numberOfRounds / 2;

        for (int i = 0; i < halfRounds; i++)
        {
            float currentScore = rapidScanning.Score;
            int currentRound = rapidScanning.CurrentRound;

            rapidScanning.CheckAnswer(rapidScanning.images.Where(x => x.currentTexture == rapidScanning.targetIcon).Count() != 0);

            Assert.IsTrue(rapidScanning.Score > currentScore, "Score did not increase after correct answer");
            Assert.IsTrue(rapidScanning.CurrentRound == currentRound + 1, "Round number did not increment correctly for correct answer");
            Assert.IsTrue(rapidScanning.roundText.text.Split(" ", StringSplitOptions.RemoveEmptyEntries).Contains(rapidScanning.CurrentRound.ToString()), "Round number incorrectly displayed");
        }

        rapidScanning.CheckAnswer(false);

        for (int i = halfRounds; i < rapidScanning.numberOfRounds - 1; i++)
        {
            float currentScore = rapidScanning.Score;
            int currentRound = rapidScanning.CurrentRound;

            rapidScanning.CheckAnswer(rapidScanning.images.Where(x => x.currentTexture == rapidScanning.targetIcon).Count() == 0);

            Assert.IsTrue(rapidScanning.Score == currentScore, "Score changed after incorrect answer");
            Assert.IsTrue(rapidScanning.CurrentRound == currentRound + 1 || rapidScanning.CurrentRound == rapidScanning.numberOfRounds , $"Round number did not increment correctly for incorrect answer. Round {rapidScanning.CurrentRound}");
        }
    }

    private IEnumerator SetupPong(bool playerEnabled, bool opponentEnabled)
    {
        yield return LoadWarmupZone(1, "Module3Pong");

        GameObject.FindObjectOfType<PlayerController>().gameObject.SetActive(playerEnabled);
        GameObject.FindObjectOfType<OpponentController>().gameObject.SetActive(opponentEnabled);
    }

    [UnityTest]
    public IEnumerator TableTennisAceTests()
    {
        Time.timeScale = 5f;

        yield return SetupPong(true, false);      

        PongScoring pongScoring = GameObject.FindObjectOfType<PongScoring>();

        bool gameComplete = false;
        pongScoring.gameOver.AddListener(() => gameComplete = true);
        pongScoring.PointScored.AddListener(() => {
            IEnumerator SetBallDirection()
            {
                PongBall pongBall = GameObject.FindObjectOfType<PongBall>();
                BallCountDown ballCountDown = GameObject.FindObjectOfType<BallCountDown>();
                yield return new WaitForSeconds(ballCountDown.taskLengthSeconds - 2);
                pongBall.SetGoingRight(true); 
            }

            pongScoring.StartCoroutine(SetBallDirection());
        });

        while (!gameComplete)
        {
            yield return new WaitForFixedUpdate();
        }

        Assert.IsTrue(pongScoring.PlayerScore == pongScoring.scoreToWin, "Player didn't score the points to win");
        Assert.IsTrue(pongScoring.OpponentScore == 0, "Opponent scored a point when it shouldn't have");

        yield return SetupPong(false, true);

        pongScoring = GameObject.FindObjectOfType<PongScoring>();
        gameComplete = false;
        pongScoring.gameOver.AddListener(() => gameComplete = true);
        pongScoring.PointScored.AddListener(() => { 
            IEnumerator MoveBall()
            {
                BallCountDown ballCountDown = GameObject.FindObjectOfType<BallCountDown>();
                PongBall pongBall = GameObject.FindObjectOfType<PongBall>();

                yield return new WaitForSeconds(ballCountDown.taskLengthSeconds - 2);

                Vector3 ballPosition = pongBall.transform.localPosition;
                ballPosition.y = UnityEngine.Random.Range(-900f, 900f);
                pongBall.transform.localPosition = ballPosition;

                pongBall.SetGoingRight(false);
            }

            pongScoring.StartCoroutine(MoveBall());
        });

        while (!gameComplete)
        {
            yield return new WaitForFixedUpdate();
        }

        Assert.IsTrue(pongScoring.PlayerScore == 0, "Player scored when it shouldn't have");
        Assert.IsTrue(pongScoring.OpponentScore == pongScoring.scoreToWin, "Opponent didn't score the points to win");
    }
}
