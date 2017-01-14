using UnityEngine;

public class GameStateManager : BroadcasterAndReceiver
{
    protected override void SubscribeToEvents()
    {
        EventSys.onGameExitCall.AddListener(ExitGame);
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}