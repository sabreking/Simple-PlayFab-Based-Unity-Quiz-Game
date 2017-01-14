using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : UnitySingleton<EventManager>
{
    //Game State Events
    [HideInInspector]
    public UnityEvent onLoginRequested;
    public UnityEvent onPlayerLoggedIn;
    public UnityEvent onLoginFailed;
    public UnityEvent onQuestionsDownloaded;

    public UnityEvent onQuestionWindowIdle;

    public UnityEvent onGameExitCall;
    public UnityEvent onPlayButtonTriggered;

    public UnityEvent onGameWon;
    public UnityEvent onGameLost;

    //Score Updates
    public IntEvent onScoreUpdate;
    public IntEvent onHighScoreUpdate;
    public IntEvent onLoseStreak;
    public IntEvent onWinStreak;

    //GamePlayEvents
    public QuestionEvent onQuestionAnsweredCorrectly;
    public QuestionEvent onNextQuestionForQuestionWindow;
    public QuestionRequestEvent onQuizPlayerWithNewQuestion;
    public UnityEvent onQuestionAnsweredInCorrectlyOrTimesUp;
    

    //Logic Events
    public UnityEvent onOutOfQuestions;
    public QuestionRequestEvent onOutOfQuestionsInDiffculty;

    //Difficulty Adapter Events
    public QuestionRequestEvent onChangeDifficulty;
}

[Serializable]
public class IntEvent : UnityEvent<int> { }

[Serializable]
public class QuestionEvent : UnityEvent<Question> { }

[Serializable]
public class QuestionRequestEvent : UnityEvent<QuestionDifficulty> { }


