using System;
using System.Collections;
using TicTacToe.Firebase;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Type = LoggerNS.Type;

public class AuthManager : MonoBehaviour
{
    public static AuthManager Instance;
    private Action<bool> authCallBack;

    private void Awake()
    {
        if (Instance==null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Auth(Action<bool> _callBack)
    {
        authCallBack = _callBack;
        if (FirebaseManager.Instance.IsAuthenticated)
        {
            authCallBack?.Invoke(true);
            return;
        }

        string _email = $"unityEditor@{FirebaseManager.EMAIL_DOMAIN}";
        string _password = "unityEditor123";
        
        FirebaseManager.Instance.Authenticate(_email,_password, HandleAuthResponse);
    }

    private void HandleAuthResponse(AuthResponse _response)
    {
        if (!_response.IsSuccessful)
        {
            authCallBack?.Invoke(false);
            return;
        }
        
        if (_response.IsNewAccount)
        {
            DataManager.Instance.SetupNewPlayer();
            string _json = JsonConvert.SerializeObject(DataManager.Instance.PlayerData);
            FirebaseManager.Instance.SavePlayerData(_json, () =>
            {
                authCallBack?.Invoke(true);
            });
            return;
        }
        
        authCallBack?.Invoke(true);
    }

    // public void GetSignalRToken(string _token,string _url, Action<SignalRTokenResponse> _callBack)
    // {
    //     string _jsonData = JsonConvert.SerializeObject(new { userToken = _token });
    //     Debug.Log(_jsonData);
    //     Debug.Log(_url);
    //     StartCoroutine(Post(_url, _jsonData, _data =>
    //         {
    //             Logger.Log(_data, _type: Type.Error);
    //             _callBack?.Invoke(new SignalRTokenResponse
    //             {
    //                 IsSuccessful = !string.IsNullOrEmpty(_data),
    //                 Token = _data
    //             });
    //         }, _result =>
    //     {
    //         Logger.Log(_result, _type: Type.Error);
    //         _callBack?.Invoke(new SignalRTokenResponse
    //         {
    //             IsSuccessful = false,
    //             Token = null
    //         });
    //     }));
    // }
    
    private IEnumerator Post(string _uri, string _jsonData, Action<string> _onSuccess, Action<string> _onError)
    {
        using UnityWebRequest _webRequest = UnityWebRequest.Post(_uri, _jsonData);
        using CertificateHandler _certificate = new ForceAcceptAll();
        byte[] _jsonToSend = new System.Text.UTF8Encoding().GetBytes(_jsonData);
        _webRequest.uploadHandler = new UploadHandlerRaw(_jsonToSend);
        _webRequest.downloadHandler = new DownloadHandlerBuffer();
        _webRequest.certificateHandler = _certificate;
        _webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return _webRequest.SendWebRequest();

        if (_webRequest.result == UnityWebRequest.Result.Success)
        {
            _onSuccess?.Invoke(_webRequest.downloadHandler.text);
        }
        else
        {
            Debug.Log(_uri);
            Debug.Log(_webRequest.error);
            Debug.Log(_webRequest.downloadHandler.text);
            Debug.Log(_webRequest.downloadHandler.data);
            _onError?.Invoke(_webRequest.error);
        }

        _webRequest.uploadHandler.Dispose();
        _webRequest.downloadHandler.Dispose();
        _certificate.Dispose();
        _webRequest.Dispose();
    }

}