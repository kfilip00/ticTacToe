using System;

namespace Authentication
{
    public interface IAuthentication
    {
        bool IsAuthenticated();
        void SignIn(string _email, string _password, Action<Response> _callback);
        void SignUp(string _email, string _password, Action<Response> _callback);
        void TryAutoSignIn(Action<Response> _callback);
        void SignOut(Action<Response> _callback);
        void SendPasswordResetEmail(string _email, Action<Response> _callback);
    }
}