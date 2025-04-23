using System;
using Authentication;
using NaughtyAttributes;
using UnityEngine;

namespace Test
{
    public class Authentication : MonoBehaviour
    {
        [SerializeField] private Configuration configuration;
        [SerializeField] private WebRequestHandler webRequestHandler;

        private AuthenticationHandler authentication;

        [Button]
        public void Setup()
        {
            authentication = new AuthenticationHandler(configuration.GetConfig(),new AuthenticatorFactory(), webRequestHandler);
        }
        
        [Button]
        private void AutomaticSignIn()
        {
            authentication.TryAutoSignIn(HandleAutomaticSignInResponse);
        }

        private void HandleAutomaticSignInResponse(Response _response)
        {
            if (!_response.IsSuccessful)
            {
                Debug.Log("Failed to automatically sign in: "+_response.Message);
                return;
            }
            
            Debug.Log("Successfully automatically signed in");
        }

        [Button]
        private void SignIn()
        {
            var _credentials = GetCredentials();
            authentication.SignIn(_credentials.Item1, _credentials.Item2, HandleSignInResponse);
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

        [Button]
        private void SignOut()
        {
            authentication.SignOut(HandleSignOutResponse);
        }

        private void HandleSignOutResponse(Response _response)
        {
            if (!_response.IsSuccessful)
            {
                Debug.Log("Failed to sign out: "+_response.Message);
                return;
            }
            
            Debug.Log("Successfully signed out");
        }

        public AuthenticationHandler GetAuthenticationHandler()
        {
            return authentication;
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
        
        public void SignInAuto(Action<bool> _callback)
        {
            authentication.TryAutoSignIn(CallCallback);
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
    }
}