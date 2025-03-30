using System;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using TicTacToe.CSignalR.Data;
using UnityEngine;
using Type = LoggerNS.Type;

namespace TicTacToe.CSignalR
{
    public class SignalRManager : MonoBehaviour
    {
        private string ServerUri => useLocalHost 
            ? "http://localhost:5113/"
            : "https://tictactoe-250326183105.azurewebsites.net/";
        private string GameHub => ServerUri + "hubs/game";

        public static Action<MessageData> OnReceivedMessage;

        public static SignalRManager Instance;

        [SerializeField] private bool useLocalHost;

        private HubConnection connection;
        private string connectionId;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public async void StartConnection(Action<bool> _callBack)
        {
            if (connection is { State: HubConnectionState.Connected })
            {
                _callBack.Invoke(true);
                return;
            }

            connection = new HubConnectionBuilder()
                .WithUrl(GameHub)
                .WithAutomaticReconnect()
                .Build();
            
            connection.On<string>(nameof(ReceiveMessage), ReceiveMessage);

            try
            {
                await connection.StartAsync();
                _callBack?.Invoke(true);
            }
            catch (Exception _error)
            {
                Logger.Log($"Trying to connect with: {GameHub} Failed with error: {_error}", _type: Type.Error);
                _callBack?.Invoke(false);
            }
        }


        public void SendMessage(string _sender, string _message)
        {
            MessageData _data = new MessageData { Username = _sender, Message = _message };
            string _json = JsonConvert.SerializeObject(_data);
            connection.SendAsync("SendMessage", _json);
        }

        private void ReceiveMessage(string _json)
        {
            MessageData _data = JsonConvert.DeserializeObject<MessageData>(_json);
            OnReceivedMessage?.Invoke(_data);
        }
    }
}