using System;
using UnityEngine;
using UnityEngine.UI;

public class QuestionWindow : UIWindow
{
    private Animator _animator;

    public Text questionText;
    public AnswerButtons[] answerButtons;
    public Slider timeSlider;
    public Text timeText;
    public Text questionDifficultyDisplayText;
    [Header("Will show up in runtime- for debugging")]
    [SerializeField] private Question _currentQuestion;
    [SerializeField] private float _timeLeft;
    private bool _isRunningTimer;
    private bool _isTimeUp;

    protected override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
        timeSlider.maxValue = GlobalSettings.Instance.timeAllowancePerQuestion;
        ResetTimer();
    }

    private void Update()
    {
        Timer();
    }

    protected override void SubscribeToEvents()
    {
        EventSys.onNextQuestionForQuestionWindow.AddListener(PopulateAndShowQuestion);
    }

    public override void Open()
    {
        _animator.SetTrigger("MoveIn");
    }

    public override void Close()
    {
        _animator.SetTrigger("MoveOut");
        EventSys.onQuestionWindowIdle.Invoke();
    }

    private void PopulateAndShowQuestion(Question question)
    {

        _currentQuestion = question;
        PopulateWindow(question);
        Open();
        ResetTimer();
        StartTimer();
    }

    private void CorrectAnswerGiven()
    {
        ResetTimer();
        EventSys.onQuestionAnsweredCorrectly.Invoke(_currentQuestion);
        Close();
    }

    private void TimeIsUpOrWrongAnswer()
    {
        ResetTimer();
        EventSys.onQuestionAnsweredInCorrectlyOrTimesUp.Invoke();
        Close();
    }

    private void Timer()
    {
        if (_isRunningTimer)
        {
            _timeLeft -= Time.deltaTime;
            timeSlider.value = _timeLeft;
            if (_timeLeft <= 0.0f)
            {
                TimeIsUpOrWrongAnswer();
            }
        }
    }

    private void StartTimer()
    {
        _isRunningTimer = true;
    }

    private void ResetTimer()
    {
        _isRunningTimer = false;
        _timeLeft = GlobalSettings.Instance.timeAllowancePerQuestion;
        timeSlider.value = _timeLeft;
    }

    private void PopulateWindow(Question question)
    {
        questionText.text = question.questionString;
        questionDifficultyDisplayText.text = question.questionDifficulty.ToString();
        //populating answer buttons
        for (var i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].answerText.text = question.answerStrings[i];
            if (i == question.correctIndex)
            {
                answerButtons[i].button.onClick.RemoveAllListeners();
                answerButtons[i].button.onClick.AddListener(CorrectAnswerGiven);
            } else
            {
                answerButtons[i].button.onClick.RemoveAllListeners();
                answerButtons[i].button.onClick.AddListener(TimeIsUpOrWrongAnswer);
            }
        }
    }
}

[Serializable]
public struct AnswerButtons
{
    public Button button;
    public Text answerText;
}