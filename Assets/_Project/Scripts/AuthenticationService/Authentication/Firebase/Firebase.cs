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
        private Action<Response> callback;

        private string currentIdToken;
        private string playerIdentifier;
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
            callback = _callback;
            string _params = "{\"email\":\"" + _email + "\",\"password\":\"" + _password + "\",\"returnSecureToken\":true}";
            string _url = "https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=" + WEB_API_KEY;

            webRequests.Post(_url, _params, HandleSuccessfulSignIn, HandleUnsuccessfulSignIn);
        }

        private void HandleSuccessfulSignIn(string _data)
        {
            SignInResponse _signInResponse = JsonConvert.DeserializeObject<SignInResponse>(_data);
            Response _response = new Response { IsSuccessful = true, IdToken = _signInResponse.IdToken, Identifier = _signInResponse.LocalId };
            int _expiresIn = int.Parse(_signInResponse.ExpiresIn);
            SaveData(_response,_expiresIn,_signInResponse.RefreshToken);
        }

        private void HandleUnsuccessfulSignIn(string _data)
        {
            Response _response = new Response { IsSuccessful = false, Message = _data, };
            SaveData(_response);
        }

        public void SignUp(string _email, string _password, Action<Response> _callback)
        {
            callback = _callback;
            string _params = "{\"email\":\"" + _email + "\",\"password\":\"" + _password + "\",\"returnSecureToken\":true}";
            string _url = "https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=" + WEB_API_KEY;

            webRequests.Post(_url, _params, HandleSuccessfulSignUp, HandleUnsuccessfulSignUp);
        }

        private void HandleSuccessfulSignUp(string _data)
        {
            SignUpResponse _signUpResponse = JsonConvert.DeserializeObject<SignUpResponse>(_data);
            Response _response = new Response { IsSuccessful = true, IdToken = _signUpResponse.IdToken, Identifier = _signUpResponse.LocalId };
            int _expiresIn = int.Parse(_signUpResponse.ExpiresIn);
            SaveData(_response,_expiresIn,_signUpResponse.RefreshToken);
        }

        private void HandleUnsuccessfulSignUp(string _data)
        {
            Response _response = new Response { IsSuccessful = false, Message = _data, };
            SaveData(_response);
        }

        public void TryAutoSignIn(Action<Response> _callback)
        {
            callback = _callback;
            string _refreshToken = PlayerPrefs.GetString(REFRESH_TOKEN_KEY);
            Response _response = new Response();
            if (string.IsNullOrEmpty(_refreshToken))
            {
                _response.Message = "No refresh token available";
                SaveData(_response);
                return;
            }

            RefreshIdToken(_refreshToken);
        }

        private void RefreshIdToken(string _refreshToken)
        {
            string _url = "https://securetoken.googleapis.com/v1/token?key=" + WEB_API_KEY;
            string _params = "{\"grant_type\":\"refresh_token\",\"refresh_token\":\"" + _refreshToken + "\"}";

            webRequests.Post(_url, _params, HandleSuccessfulRefreshIdToken,HandleUnsuccessfulRefreshIdToken);
        }

        private void HandleSuccessfulRefreshIdToken(string _data)
        {
            RefreshTokenResponse _refreshResponse = JsonConvert.DeserializeObject<RefreshTokenResponse>(_data);
            Response _response = new Response
            {
                IdToken = _refreshResponse.IdToken,
                IsSuccessful = true,
                Identifier = _refreshResponse.UserId
            };

            int _expiresIn = int.Parse(_refreshResponse.ExpiresIn); 
            SaveData(_response,_expiresIn,_refreshResponse.ExpiresIn);
        }
        
        private void HandleUnsuccessfulRefreshIdToken(string _error)
        {
            Response _response = new Response { IsSuccessful = false, Message = _error };
            SaveData(_response);
        }

        public void SignOut(Action<Response> _callback)
        {
            callback = _callback;
            Response _response = new Response();
            if (!IsAuthenticated())
            {
                _response.Message = "Not signed in";
            }
            else
            {
                _response.IsSuccessful = true;
            }

            SaveData(_response,0,string.Empty);
        }

        private void SaveData(Response _response, int _tokenExpiration = 0, string _refreshToken= "")
        {
            if (!_response.IsSuccessful)
            {
                callback?.Invoke(_response);
                return;
            }
            
            currentIdToken = _response.IdToken;
            tokenExpirationTime = DateTime.UtcNow.AddSeconds(_tokenExpiration);
            playerIdentifier = _response.Identifier;
            PlayerPrefs.SetString(REFRESH_TOKEN_KEY, _refreshToken);
            PlayerPrefs.Save();
            callback?.Invoke(_response);
        }
        
        public void SendPasswordResetEmail(string _email, Action<Response> _callback)
        {
            callback = _callback;
            string _params = "{\"requestType\":\"PASSWORD_RESET\",\"email\":\"" + _email + "\"}";
            string _url = "https://identitytoolkit.googleapis.com/v1/accounts:sendOobCode?key=" + WEB_API_KEY;

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

            callback?.Invoke(_response);
        }

        private void HandleUnsuccessfulPasswordReset(string _error)
        {
            Response _response = new Response
            {
                IsSuccessful = false,
                Message = _error
            };

            callback?.Invoke(_response);
        }

        public string GetPlayerIdentifier()
        {
            return playerIdentifier;
        }

        public string GetPlayerToken()
        {
            return currentIdToken;
        }
    }
}