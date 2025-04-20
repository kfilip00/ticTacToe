using System;
using Authentication;
using NaughtyAttributes;
using UnityEngine;

namespace Test
{
    public class Authentication : MonoBehaviour
    {
        [SerializeField] private WebRequestHandler webRequestHandler;

        private AuthenticationHandler authentication;

        [Button]
        public void Setup()
        {
            authentication = new AuthenticationHandler(new AuthenticatorFactory(), webRequestHandler);
        }

        [Button]
        private void SignIn()
        {
            var _credentials = GetCredentials();
            authentication.SignIn(_credentials.Item1, _credentials.Item2, HandleSignInResponse);
        }

        public void SignIn(Action<bool> _callback)
        {
            var _credentials = GetCredentials();
            authentication.SignIn(_credentials.Item1, _credentials.Item2, CallCallback);
            return;

            void CallCallback(Response _response)
            {
                if (!_response.IsSuccessful)
                {
                    _callback?.Invoke(false);
                    HandleSignInResponse(_response);
                    return;
                }
                
                _callback?.Invoke(true);
                HandleSignInResponse(_response);
            }
        }

        private void HandleSignInResponse(Response _response)
        {
            if (_response.IsSuccessful)
            {
                Debug.Log("Successfully signed in");
                return;
            }

            Debug.Log("Failed to sign in: " + _response.Message);
        }

        [Button]
        private void SignUp()
        {
            var _credentials = GetCredentials();
            authentication.SignUp(_credentials.Item1, _credentials.Item2, HandleSignUpResponse);
        }

        private void HandleSignUpResponse(Response _response)
        {
            if (_response.IsSuccessful)
            {
                Debug.Log("Successfully signed up");
                return;
            }

            Debug.Log("Failed to sign up: " + _response.Message);
        }

        private (string, string) GetCredentials()
        {
            return ("unityEditor@tictactoe.com", "Kjkszpj123");
        }

        [Button]
        private void IsAuthenticated()
        {
            Debug.Log("Is authenticated: "+ authentication.IsAuthenticated());
        }

        public AuthenticationHandler GetAuthenticationHandler()
        {
            return authentication;
        }
    }
}