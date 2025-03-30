using NaughtyAttributes;
using TicTacToe.CSignalR;
using UnityEngine;

namespace Tests.SignalR
{
    public class SendMessage : MonoBehaviour
    {
        [SerializeField] private string sender;
        [SerializeField] private string message;
        
        [Button]
        private void Send()
        {
            var _signalR = SignalRManager.Instance;
            if (_signalR == null)
            {
                return;
            }
            
            _signalR.SendMessage(sender,message);
        }
    }
   
}