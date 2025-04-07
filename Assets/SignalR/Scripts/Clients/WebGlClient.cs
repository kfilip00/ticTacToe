using System;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace UnitySignalR
{
    public class WebGlClient : UnityClient
    {
        [DllImport("__Internal")]
        private static extern void StartConnectionJS(string _jsonData);

        [DllImport("__Internal")]
        private static extern void TalkToServerJS(string _jsonData);

        private Action<ConnectionResponse> connectionCallback;
        
        public override void StartConnection(string _url, Action<ConnectionResponse> _callback)
        {
            connectionCallback = _callback;
            string _jsonData = JsonConvert.SerializeObject(new { Url = _url });
            StartConnectionJS(_jsonData);
        }

        public override void TalkToServer(string _functionName, string _jsonData)
        {
            string _json = JsonConvert.SerializeObject(new { FunctionName = _functionName, JsonData = _jsonData });
            TalkToServerJS(_json);
        }
    }
}