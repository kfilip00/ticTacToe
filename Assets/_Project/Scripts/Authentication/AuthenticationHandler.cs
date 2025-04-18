using System;
using Authentication;

public class AuthenticationHandler
{
    private IAuthentication authentication;
    
    public AuthenticationHandler(IAuthenticatorFactory _authenticatorFactory, IWebRequests _webRequests)
    {
        authentication = _authenticatorFactory.Create(_webRequests);
    }

    public void SignIn(string _email, string _password, Action<Response> _callback)
    {
        authentication.SignIn(_email, _password, _callback);
    }

    public void SignUp(string _email, string _password, Action<Response> _callback)
    {
        authentication.SignUp(_email, _password, _callback);
    }
    
    public bool IsAuthenticated()
    {
        return authentication.IsAuthenticated();
    }
}
