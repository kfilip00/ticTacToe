using System;

namespace Authentication
{
    [Serializable]
    public class Response
    {
        public bool IsSuccessful;
        public string Message;
        public string IdToken;
        public string Identifier;
    }
}