using System;
using Newtonsoft.Json;

namespace UnitySignalR
{
    public class SignalRHandler : IServerFunctions
    {
        private IClient client;
        private IEnvironment environment;

        public SignalRHandler(Config _config,IClientFactory _clientFactory, IEnvironmentFactory _environmentFactory)
        {
            client = _clientFactory.CreateClient();
            environment = _environmentFactory.Create(_config.IsTesting);
        }

        public void StartConnection(Action<ConnectionResponse> _callBack)
        {
            client.StartConnection(environment.GetGameHub(),_callBack);
        }

        public void SendMessage(string _sender, string _message)
        {
            MessageData _messageData = new MessageData { Sender = _sender, Message = _message };
            string _data = JsonConvert.SerializeObject(_messageData);
            client.TalkToServer("SendMessage", _data);
        }
    }
}