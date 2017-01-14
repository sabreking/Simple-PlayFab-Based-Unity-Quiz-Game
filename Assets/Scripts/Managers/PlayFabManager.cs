using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.Events;

public class PlayFabManager : BroadcasterAndReceiver
{
    public string titleId;
    public static string playFabId;

    protected override void SubscribeToEvents()
    {
        EventSys.onLoginRequested.AddListener(delegate
        {
            Login(titleId);
        });
    }

    private void Login(string titleIdToLoginWith)
    {
        Debug.Log("Attempting Loggin");
        var request = new LoginWithCustomIDRequest {TitleId = titleIdToLoginWith, CreateAccount = true, CustomId = SystemInfo.deviceUniqueIdentifier};

        PlayFabClientAPI.LoginWithCustomID(request, result =>
        {
            playFabId = result.PlayFabId;
            Debug.Log("Got PlayFabID: " + playFabId);

            if (result.NewlyCreated)
            {
                Debug.Log("(new account)");
            } else
            {
                Debug.Log("(existing account)");
            }
            EventSys.onPlayerLoggedIn.Invoke();
        }, error =>
        {

            Debug.Log("Error logging in player with custom ID:");
            Debug.Log(error.ErrorMessage);
            EventSys.onLoginFailed.Invoke();
        });
    }
}