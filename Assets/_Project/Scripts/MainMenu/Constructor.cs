using Matchmaking;
using UnityEngine;

namespace MainMenu
{
    public class Constructor : MonoBehaviour
    {
        [SerializeField] private MainMenuUI mainMenu;
        [SerializeField] private MatchMakingUI matchMakingUI;
        [SerializeField] private ScreenSwitcher screenSwitcher;
        [SerializeField] private MatchMakingHandler matchMakingHandler;
        
        private void Awake()
        {
            mainMenu.Construct(screenSwitcher, ServiceProvider.Instance.Config);
            matchMakingUI.Construct(screenSwitcher,matchMakingHandler);
            screenSwitcher.ShowMainMenu();
        }
    }
}