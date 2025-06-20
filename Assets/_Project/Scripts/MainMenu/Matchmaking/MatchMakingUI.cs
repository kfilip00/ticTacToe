using Matchmaking;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class MatchMakingUI : BasicScreen
    {
        [SerializeField] private Button cancel;
        private ScreenSwitcher screenSwitcher;
        private MatchMakingHandler matchMakingHandler;

        public void Construct(ScreenSwitcher _screenSwitcher, MatchMakingHandler _matchMakingHandler)
        {
            screenSwitcher = _screenSwitcher;
            matchMakingHandler = _matchMakingHandler;
        }

        private void OnEnable()
        {
            cancel.onClick.AddListener(Cancel);        
        }

        private void OnDisable()
        {
            cancel.onClick.RemoveListener(Cancel);        
        }

        private void Cancel()
        {
            
        }
    }   
}