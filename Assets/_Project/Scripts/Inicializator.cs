using TicTacToe.CSignalR;
using UnityEngine;

public class Inicializator : MonoBehaviour
{
    private void Start()
    {
        Auth();
    }

    private void Auth()
    {
        AuthManager.Instance.Auth(HandleAuth);
    }

    private void HandleAuth(bool _didAuth)
    {
        if (!_didAuth)
        {
            return;
        }

        DataManager.Instance.CollectData(HandleCollectData);
    }

    private void HandleCollectData(bool _didCollectData)
    {
        if (!_didCollectData)
        {
            return;
        }

        DataManager.Instance.SubscribeEvents();
        SignalRManager.Instance.StartConnection(HandleSignalRConnection);
    }

    private void HandleSignalRConnection(bool _status)
    {
        Logger.Log($"SignalR connected");
    }
}