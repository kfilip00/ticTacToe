using UnityEngine;

namespace MainMenu
{
    public class ScreenSwitcher : BasicScreenHandler
    {
        [SerializeField] private MainMenuUI mainMenu;
        [SerializeField] private MatchMakingUI matchMaking;
        private IScreen currentScreen;

        public void ShowMainMenu()
        {
            ShowScreen(mainMenu);
        }

        public void ShowMatchMaking()
        {
            ShowScreen(matchMaking);
        }

        public void ShowGameplay()
        {
            SceneManager.Instance.LoadScene(SceneManager.GAMEPLAY);
        }
    }
}
