using System;
using UnityEngine;

namespace UnitySignalR
{
    public abstract class SignalRClient : MonoBehaviour
    {
        protected string ReceiverObjectName;
        
        public abstract void StartConnection(string _url, Action<ConnectionResponse> _callback);
        public abstract void TalkToServer(string _functionName, string _jsonData);

        public void Setup(string _receiverObject)
        {
            ReceiverObjectName = _receiverObject;
        }
        
        public void ReceiveMessageFromServer(string _function, string _jsonData)
        {
            
        }
    }
}