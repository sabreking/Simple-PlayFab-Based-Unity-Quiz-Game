using System;
using System.Collections.Generic;
using System.Linq;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class QuestionsManager : BroadcasterAndReceiver
{
    public List<Question> allDownloadedQuestions;
    public bool isOutOfQuestions;

    public int nextQuestion;

    protected override void Start()
    {
        base.Start();
    }

    protected override void SubscribeToEvents()
    {
        EventSys.onPlayerLoggedIn.AddListener(ImportQuestionsFromServer);
        EventSys.onQuizPlayerWithNewQuestion.AddListener(ShowNextQuestion);
    }

    private void ImportQuestionsFromServer()
    {
        var getRequest = new GetTitleDataRequest();
        PlayFabClientAPI.GetTitleData(getRequest, result =>
        {
            var questionStrings = ExtractDataFromCsvString(result.Data["Questions"]);
            var answer0Strings = ExtractDataFromCsvString(result.Data["Answer0"]);
            var answer1Strings = ExtractDataFromCsvString(result.Data["Answer1"]);
            var answer2Strings = ExtractDataFromCsvString(result.Data["Answer2"]);
            var answer3Strings = ExtractDataFromCsvString(result.Data["Answer3"]);
            var correctIndexStrings = ExtractDataFromCsvString(result.Data["CorrectIndex"]);
            var difficultyStrings = ExtractDataFromCsvString(result.Data["Difficulty"]);

            for (var i = 0; i < questionStrings.Length; i++)
            {
                string[] answerStrings = {answer0Strings[i], answer1Strings[i], answer2Strings[i], answer3Strings[i]};

                allDownloadedQuestions.Add(new Question(int.Parse(correctIndexStrings[i]), answerStrings, questionStrings[i], (QuestionDifficulty) Enum.Parse(typeof (QuestionDifficulty), difficultyStrings[i])));
            }
            EventManager.Instance.onQuestionsDownloaded.Invoke();
        }, error =>
        {
            Debug.Log("Got error getting titleData:");
            Debug.Log(error.ErrorMessage);
        });
    }

    private void ShowNextQuestion(QuestionDifficulty difficultyToUse)
    {
        var chosenQuestion = FindQuestionByDifficulty(difficultyToUse);
        if (chosenQuestion != null)
        {

            chosenQuestion.isAlreadyShownOnce = true;
            EventSys.onNextQuestionForQuestionWindow.Invoke(chosenQuestion); // sending the question to the question window
        } else
        {
            EventSys.onOutOfQuestionsInDiffculty.Invoke(difficultyToUse);//letting the Quizer know.
        }
    }

    Question FindQuestionByDifficulty(QuestionDifficulty difficulty)
    {
        return (from question in allDownloadedQuestions
                where (question.isAlreadyShownOnce == false && question.questionDifficulty == difficulty)
                select question).FirstOrDefault();
    }
    private string[] ExtractDataFromCsvString(string playFabCsvString)
    {
        return (playFabCsvString.Trim()).Split(","[0]);
    }
}