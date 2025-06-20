using UnityEngine;

namespace Authenticate
{
    public class ScreenSwitcher : BasicScreenHandler
    {
        [SerializeField] private SignInUI signInUI;
        [SerializeField] private SignUpUI signUpUI;
        [SerializeField] private PasswordResetUI passwordResetUI;
    
        private void OnDisable()
        {
            StopAllCoroutines();
        }
    
        public void Setup()
        {
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
    
        public void LoadMainMenu()
        {
            SceneManager.Instance.LoadScene(SceneManager.MAIN_MENU);
        }
    }
}
