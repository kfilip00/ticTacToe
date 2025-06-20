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

        private IAuthentication authentication;

        [Button]
        public void Setup()
        {
            authentication = new AuthenticatorFactory().Create(configuration.GetConfig(), webRequestHandler);
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
            GetCredentials(out string _email,out string _password );
            authentication.SignIn(_email, _password, HandleSignInResponse);
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
            GetCredentials(out string _email,out string _password );
            authentication.SignUp(_email, _password, HandleSignUpResponse);
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

        private void GetCredentials(out string _email,out string _password)
        {
            _email = "unityEditor@tictactoe.com";
            _password = "Kjkszpj123";
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

        [Button]
        private void SendPasswordReset()
        {
            GetCredentials(out string _email,out string _ );
            authentication.SendPasswordResetEmail(_email, HandlePasswordResetResponse);
        }

        private void HandlePasswordResetResponse(Response _response)
        {
            if (!_response.IsSuccessful)
            {
                Debug.Log("Failed to send password reset: "+_response.Message);
                return;
            }
            
            Debug.Log("Sent password reset: "+_response.Message);
        }

        public IAuthentication GetAuthenticationHandler()
        {
            return authentication;
        }
        
        public void SignIn(Action<bool> _callback)
        {
            GetCredentials(out string _email,out string _password );
            authentication.SignIn(_email, _password, CallCallback);
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