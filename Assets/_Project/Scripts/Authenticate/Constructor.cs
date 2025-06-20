using Authentication;
using UnityEngine;

namespace Authenticate
{
    public class Constructor : MonoBehaviour
    {
        [SerializeField] private ScreenSwitcher screenSwitcher;
        [SerializeField] private SignInUI signInUI;
        [SerializeField] private SignUpUI signUpUI;
        [SerializeField] private PasswordResetUI passwordResetUI;
        [SerializeField] private AutomaticAuthentication automaticAuthentication;
        
        private void Awake()
        {
            IAuthentication _authenticationHandler = ServiceProvider.Instance.Authentication;
            signInUI.Construct(screenSwitcher,_authenticationHandler);
            signUpUI.Construct(screenSwitcher,_authenticationHandler);
            passwordResetUI.Construct(screenSwitcher,_authenticationHandler);
            automaticAuthentication.Construct(_authenticationHandler);
        }

        private void Start()
        {
            automaticAuthentication.TryToAutoSignIn();
        }
    }
}