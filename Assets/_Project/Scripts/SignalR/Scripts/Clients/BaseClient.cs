using System;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;

namespace UnitySignalR
{
    public class BaseClient : IClient
    {
        public virtual void StartConnection(string _url, Action<ConnectionResponse> _callback)
        {
            throw new NotImplementedException();
        }

        public virtual void TalkToServer(string _functionName, string _jsonData)
        {
            throw new NotImplementedException();
        }

        public void ReceiveMessageFromServer(string _function, string _jsonData)
        {
            Type _thisType = GetType();
            MethodInfo _methodInfo = _thisType.GetMethod(_function, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            if (_methodInfo == null)
            {
                Debug.Log($"No method found for function: {_function}");
                return;
            }

            try
            {
                ParameterInfo[] _parameters = _methodInfo.GetParameters();

                if (_parameters.Length == 0)
                {
                    _methodInfo.Invoke(this, null);
                }
                else if (_parameters.Length == 1)
                {
                    _methodInfo.Invoke(this, new object[] { _jsonData });
                }
                else
                {
                    Debug.Log($"Method {_function} has an unsupported signature.");
                }
            }
            catch (Exception _ex)
            {
                Debug.Log($"Error invoking method {_function}: {_ex.Message}");
            }
        }

        public void ReceiveMessage(string _jsonData)
        {
            MessageData _messageData = JsonConvert.DeserializeObject<MessageData>(_jsonData);
            SignalREvents.HandleReceivedMessage?.Invoke(_messageData);
        }
    }
}