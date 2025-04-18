using System;

namespace Authentication.Firebase
{
    [Serializable]
    public class SignUpResponse
    {
        public string IdToken;
        public string Email;
        public string RefreshToken;
        public string ExpiresIn;
        public string LocalId;
    }
}