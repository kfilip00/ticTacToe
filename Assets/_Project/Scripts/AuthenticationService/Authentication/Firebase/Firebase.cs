using System;
using Newtonsoft.Json;

namespace Authentication.Firebase
{
    public class Firebase : IAuthentication
    {
        private const string WEB_API_KEY = "AIzaSyAWWXYVDfI0HEfR-0G0KJERPYck-l5yf9E";
        private IWebRequests webRequests;
        private Action<Response> callBack;
        
        private string currentIdToken;
        private DateTime tokenExpirationTime;
        
        public Firebase(IWebRequests _webRequests)
        {
            webRequests = _webRequests;
        }
        
        public bool IsAuthenticated()
        {
            if (string.IsNullOrEmpty(currentIdToken))
            {
                return false;
            }

            return DateTime.UtcNow < tokenExpirationTime;
        }

        public void SignIn(string _email, string _password, Action<Response> _callback)
        {
            string _params = "{\"email\":\"" + _email + "\",\"password\":\"" + _password + "\",\"returnSecureToken\":true}";
            string _url = "https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=" + WEB_API_KEY;
            callBack = _callback;
            
            webRequests.Post(_url, _params,HandleSuccessfulSignIn, HandleUnsuccessfulSignIn);
        }

        private void HandleSuccessfulSignIn(string _data)
        {
            SignInResponse _signInResponse = JsonConvert.DeserializeObject<SignInResponse>(_data);
            Response _response = new Response
            {
                IsSuccessful = true,
                IdToken = _signInResponse.IdToken,
                Identifier = _signInResponse.LocalId
            };
            
            currentIdToken = _signInResponse.IdToken;
            tokenExpirationTime = DateTime.UtcNow.AddSeconds(int.Parse(_signInResponse.ExpiresIn));
            
            callBack?.Invoke(_response);
        }
        
        private void HandleUnsuccessfulSignIn(string _data)
        {
            Response _response = new Response
            {
                IsSuccessful = false,
                Message = _data,
            };
            
            callBack?.Invoke(_response);
        }
        
        public void SignUp(string _email, string _password, Action<Response> _callback)
        {
            string _params = "{\"email\":\"" + _email + "\",\"password\":\"" + _password + "\",\"returnSecureToken\":true}";
            string _url = "https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=" + WEB_API_KEY;
            callBack = _callback;
            
            webRequests.Post(_url,_params,HandleSuccessfulSignUp, HandleUnsuccessfulSignUp);
        }

        private void HandleSuccessfulSignUp(string _data)
        {
            SignUpResponse _signUpResponse = JsonConvert.DeserializeObject<SignUpResponse>(_data);
            Response _response = new Response
            {
                IsSuccessful = true,
                IdToken = _signUpResponse.IdToken,
                Identifier = _signUpResponse.LocalId
            };
            
            currentIdToken = _signUpResponse.IdToken;
            tokenExpirationTime = DateTime.UtcNow.AddSeconds(int.Parse(_signUpResponse.ExpiresIn));
            
            callBack?.Invoke(_response);
        }
        
        private void HandleUnsuccessfulSignUp(string _data)
        {
            Response _response = new Response
            {
                IsSuccessful = false,
                Message = _data,
            };
            
            callBack?.Invoke(_response);
        }
    }
}