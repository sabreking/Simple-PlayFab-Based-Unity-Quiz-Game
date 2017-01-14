using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusPanel : BroadcasterAndReceiver
{

    public Text highScoreText;
    public Text currentScoreText;
    public Text gameStatusText;
    public Text losingStreak;
    public Text winStreak;
    public Text loginStatus;
    public Button loginButton;

    protected override void Start () {
        base.Start();
        loginStatus.text = "Please Log In";
    }

    protected override void SubscribeToEvents()
    {
        loginButton.onClick.AddListener(delegate
        {
            EventSys.onLoginRequested.Invoke();
            loginButton.interactable = false;
        });

        EventSys.onScoreUpdate.AddListener(delegate (int score)
       { currentScoreText.text = "Current Score: " + score.ToString(); });

        EventSys.onHighScoreUpdate.AddListener(delegate (int score)
        { highScoreText.text = "HighScore: " + score.ToString(); });

        EventSys.onLoseStreak.AddListener(delegate (int score)
        { losingStreak.text = "Losing Streak: " + score.ToString(); });

        EventSys.onWinStreak.AddListener(delegate (int score)
        { winStreak.text = "Win Streak: " + score.ToString(); });

        EventSys.onPlayerLoggedIn.AddListener(delegate
        { loginStatus.text = "Player Logged in Successfully"; });

        EventSys.onGameWon.AddListener(delegate
        { gameStatusText.text = "VICTORY: You answered 5 in a row Correctly"; });

        EventSys.onGameLost.AddListener(delegate
        { gameStatusText.text = "YOU LOSE: You answer 3 incorrectly in a row"; });
    }
}
