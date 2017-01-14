using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class ScoreManager : BroadcasterAndReceiver
{
    [SerializeField] private int _losingStreak;
    [SerializeField] private int _winningStreak;
    [SerializeField] private int _score;

    private int _highScore;

    protected override void Start()
    {
        base.Start();
        FireGuiUpdateEvents();
    }

    protected override void SubscribeToEvents()
    {
        EventSys.onQuestionAnsweredCorrectly.AddListener(CorrecttlyAnswered);
        EventSys.onQuestionAnsweredInCorrectlyOrTimesUp.AddListener(WrongAnswerOrTimeOut);

        EventSys.onPlayerLoggedIn.AddListener(GetUserData);
        EventSys.onGameWon.AddListener(SetOrUpdateUserData);
        EventSys.onGameLost.AddListener(SetOrUpdateUserData);
    }

    private void SetOrUpdateUserData()
    {
        var request = new UpdateUserDataRequest {Data = new Dictionary<string, string> {{"HighScore", _highScore.ToString()}}};

        PlayFabClientAPI.UpdateUserData(request, result => { Debug.Log("Successfully updated HighScore Data "); }, error =>
        {
            Debug.Log("Got error setting HighScore Data data for HighScore");
            Debug.Log(error.ErrorDetails);
        });
    }

    private void GetUserData()
    {
        var request = new GetUserDataRequest {PlayFabId = PlayFabManager.playFabId, Keys = null};

        PlayFabClientAPI.GetUserData(request, result =>
        {
            Debug.Log("Got HighScore Data data:");
            if ((result.Data == null) || (result.Data.Count == 0))
            {
                Debug.Log("No HighScore Data data available");
                SetOrUpdateUserData();
            } else
            {
                _highScore = int.Parse(result.Data["HighScore"].Value);
                Debug.Log(_highScore);
                FireGuiUpdateEvents();
            }
        }, error =>
        {
            Debug.Log("Got error retrieving HighScore Data");
            Debug.Log(error.ErrorMessage);
        });
    }

    private void CorrecttlyAnswered(Question question)
    {
        _score += (int) question.ScoreValue;
        HighScoreCheck();
        _winningStreak += 1;
        _losingStreak = 0;
        WinLoseConditionCheck();
        FireGuiUpdateEvents();
    }

    private void WrongAnswerOrTimeOut()
    {
        _winningStreak = 0;
        _losingStreak += 1;
        WinLoseConditionCheck();
        FireGuiUpdateEvents();
    }

    private void HighScoreCheck()
    {
        if (_score > _highScore)
        {
            _highScore = _score;
        }
    }

    private void FireGuiUpdateEvents()
    {
        EventSys.onScoreUpdate.Invoke(_score);
        EventSys.onWinStreak.Invoke(_winningStreak);
        EventSys.onLoseStreak.Invoke(_losingStreak);
        EventSys.onHighScoreUpdate.Invoke(_highScore);
    }

    public void WinLoseConditionCheck()
    {
        if (_winningStreak >= 5)
        {
            EventSys.onGameWon.Invoke();
        } else if (_losingStreak >= 3)
        {
            EventSys.onGameLost.Invoke();
        }
    }
}