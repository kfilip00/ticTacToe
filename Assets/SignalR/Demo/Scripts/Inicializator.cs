using UnityEngine;
using UnitySignalR;

namespace CSignalr.Demo
{
    public class Inicializator : MonoBehaviour
    {
        private void Start()
        {
            SignalRManager.Instance.StartConnection(HandleConnectionResponse);
        }

        private void HandleConnectionResponse(ConnectionResponse _connectionResponse)
        {
            Debug.Log(_connectionResponse);
        }
    }
}
