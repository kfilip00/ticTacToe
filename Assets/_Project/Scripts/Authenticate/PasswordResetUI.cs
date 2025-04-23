using Authentication;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PasswordResetUI : BasicScreen
{
    [SerializeField] private TMP_InputField email;
    [SerializeField] private Button resetPassword;
    [SerializeField] private Button signIn;
    
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
        AuthenticateUI.Instance.GetAuthenticationHandler().SendPasswordReset(_email,HandleSendResetPasswordResponse);
    }

    private void HandleSendResetPasswordResponse(Response _response)
    {
        if (_response.IsSuccessful)
        {
            AuthenticateUI.Instance.FinishAuthentication();
            return;
        }
        
        ManageInteractables(true);
        Debug.Log(_response.Message);
    }

    private void ShowSignIn()
    {
        AuthenticateUI.Instance.ShowSignIn();
    }


    private void ManageInteractables(bool _status)
    {
        email.interactable = _status;
        signIn.interactable = _status;
        resetPassword.interactable = _status;
    }
}