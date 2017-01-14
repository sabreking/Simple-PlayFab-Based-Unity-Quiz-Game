using UnityEngine.UI;

public class MainMenuPanel : UIWindow
{
    public Button playButton;
    public Button exitButton;

    protected override void Start()
    {
        base.Start();
    }

    protected override void SubscribeToEvents()
    {
        exitButton.onClick.AddListener(ExitGameRequest);
        playButton.onClick.AddListener(delegate { EventSys.onPlayButtonTriggered.Invoke(); });

        EventSys.onPlayerLoggedIn.AddListener(delegate { WindowName = "Login Successful, Downloading questions.... Please wait"; });

        EventSys.onQuestionsDownloaded.AddListener(delegate
        {
            WindowName = "Questions downloaded successfully, Press play to start Answering Questions";
            playButton.interactable = true;
        });
        EventSys.onLoginFailed.AddListener(delegate { WindowName = "Login Failed, please Restart Game"; });
    }

    public void ExitGameRequest()
    {
        EventManager.Instance.onGameExitCall.Invoke();
    }
}