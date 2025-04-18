using System;

public interface IWebRequests
{
    void Post(string _uri, string _jsonData, Action<string> _onSuccess, Action<string> _onError);
    void Put(string _uri, string _jsonData, Action<string> _onSuccess, Action<string> _onError);
    void Get(string _uri, Action<string> _onSuccess, Action<string> _onError);
}