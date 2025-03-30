using TicTacToe.CSignalR;
using TicTacToe.CSignalR.Data;
using UnityEngine;

namespace Tests.SignalR
{
    public class ReceiveMessage : MonoBehaviour
    {
        private void OnEnable()
        {
            SignalRManager.OnReceivedMessage += ShowMessage;
        }

        private void OnDisable()
        {
            SignalRManager.OnReceivedMessage -= ShowMessage;
        }

        private void ShowMessage(MessageData _messageData)
        {
            Debug.Log($"{_messageData.Username}: {_messageData.Message}");
        }
    }
}