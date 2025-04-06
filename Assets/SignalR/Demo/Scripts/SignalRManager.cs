using System;
using UnitySignalR;

namespace CSignalr.Demo
{
    public class SignalRManager : SignalRHandler
    {
        public static SignalRManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                Setup();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void StartConnection(Action<ConnectionResponse> _callBack)
        {
            Client.StartConnection(SignalRSetup.GameHub,_callBack);
        }

        private void TalkToServer(string _function, string _jsonData)
        {
            Client.TalkToServer(_function,_jsonData);
        }
    }
}