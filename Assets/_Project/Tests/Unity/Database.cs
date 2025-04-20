using DataService;
using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEngine;

namespace Test
{
    public class Database : MonoBehaviour
    {
        [SerializeField] private WebRequestHandler webRequestHandler;
        [SerializeField] private Authentication authentication;
        [SerializeField] private string playerName;
        
        private DataHandler dataHandler;

        [Button]
        private void Setup()
        {
            authentication.Setup();
            authentication.SignIn(ContinueSetup);
            return;

            void ContinueSetup(bool _isSuccessful)
            {
                if (!_isSuccessful)
                {
                    Debug.Log("Failed to authenticate");
                    return;
                }

                var _auth = authentication.GetAuthenticationHandler();
                dataHandler = new DataHandler(new DataServiceFactory(), _auth.GetPlayerIdentifier(), _auth.GetPlayerToken(), webRequestHandler);
            }
        }

        [Button]
        private void GetPlayerData()
        {
            dataHandler.GetPlayerData(HandleGetPlayerData);
        }

        private void HandleGetPlayerData(Response _response)
        {
            if (!_response.IsSuccessful)
            {
                Debug.Log("Failed to get player data: " + _response.Message);
                return;
            }

            Debug.Log("Got player data: " + _response.Message);
        }

        [Button]
        private void SetPlayerName()
        {
            string _json = JsonConvert.SerializeObject(new { Name = playerName });
            dataHandler.SavePlayerData(_json, HandleSavePlayerData);
        }

        private void HandleSavePlayerData(Response _response)
        {
            if (!_response.IsSuccessful)
            {
                Debug.Log("Failed to save player data: " + _response.Message);
                return;
            }

            Debug.Log("Saved player data: " + _response.Message);
        }
    }
}