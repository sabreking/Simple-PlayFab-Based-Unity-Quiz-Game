using System;
using UnityEngine;

public class Quizer : BroadcasterAndReceiver
{
    [SerializeField] private bool _isDisplayingAQuestion;
    [SerializeField] private bool _isQuizSessionRunning;
    private QuestionDifficulty _difficultyToUse = QuestionDifficulty.Easy;//changes from event fired from difficulty adapted defaults to easy

    protected override void SubscribeToEvents()
    {
        EventSys.onPlayButtonTriggered.AddListener(StartQuizSession);
        EventSys.onOutOfQuestions.AddListener(EndQuizSession);
        EventSys.onGameWon.AddListener(EndQuizSession);
        EventSys.onGameLost.AddListener(EndQuizSession);
        EventSys.onQuestionWindowIdle.AddListener(delegate
        {
            _isDisplayingAQuestion = false;
            if (_isQuizSessionRunning)
            {
                RequestNewQuestion();
            }
        });
        EventSys.onOutOfQuestionsInDiffculty.AddListener(ReRequestQuestionInDifferentDifficulty);
        EventSys.onChangeDifficulty.AddListener(delegate (QuestionDifficulty difficulty)
        { _difficultyToUse = difficulty; });
    }

    private void StartQuizSession()
    {
        _isQuizSessionRunning = true;
        RequestNewQuestion();
    }

    private void EndQuizSession()
    {
        Debug.Log("Ending Quiz session");
        _isQuizSessionRunning = false;
    }

    private void RequestNewQuestion()
    {
        _isDisplayingAQuestion = true;
        EventSys.onQuizPlayerWithNewQuestion.Invoke(AssessDifficultyForNextQuesiton());
    }

    /// <param name="emptyDifficulty">The difficulty no question could be found in</param>
    private void ReRequestQuestionInDifferentDifficulty(QuestionDifficulty emptyDifficulty)
    {
        switch (emptyDifficulty)
        {
            case QuestionDifficulty.Easy:
                EventSys.onQuizPlayerWithNewQuestion.Invoke(QuestionDifficulty.Medium);
                break;
            case QuestionDifficulty.Medium:
                EventSys.onQuizPlayerWithNewQuestion.Invoke(QuestionDifficulty.Hard);
                break;
            case QuestionDifficulty.Hard:
                EventSys.onOutOfQuestions.Invoke();
                break;
            default:
                throw new ArgumentOutOfRangeException("emptyDifficulty");
        }
    }

    private QuestionDifficulty AssessDifficultyForNextQuesiton()
    {
        return _difficultyToUse;
    }
}