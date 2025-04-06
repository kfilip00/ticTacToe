using System;

namespace UnitySignalR
{
    [Serializable]
    public class ConnectionResponse
    {
        public ConnectionStatus ConnectionStatus;
        public string Message;

        public ConnectionResponse(ConnectionStatus _status, string _message)
        {
            ConnectionStatus = _status;
            Message = _message;
        }

        public override string ToString()
        {
            return $"[{ConnectionStatus}] {Message}";
        }
    }   
}
