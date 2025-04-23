using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Authentication.Firebase
{
    public class Firebase : IAuthentication
    {
        private const string WEB_API_KEY = "AIzaSyAWWXYVDfI0HEfR-0G0KJERPYck-l5yf9E";
        private const string REFRESH_TOKEN_KEY = "FirebaseRefreshToken";
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

            webRequests.Post(_url, _params, HandleSuccessfulSignIn, HandleUnsuccessfulSignIn);
        }

        private void HandleSuccessfulSignIn(string _data)
        {
            SignInResponse _signInResponse = JsonConvert.DeserializeObject<SignInResponse>(_data);
            Response _response = new Response { IsSuccessful = true, IdToken = _signInResponse.IdToken, Identifier = _signInResponse.LocalId };

            SaveData(_signInResponse.IdToken, int.Parse(_signInResponse.ExpiresIn), _signInResponse.RefreshToken);

            callBack?.Invoke(_response);
        }

        private void HandleUnsuccessfulSignIn(string _data)
        {
            Response _response = new Response { IsSuccessful = false, Message = _data, };

            callBack?.Invoke(_response);
        }

        public void SignUp(string _email, string _password, Action<Response> _callback)
        {
            string _params = "{\"email\":\"" + _email + "\",\"password\":\"" + _password + "\",\"returnSecureToken\":true}";
            string _url = "https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=" + WEB_API_KEY;
            callBack = _callback;

            webRequests.Post(_url, _params, HandleSuccessfulSignUp, HandleUnsuccessfulSignUp);
        }

        private void HandleSuccessfulSignUp(string _data)
        {
            SignUpResponse _signUpResponse = JsonConvert.DeserializeObject<SignUpResponse>(_data);
            Response _response = new Response { IsSuccessful = true, IdToken = _signUpResponse.IdToken, Identifier = _signUpResponse.LocalId };

            SaveData(_signUpResponse.IdToken, int.Parse(_signUpResponse.ExpiresIn), _signUpResponse.RefreshToken);

            callBack?.Invoke(_response);
        }

        private void HandleUnsuccessfulSignUp(string _data)
        {
            Response _response = new Response { IsSuccessful = false, Message = _data, };

            callBack?.Invoke(_response);
        }

        public void TryAutoSignIn(Action<Response> _callback)
        {
            string _refreshToken = PlayerPrefs.GetString(REFRESH_TOKEN_KEY);
            if (string.IsNullOrEmpty(_refreshToken))
            {
                _callback?.Invoke(new Response { IsSuccessful = false, Message = "No refresh token available" });
                return;
            }

            RefreshIdToken(_refreshToken, _callback);
        }

        private void RefreshIdToken(string _refreshToken, Action<Response> _callback)
        {
            callBack = _callback;
            string _url = "https://securetoken.googleapis.com/v1/token?key=" + WEB_API_KEY;
            string _params = "{\"grant_type\":\"refresh_token\",\"refresh_token\":\"" + _refreshToken + "\"}";

            webRequests.Post(_url, _params, HandleSuccessfulRefreshIdToken,HandleUnsuccessfulRefreshIdToken);
        }

        private void HandleSuccessfulRefreshIdToken(string _data)
        {
            RefreshTokenResponse _refreshResponse = JsonConvert.DeserializeObject<RefreshTokenResponse>(_data);
            SaveData(_refreshResponse.IdToken,int.Parse(_refreshResponse.ExpiresIn),_refreshResponse.RefreshToken);

            callBack?.Invoke(new Response
            {
                IsSuccessful = true, IdToken = currentIdToken, Identifier = _refreshResponse.UserId
            });
        }
        
        private void HandleUnsuccessfulRefreshIdToken(string _error)
        {
            callBack?.Invoke(new Response { IsSuccessful = false, Message = _error });
        }
        
        public void SignOut(Action<Response> _callback)
        {
            if (!IsAuthenticated())
            {
                _callback?.Invoke(new Response { IsSuccessful = false, Message = "Not signed in" });
                return;
            }
            SaveData(string.Empty,0,string.Empty);
            _callback?.Invoke(new Response 
            {
                IsSuccessful = true, Message = string.Empty, IdToken = string.Empty, Identifier = string.Empty
            });
        }
        
        private void SaveData(string _idToken, int _expiresIn, string _refreshToken)
        {
            currentIdToken = _idToken;
            tokenExpirationTime = DateTime.UtcNow.AddSeconds(_expiresIn);
            PlayerPrefs.SetString(REFRESH_TOKEN_KEY, _refreshToken);
            PlayerPrefs.Save();
        }
        
        public void SendPasswordResetEmail(string _email, Action<Response> _callback)
        {
            string _params = "{\"requestType\":\"PASSWORD_RESET\",\"email\":\"" + _email + "\"}";
            string _url = "https://identitytoolkit.googleapis.com/v1/accounts:sendOobCode?key=" + WEB_API_KEY;
            callBack = _callback;

            webRequests.Post(_url, _params, HandleSuccessfulPasswordReset, HandleUnsuccessfulPasswordReset);
        }
        
        private void HandleSuccessfulPasswordReset(string _data)
        {
            PasswordResetResponse _resetResponse = JsonConvert.DeserializeObject<PasswordResetResponse>(_data);
            Response _response = new Response
            {
                IsSuccessful = true,
                Message = $"Password reset email sent to {_resetResponse.Email}"
            };

            callBack?.Invoke(_response);
        }

        private void HandleUnsuccessfulPasswordReset(string _error)
        {
            Response _response = new Response
            {
                IsSuccessful = false,
                Message = _error
            };

            callBack?.Invoke(_response);
        }
    }
}