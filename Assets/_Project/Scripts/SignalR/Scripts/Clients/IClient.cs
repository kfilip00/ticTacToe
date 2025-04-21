using System;

namespace UnitySignalR
{
    public interface IClient
    {
        public void StartConnection(string _url, Action<ConnectionResponse> _callback);
        public void TalkToServer(string _functionName, string _jsonData);
        public void ReceiveMessageFromServer(string _function, string _jsonData);
        public void ReceiveMessage(string _jsonData);
    }
}