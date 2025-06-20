using Authentication;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Authenticate
{
    public class SignUpUI : BasicScreen
    {
        [SerializeField] private TMP_InputField email;
        [SerializeField] private TMP_InputField password;
        [SerializeField] private TMP_InputField confirmPassword;
        [SerializeField] private Button signUp;
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
            signUp.onClick.AddListener(SignUp);
            signIn.onClick.AddListener(ShowSignIn);
        }

        private void OnDisable()
        {
            signUp.onClick.RemoveListener(SignUp);
            signIn.onClick.RemoveListener(ShowSignIn);
        }

        private void SignUp()
        {
            string _email = email.text;
            string _password = password.text;
            ValidationResult _validationResult = CredentialsValidator.ValidateEmail(_email);
            if (!_validationResult.IsValid)
            {
                Debug.Log(_validationResult.ErrorMessage);
                return;
            }

            _validationResult = CredentialsValidator.ValidatePassword(_password);
            if (!_validationResult.IsValid)
            {
                Debug.Log(_validationResult.ErrorMessage);
                return;
            }

            string _confirmPassword = confirmPassword.text;
            if (_password != _confirmPassword)
            {
                Debug.Log("Passwords don't match");
                return;
            }

            ManageInteractables(false);
            authenticationHandler.SignUp(_email, _password, HandleSignUpResponse);
        }

        private void HandleSignUpResponse(Response _response)
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
            password.interactable = _status;
            confirmPassword.interactable = _status;
            signIn.interactable = _status;
            signUp.interactable = _status;
        }
    }
}