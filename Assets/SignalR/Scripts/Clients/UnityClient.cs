using System;
using Microsoft.AspNetCore.SignalR.Client;

namespace UnitySignalR
{
    public class UnityClient : SignalRClient
    {
        private HubConnection connection;
        private string connectionId;

        public override async void StartConnection(string _gameHubUrl, Action<ConnectionResponse> _callBack)
        {
            if (connection is { State: HubConnectionState.Connected })
            {
                _callBack.Invoke(new ConnectionResponse(ConnectionStatus.AlreadyConnected,"Already connected"));
                return;
            }

            connection = new HubConnectionBuilder().WithUrl(_gameHubUrl).WithAutomaticReconnect().Build();

            connection.On<string,string>(nameof(ReceiveMessageFromServer), ReceiveMessageFromServer);

            try
            {
                await connection.StartAsync();
                _callBack?.Invoke(new ConnectionResponse(ConnectionStatus.Successful, "Successfully connected"));
            }
            catch (Exception _error)
            {
                _callBack?.Invoke(new ConnectionResponse(ConnectionStatus.Failed, _error.ToString()));
            }
        }

        public override void TalkToServer(string _function, string _jsonData)
        {
            connection.SendAsync(_function, _jsonData);
        }
    }
}