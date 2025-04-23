using System;
using DataService;
using Zenject;

public class DataHandler
{
    private IDataService dataService;

    [Inject]
    public DataHandler(Config _config,IDataServiceFactory _factory, AuthenticationHandler _authentication, IWebRequests _webRequests)
    {
        dataService = _factory.Create(_config,_authentication, _webRequests);
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