using System;
using System.Collections;
using UnityEngine.Networking;

public class WebRequestHandler : Singleton<WebRequestHandler>, IWebRequests
{
    public void Post(string _uri, string _jsonData, Action<string> _onSuccess, Action<string> _onError)
    {
        StartCoroutine(PostRoutine(_uri, _jsonData, _onSuccess, _onError));
    }
    
    private IEnumerator PostRoutine(string _uri,string _jsonData, Action<string> _onSuccess, Action<string> _onError)
    {
        using UnityWebRequest _webRequest = UnityWebRequest.Post(_uri, _jsonData);
        byte[] _jsonToSend = new System.Text.UTF8Encoding().GetBytes(_jsonData);
        _webRequest.uploadHandler = new UploadHandlerRaw(_jsonToSend);
        _webRequest.downloadHandler = new DownloadHandlerBuffer();

        yield return _webRequest.SendWebRequest();

        if (_webRequest.result == UnityWebRequest.Result.Success)
        {
            _onSuccess?.Invoke(_webRequest.downloadHandler.text);
        }
        else
        {
            _onError?.Invoke(_webRequest.downloadHandler.text);
        }

        _webRequest.uploadHandler.Dispose();
        _webRequest.downloadHandler.Dispose();
    }

    public void Put(string _uri, string _jsonData, Action<string> _onSuccess, Action<string> _onError)
    {
        StartCoroutine(PutRoutine(_uri, _jsonData, _onSuccess, _onError));
    }
    
    private IEnumerator PutRoutine(string _uri,string _jsonData, Action<string> _onSuccess, Action<string> _onError)
    {
        using UnityWebRequest _webRequest = UnityWebRequest.Put(_uri, _jsonData);
        byte[] _jsonToSend = new System.Text.UTF8Encoding().GetBytes(_jsonData);
        _webRequest.uploadHandler = new UploadHandlerRaw(_jsonToSend);
        _webRequest.downloadHandler = new DownloadHandlerBuffer();

        yield return _webRequest.SendWebRequest();

        if (_webRequest.result == UnityWebRequest.Result.Success)
        {
            _onSuccess?.Invoke(_webRequest.downloadHandler.text);
        }
        else
        {
            _onError?.Invoke(_webRequest.downloadHandler.text);
        }

        _webRequest.uploadHandler.Dispose();
        _webRequest.downloadHandler.Dispose();
    }

    public void Get(string _uri, Action<string> _onSuccess, Action<string> _onError)
    {
        StartCoroutine(GetRequest(_uri, _onSuccess, _onError));
    }
    
    private IEnumerator GetRequest(string _url, Action<string> _onSuccess, Action<string> _onError)
    {
        using UnityWebRequest _webRequest = UnityWebRequest.Get(_url);
        yield return _webRequest.SendWebRequest();

        if (_webRequest.result == UnityWebRequest.Result.Success)
        {
            _onSuccess?.Invoke(_webRequest.downloadHandler.text);
        }
        else
        {
            _onError?.Invoke(_webRequest.downloadHandler.text);
        }
    }
    
    public void Patch(string _uri, string _jsonData, Action<string> _onSuccess, Action<string> _onError)
    {
        StartCoroutine(PatchRequest(_uri, _jsonData, _onSuccess, _onError));
    }
    
    private IEnumerator PatchRequest(string _uri, string _jsonData, Action<string> _onSuccess, Action<string> _onError)
    {
        using UnityWebRequest _webRequest = new UnityWebRequest(_uri, "PATCH");
        byte[] _jsonToSend = new System.Text.UTF8Encoding().GetBytes(_jsonData);
        _webRequest.uploadHandler = new UploadHandlerRaw(_jsonToSend);
        _webRequest.downloadHandler = new DownloadHandlerBuffer();
        _webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return _webRequest.SendWebRequest();

        if (_webRequest.result == UnityWebRequest.Result.Success)
        {
            _onSuccess?.Invoke(_webRequest.downloadHandler.text);
        }
        else
        {
            _onError?.Invoke(_webRequest.downloadHandler.text);
        }

        _webRequest.uploadHandler.Dispose();
        _webRequest.downloadHandler.Dispose();
    }
}
