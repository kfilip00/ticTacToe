using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using Type = LoggerNS.Type;

namespace TicTacToe.Firebase
{
    public class FirebaseManager : MonoBehaviour
    {
        public const string EMAIL_DOMAIN = "tictactoe.com";
        private const string WEB_API_KEY = "AIzaSyAWWXYVDfI0HEfR-0G0KJERPYck-l5yf9E";

        private const string DATABASE_LINK = "https://tictactoe-25485-default-rtdb.firebaseio.com/";
        private const string USERS_LINK = DATABASE_LINK + "users/";

        public static FirebaseManager Instance;

        private string userLocalId;
        private string token;

        public string UserLocalId => userLocalId;
        public string Token => token;

        public bool IsAuthenticated => !string.IsNullOrEmpty(userLocalId);

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        public void Authenticate(string _email, string _password, Action<AuthResponse> _callBack)
        {
            string _loginParms = "{\"email\":\"" + _email + "\",\"password\":\"" + _password + "\",\"returnSecureToken\":true}";

            StartCoroutine(Post("https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=" + WEB_API_KEY, _loginParms, _result =>
            {
                SignInResponse _signInResponse = JsonConvert.DeserializeObject<SignInResponse>(_result);
                userLocalId = _signInResponse.LocalId;
                token = _signInResponse.IdToken;
                _callBack?.Invoke(new AuthResponse { IsNewAccount = false });
            }, _ =>
            {
                Register(_loginParms,_callBack);
            }, false));
        }

        private void Register(string _parms,Action<AuthResponse> _callBack)
        {
            StartCoroutine(Post("https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=" + WEB_API_KEY, _parms, _result =>
            {
                RegisterResponse _registerResult = JsonConvert.DeserializeObject<RegisterResponse>(_result);
                userLocalId = _registerResult.LocalId;
                token = _registerResult.IdToken;
                _callBack?.Invoke(new AuthResponse { IsNewAccount = true });
            }, _result =>
            {
                Logger.Log(_result, _type:Type.Error);
                _callBack?.Invoke(new AuthResponse {IsSuccessful = false });
            }));
        }

        public void GetPlayerData(Action<PlayerDataResponse> _callBack)
        {
            string _url = $"{USERS_LINK}{userLocalId}.json?auth={token}";
            StartCoroutine(GetRequest(_url, _data =>
            {
                bool _isSuccessful = !string.IsNullOrEmpty(_data);
                PlayerDataResponse _response = new PlayerDataResponse
                {
                    IsSuccessful = _isSuccessful,
                    Json = _data
                };

                _callBack?.Invoke(_response);
            }));
        }

        public void SavePlayerData(string _json, Action _callBack)
        {
            string _url = $"{USERS_LINK}/{userLocalId}.json?auth={token}";
            StartCoroutine(PutRequest(_url, _json, _callBack));
        }

        private IEnumerator GetRequest(string _url, Action<string> _callback)
        {
            using UnityWebRequest _webRequest = UnityWebRequest.Get(_url);
            yield return _webRequest.SendWebRequest();

            if (_webRequest.result == UnityWebRequest.Result.Success)
            {
                _callback?.Invoke(_webRequest.downloadHandler.text);
            }
            else
            {
                Debug.LogError($"Error getting data: {_webRequest.error}");
                _callback?.Invoke(null);
            }
        }

        private IEnumerator PutRequest(string _url, string _jsonData, Action _callBack)
        {
            byte[] _bodyRaw = Encoding.UTF8.GetBytes(_jsonData);
            using UnityWebRequest _webRequest = new UnityWebRequest(_url, "PUT");
            _webRequest.uploadHandler = new UploadHandlerRaw(_bodyRaw);
            _webRequest.downloadHandler = new DownloadHandlerBuffer();
            _webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return _webRequest.SendWebRequest();

            if (_webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error saving data: {_webRequest.error}");
            }
            
            _callBack?.Invoke();
        }

        private IEnumerator Post(string _uri, string _jsonData, Action<string> _onSuccess, Action<string> _onError, bool _includeHeader = true)
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
                _onError?.Invoke(_webRequest.error);
            }

            _webRequest.uploadHandler.Dispose();
            _webRequest.downloadHandler.Dispose();
        }
    }
}