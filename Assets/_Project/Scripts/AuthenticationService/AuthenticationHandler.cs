using System;
using Authentication;
using Zenject;

public class AuthenticationHandler
{
    private IAuthentication authentication;
    private Action<Response> callback;

    private string playerIdentifier;
    private string playerToken;
    
    [Inject]
    public AuthenticationHandler(Config _config,IAuthenticatorFactory _authenticatorFactory, IWebRequests _webRequests)
    {
        authentication = _authenticatorFactory.Create(_config,_webRequests);
    }

    public void SignIn(string _email, string _password, Action<Response> _callback)
    {
        callback = _callback;
        authentication.SignIn(_email, _password, TryToSavePlayerData);
    }

    public void SignUp(string _email, string _password, Action<Response> _callback)
    {
        callback = _callback;
        authentication.SignUp(_email, _password, TryToSavePlayerData);
    }

    public void TryAutoSignIn(Action<Response> _callback)
    {
        callback = _callback;
        authentication.TryAutoSignIn(TryToSavePlayerData);
    }

    private void TryToSavePlayerData(Response _response)
    {
        if (!_response.IsSuccessful)
        {
            callback?.Invoke(_response);
            return;
        }

        playerIdentifier = _response.Identifier;
        playerToken = _response.IdToken;
        callback?.Invoke(_response);
    }
    
    public bool IsAuthenticated()
    {
        return authentication.IsAuthenticated();
    }
    
    public string GetPlayerIdentifier()
    {
        return playerIdentifier;
    }

    public string GetPlayerToken()
    {
        return playerToken;
    }
    
}