using System;

namespace TicTacToe.Firebase
{
    [Serializable]
    public class RegisterResponse
    {
        public string IdToken;
        public string Email;
        public string RefreshToken;
        public string ExpiresIn;
        public string LocalId;
    }
}