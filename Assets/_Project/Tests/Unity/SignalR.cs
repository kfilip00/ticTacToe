using NaughtyAttributes;
using UnityEngine;
using UnitySignalR;

namespace Test
{
    public class SignalR : MonoBehaviour
    {
        [SerializeField] private Configuration configuration;
        [SerializeField] private string sender;
        [SerializeField] private string message;
        private SignalRHandler signalRHandler;
        
        [Button]
        private void Setup()
        {
            if (signalRHandler!=null)
            {
                SignalREvents.HandleReceivedMessage -= ShowMessage;
            }
            
            signalRHandler = new SignalRHandler(configuration.GetConfig(),new ClientFactory(), new EnvironmentFactory());
            SignalREvents.HandleReceivedMessage += ShowMessage;
        }

        [Button]
        private void Connect()
        {
            signalRHandler.StartConnection(HandleConnectionResponse);
        }

        private void HandleConnectionResponse(ConnectionResponse _response)
        {
            if (_response.ConnectionStatus == ConnectionStatus.Failed)
            {
                Debug.Log("Failed to connect");
                return;
            }
            
            Debug.Log("Connected signalr");
        }

        [Button]
        private void SendMessage()
        {
            signalRHandler.SendMessage(sender,message);
        }
        
        private void ShowMessage(MessageData _messageData)
        {
            Debug.Log($"Received {_messageData.Sender}: {_messageData.Sender}");
        }
    }
}