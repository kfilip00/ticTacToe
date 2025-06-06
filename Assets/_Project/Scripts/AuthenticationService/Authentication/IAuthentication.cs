using System;

namespace Authentication
{
    public interface IAuthentication
    {
        bool IsAuthenticated();
        void SignIn(string _email, string _password, Action<Response> _callback);
        void SignUp(string _email, string _password, Action<Response> _callback);
    }
}