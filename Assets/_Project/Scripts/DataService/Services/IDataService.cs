using System;

namespace DataService
{
    public interface IDataService
    {
        void GetPlayerData(Action<Response> _callback);
        void SavePlayerData(string _json, Action<Response> _callback);
    }
}
