using System;

namespace TicTacToe.CSignalR.Data
{
    [Serializable]
    public class MessageData
    {
        public string Username;
        public string Message;
    }
}