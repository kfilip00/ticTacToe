using System;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace UnitySignalR
{
    public class WebGlClient : SignalRClient
    {
        [DllImport("__Internal")]
        private static extern void StartConnectionJS(string _jsonData);

        [DllImport("__Internal")]
        private static extern void TalkToServerJS(string _jsonData);

        private Action<ConnectionResponse> connectionCallback;
        
        public override void StartConnection(string _url, Action<ConnectionResponse> _callback)
        {
            connectionCallback = _callback;
            string _jsonData = JsonConvert.SerializeObject(new { Url = _url, ReceiverObjectName });
            StartConnectionJS(_jsonData);
        }

        public override void TalkToServer(string _functionName, string _jsonData)
        {
            string _json = JsonConvert.SerializeObject(new { FunctionName = _functionName, JsonData = _jsonData });
            TalkToServerJS(_json);
        }

        public void ReceiveConnectionStatusFromJS(string _json)
        {
            ConnectionResponse _response = JsonConvert.DeserializeObject<ConnectionResponse>(_json);
            connectionCallback?.Invoke(_response);
            connectionCallback = null;
        }

        public void ReceiveMessageFromServerJS(string _function, string _jsonData)
        {
            ReceiveMessageFromServer(_function,_jsonData);
        }
    }
}