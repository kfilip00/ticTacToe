using System;

namespace TicTacToe.Firebase
{
    [Serializable]
    public class AuthResponse
    {
        public bool IsSuccessful = true;
        public bool IsNewAccount;
    }
}