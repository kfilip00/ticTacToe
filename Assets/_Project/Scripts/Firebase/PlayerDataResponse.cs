using System;

namespace TicTacToe.Firebase
{
    [Serializable]
    public class PlayerDataResponse
    {
        public bool IsSuccessful;
        public string Json;
    }
}