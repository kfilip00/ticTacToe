using System.Collections;
using Authentication;
using UnityEngine;

public class AuthenticateUI : MonoBehaviour
{
    public static AuthenticateUI Instance;
    
    [SerializeField] private SignInUI signInUI;
    [SerializeField] private SignUpUI signUpUI;
    [SerializeField] private PasswordResetUI passwordResetUI;
    
    private AuthenticationHandler authenticationHandler;
    private IScreen currentScreen;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Start()
    {
        authenticationHandler = ServiceProvider.Instance.Authentication;
        if (authenticationHandler.IsAuthenticated())
        {
            FinishAuthentication();
            return;
        }
        
        authenticationHandler.TryAutoSignIn(HandleAutoSignIn);
    }

    private void HandleAutoSignIn(Response _response)
    {
        if (_response.IsSuccessful)
        {
            FinishAuthentication();
            return;
        }
        
        ShowSignIn();
    }

    public void ShowSignIn()
    {
        ShowScreen(signInUI);
    }
    
    public void ShowSignUp()
    {
        ShowScreen(signUpUI);
    }

    public void ShowResetPassword()
    {
        ShowScreen(passwordResetUI);
    }

    private void ShowScreen(IScreen _screen)
    {
        if (_screen == currentScreen)
        {
            return;
        }

        StartCoroutine(ShowScreenRoutine());
        IEnumerator ShowScreenRoutine()
        {
            bool _continue;
            if (currentScreen != null)
            {
                _continue = false;
                currentScreen.Close(AllowContinue);
                yield return new WaitUntil(() => _continue);
            }

            _continue = false;
            _screen.Show(AllowContinue);
            yield return new WaitUntil(() => _continue);
            currentScreen = _screen;

            void AllowContinue()
            {
                _continue = true;
            }
        }
    }

    public AuthenticationHandler GetAuthenticationHandler()
    {
        return authenticationHandler;
    }

    public void FinishAuthentication()
    {
        SceneManager.Instance.LoadScene(SceneManager.MAIN_MENU);
    }
}
