using Authentication;
using UnityEngine;

namespace Authenticate
{
    public class AutomaticAuthentication : MonoBehaviour
    {
        [SerializeField] private ScreenSwitcher screenSwitcher;
        private IAuthentication authenticationHandler;

        public void Construct(IAuthentication _authenticationHandler)
        {
            authenticationHandler = _authenticationHandler; 
        }
        
        public void TryToAutoSignIn()
        {
            if (authenticationHandler.IsAuthenticated())
            {
                screenSwitcher.LoadMainMenu();
                return;
            }
            
            authenticationHandler.TryAutoSignIn(HandleAutoSignIn);
        }

        private void HandleAutoSignIn(Response _result)
        {
            if (_result.IsSuccessful)
            {
                screenSwitcher.LoadMainMenu();
                return;
            }
            
            screenSwitcher.Setup();
        }
    }
}