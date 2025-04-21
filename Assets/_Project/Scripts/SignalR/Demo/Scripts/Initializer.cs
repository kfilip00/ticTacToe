using UnityEngine;
using UnitySignalR;

namespace CSignalr.Demo
{
    public class Initializer : MonoBehaviour
    {
        [SerializeField] private bool isTesting;

        private SignalRHandler signalRHandler;
        
        private void Start()
        {
            var _config = new Config { IsTesting = true };
            signalRHandler = new SignalRHandler(_config,new ClientFactory(), new EnvironmentFactory());
            signalRHandler.StartConnection(HandleConnectionResponse);
        }

        private void HandleConnectionResponse(ConnectionResponse _connectionResponse)
        {
            if (_connectionResponse.ConnectionStatus == ConnectionStatus.Failed)
            {
                Debug.LogError(_connectionResponse.Message);
                return;
            }
            
            UIManager.Instance.Setup(signalRHandler);
        }
    }
}