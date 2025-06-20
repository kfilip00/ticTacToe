using System;
using Authentication;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class MainMenuUI : BasicScreen
    {
        [SerializeField] private Button multiPlayer;
        [SerializeField] private Button singlePlayer;
        [SerializeField] private Button local;
        [SerializeField] private Button signOut;

        private ScreenSwitcher screenSwitcher;
        private Config config;

        public void Construct(ScreenSwitcher _screenSwitcher, Config _config)
        {
            config = _config;
            screenSwitcher = _screenSwitcher;
        }

        private void OnEnable()
        {
            multiPlayer.onClick.AddListener(MultiPlayer);
            singlePlayer.onClick.AddListener(SinglePlayer);
            local.onClick.AddListener(LocalGame);
            signOut.onClick.AddListener(SignOut);
        }

        private void OnDisable()
        {
            multiPlayer.onClick.RemoveListener(MultiPlayer);
            singlePlayer.onClick.RemoveListener(SinglePlayer);
            local.onClick.RemoveListener(LocalGame);
            signOut.onClick.RemoveListener(SignOut);
        }

        private void MultiPlayer()
        {
            StartGameplay(GameplayType.Multiplayer);
        }

        private void SinglePlayer()
        {
            StartGameplay(GameplayType.SinglePlayer);
        }

        private void LocalGame()
        {
            StartGameplay(GameplayType.Local);
        }

        private void StartGameplay(GameplayType _type)
        {
            config.Gameplay = _type;
            switch (_type)
            {
                case GameplayType.Multiplayer:
                    screenSwitcher.ShowMatchMaking();
                    break;
                case GameplayType.SinglePlayer:
                case GameplayType.Local:
                    screenSwitcher.ShowGameplay();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_type), _type, null);
            }
        }

        private void SignOut()
        {
            ServiceProvider.Instance.Authentication.SignOut(HandleSignOutResponse);
        }

        private void HandleSignOutResponse(Response _response)
        {
            if (_response.IsSuccessful)
            {
                SceneManager.Instance.LoadScene(SceneManager.AUTHENTICATE);
                return;
            }

            Debug.Log("Failed to sign out: " + _response.Message);
        }
    }
}