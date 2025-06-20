using Authentication;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Authenticate
{
    public class PasswordResetUI : BasicScreen
    {
        [SerializeField] private TMP_InputField email;
        [SerializeField] private Button resetPassword;
        [SerializeField] private Button signIn;

        private ScreenSwitcher screenSwitcher;
        private IAuthentication authenticationHandler;

        public void Construct(ScreenSwitcher _screenSwitcher,IAuthentication _authenticationHandler)
        {
            screenSwitcher = _screenSwitcher;
            authenticationHandler = _authenticationHandler;
        }

        private void OnEnable()
        {
            resetPassword.onClick.AddListener(ResetPassword);
            signIn.onClick.AddListener(ShowSignIn);
        }

        private void OnDisable()
        {
            resetPassword.onClick.RemoveListener(ResetPassword);
            signIn.onClick.RemoveListener(ShowSignIn);
        }

        private void ResetPassword()
        {
            string _email = email.text;
            ValidationResult _validationResult = CredentialsValidator.ValidateEmail(_email);
            if (!_validationResult.IsValid)
            {
                Debug.Log(_validationResult.ErrorMessage);
                return;
            }

            ManageInteractables(false);
            authenticationHandler.SendPasswordResetEmail(_email, HandleSendResetPasswordResponse);
        }

        private void HandleSendResetPasswordResponse(Response _response)
        {
            if (_response.IsSuccessful)
            {
                screenSwitcher.LoadMainMenu();
                return;
            }

            ManageInteractables(true);
            Debug.Log(_response.Message);
        }

        private void ShowSignIn()
        {
            screenSwitcher.ShowSignIn();
        }


        private void ManageInteractables(bool _status)
        {
            email.interactable = _status;
            signIn.interactable = _status;
            resetPassword.interactable = _status;
        }
    }
}