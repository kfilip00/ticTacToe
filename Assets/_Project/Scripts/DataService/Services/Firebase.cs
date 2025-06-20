using System;
using Authentication;
using UnityEngine;

namespace DataService
{
    public class Firebase : IDataService
    {
        private const string DATABASE_URL = "https://tictactoe-25485-default-rtdb.firebaseio.com";
        private const string PLAYERS_KEY = "players";
        
        private readonly IWebRequests webRequests;
        private IAuthentication authentication;
        
        private string PlayersURL => $"{DATABASE_URL}/{PLAYERS_KEY}";
        private string PlayerURL => $"{PlayersURL}/{authentication.GetPlayerIdentifier()}";
        
        
        public Firebase(IAuthentication _authentication, IWebRequests _webRequests)
        {
            authentication = _authentication;
            webRequests = _webRequests;
        }
        
        public void GetPlayerData(Action<Response> _callback)
        {
            string _url = GetPlayerUrlWithAuthToken();
            webRequests.Get(_url, OnSuccess, OnError);
            return;

            void OnSuccess(string _data)
            {
                Response _response = new Response 
                { 
                    IsSuccessful = true,
                    Message = _data 
                };
                
                _callback?.Invoke(_response);
            }

            void OnError(string _error)
            {
                Response _response = new Response 
                { 
                    IsSuccessful = false,
                    Message = _error 
                };
                
                _callback?.Invoke(_response);
            }
        }

        public void SavePlayerData(string _json, Action<Response> _callback)
        {
            string _url = GetPlayerUrlWithAuthToken();
            webRequests.Patch(_url,_json, OnSuccess,OnError);
            return;
            
            void OnSuccess(string _data)
            {
                Response _response = new Response 
                { 
                    IsSuccessful = true,
                    Message = _data 
                };
                
                _callback?.Invoke(_response);
            }

            void OnError(string _error)
            {
                Response _response = new Response 
                { 
                    IsSuccessful = false,
                    Message = _error 
                };
                
                _callback?.Invoke(_response);
            }
        }

        private string GetPlayerUrlWithAuthToken()
        {
            return $"{PlayerURL}.json?auth={authentication.GetPlayerToken()}";
        }
    }
}