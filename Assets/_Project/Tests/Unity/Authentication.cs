using Authentication;
using NaughtyAttributes;
using UnityEngine;

namespace Test
{
    public class Authentication : MonoBehaviour
    {
        [SerializeField] private WebRequestHandler webRequestHandler;

        [Button]
        private void SignIn()
        {
            AuthenticationHandler _authentication = new AuthenticationHandler(new AuthenticatorFactory(), webRequestHandler);
            var _credentials = GetCredentials();
            _authentication.SignIn(_credentials.Item1, _credentials.Item2, HandleSignInResponse);
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
            AuthenticationHandler _authentication = new AuthenticationHandler(new AuthenticatorFactory(), webRequestHandler);
            var _credentials = GetCredentials();
            _authentication.SignUp(_credentials.Item1, _credentials.Item2, HandleSignUpResponse);
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
    }
}