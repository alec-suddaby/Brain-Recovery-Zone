using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

public class VideoQuestionTask : TaskCountdown
{
    public int taskLengthPadding = 60;
    public VideoPlayer videoPlayer;
    private bool questionsDisplayed = false;
    public List<TimedQuestion> questions;

    public List<TimedQuestion> selectedQuestions;
    
    private TimedQuestion currentQuestion;
    public TimedQuestion CurrentQuestion => currentQuestion;

    public QuestionDisplay questionDisplay;
    public AnswerDisplay answerDisplay;

    private int score = 0;
    public int Score => score;

    private int numberOfQuestions = 0;
    public int NumberOfQuestions => numberOfQuestions;

    public int incorrectAnswersPerQuestion = 3;  

    public override void InitTask()
    {
        timeLimitEnabled = false;
        UpdateQuestionPlayTimes();
        SelectQuestions();
        numberOfQuestions = selectedQuestions.Count;
        taskLengthSeconds = selectedQuestions[selectedQuestions.Count - 1].time + taskLengthPadding;
        base.InitTask();
    }

    private void SelectQuestions(){
        selectedQuestions.Clear();
        SortQuestions(questions);

        bool selectedTimeChanged = true;
        int selectedTime = -1;

        while (true){
            selectedTimeChanged = false;

            foreach(TimedQuestion question in questions){
                if(question.time > selectedTime){
                    selectedTime = question.time;
                    selectedTimeChanged = true;
                    break;
                }
            }

            if(!selectedTimeChanged){
                break;
            }

            List<TimedQuestion> questionOptions = new List<TimedQuestion>();
            foreach(TimedQuestion question in questions){
                if(question.time == selectedTime){
                    questionOptions.Add(question);
                }
            }

            selectedQuestions.Add(questionOptions[Random.Range(0,questionOptions.Count)]);            
        }

        SortQuestions(selectedQuestions);
    }

    void SortQuestions(List<TimedQuestion> questionList){
        for(int i = 0; i < questionList.Count - 1; i++){
            for(int j = i + 1; j < questionList.Count; j++){
                if(questionList[j].time > questionList[i].time){
                    continue;
                }

                TimedQuestion question = questionList[i];
                questionList[i] = questionList[j];
                questionList[j] = question;
            }
        }
    }

    protected override void Tick()
    {
        if(questionsDisplayed || !beginTask){
            return;
        }

        if((selectedQuestions.Count > 0 && timeElapsed > selectedQuestions[0].time) || timeElapsed >= taskLengthSeconds){
            DisplayAnswerPrompt();
        }

        base.Tick();
    }

    //Display the answer prompt to the user
    private void DisplayAnswerPrompt(){
        questionsDisplayed = true;
        videoPlayer.Pause();

        if(currentQuestion == null){
            DisplayNextQuestion();
            return;
        }
        
        answerDisplay.Answered.AddListener(QuestionAnswered);

        List<string> answers = currentQuestion.multipleChoiceQuestion.incorrectAnswerOptions.ToList();
        for(int i = 0; i < answers.Count - 1; i++){
            for(int j = 0; j < answers.Count; j++){
                if(Random.Range(0f,1f) > 0.5f){
                    string temp = answers[i];
                    answers[i] = answers[j];
                    answers[j] = temp;
                }
            }
        }

        while(answers.Count > incorrectAnswersPerQuestion){
            answers.RemoveAt(answers.Count - 1);
        }

        answerDisplay.SetQuestionAndAnswers(currentQuestion.multipleChoiceQuestion.question, currentQuestion.multipleChoiceQuestion.correctAnswer, answers.ToArray());        
    }

    //Display the next question to the user
    private void DisplayNextQuestion(){
        if(selectedQuestions.Count == 0){
            TaskCompleted();
            return;
        }

        currentQuestion = selectedQuestions[0];
        selectedQuestions.RemoveAt(0);

        questionDisplay.continueClicked.AddListener(Continue);
        questionDisplay.SetQuestion(currentQuestion.multipleChoiceQuestion.question);
    }

    //Call this when the user clears the new question prompt
    public void Continue(){
        Debug.Log("Continue Clicked");
        questionsDisplayed = false;
        videoPlayer.Play();
    }

    //Call when question has been answered by user
    private void QuestionAnswered(bool correct){
        if(correct){
            score++;
        }

        DisplayNextQuestion();
    }

    public void UpdateQuestionPlayTimes(){
        for(int i = 0; i < questions.Count; i++){
            questions[i].time = questions[i].multipleChoiceQuestion.playTimeSeconds;
        }
    }
}
