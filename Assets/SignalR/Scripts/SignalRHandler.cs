using UnityEngine;

namespace UnitySignalR
{
    [RequireComponent(typeof(SignalRSetup))]
    public class SignalRHandler : MonoBehaviour
    {
        protected SignalRSetup SignalRSetup;
        protected SignalRClient Client;

        protected virtual void Setup()
        {
            SignalRSetup = GetComponent<SignalRSetup>();
            Client = Application.isEditor ? gameObject.AddComponent<UnityClient>() : gameObject.AddComponent<WebGlClient>();
            Client.Setup(gameObject.name);
        }
    }
}