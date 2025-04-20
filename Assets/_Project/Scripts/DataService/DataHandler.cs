using System;

namespace DataService
{
    public class DataHandler
    {
        private IDataService dataService;

        public DataHandler(IDataServiceFactory _factory, string _playerIdentifier, string _playerToken, IWebRequests _webRequests)
        {
            dataService = _factory.Create(_playerIdentifier, _playerToken, _webRequests);
        }

        public void GetPlayerData(Action<Response> _callback)
        {
            dataService.GetPlayerData(_callback);
        }

        public void SavePlayerData(string _json, Action<Response> _callback)
        {
            dataService.SavePlayerData(_json, _callback);
        }
    }
}