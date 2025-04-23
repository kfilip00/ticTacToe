using Authentication;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignInUI  : BasicScreen
{
    [SerializeField] private TMP_InputField email;
    [SerializeField] private TMP_InputField password;
    [SerializeField] private Button signIn;
    [SerializeField] private Button signUp;
    [SerializeField] private Button resetPassword;

    private void OnEnable()
    {
        signIn.onClick.AddListener(SignIn);
        signUp.onClick.AddListener(ShowSignUp);
        resetPassword.onClick.AddListener(ShowResetPassword);
    }

    private void OnDisable()
    {
        signIn.onClick.RemoveListener(SignIn);
        signUp.onClick.RemoveListener(ShowSignUp);
        resetPassword.onClick.RemoveListener(ShowResetPassword);
    }

    private void SignIn()
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
        
        ManageInteractables(false);
        AuthenticateUI.Instance.GetAuthenticationHandler().SignIn(_email,_password,HandleSignInResponse);
    }

    private void HandleSignInResponse(Response _response)
    {
        if (_response.IsSuccessful)
        {
            AuthenticateUI.Instance.FinishAuthentication();
            return;
        }
        
        ManageInteractables(true);
        Debug.Log(_response.Message);
    }

    private void ShowSignUp()
    {
        AuthenticateUI.Instance.ShowSignUp();
    }

    private void ShowResetPassword()
    {
        AuthenticateUI.Instance.ShowResetPassword();
    }

    private void ManageInteractables(bool _status)
    {
        email.interactable = _status;
        password.interactable = _status;
        signIn.interactable = _status;
        signUp.interactable = _status;
        resetPassword.interactable = _status;
    }
}
