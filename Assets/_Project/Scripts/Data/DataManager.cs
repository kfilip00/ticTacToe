using System;
using TicTacToe.Firebase;
using Newtonsoft.Json;
using UnityEngine;
using Random = UnityEngine.Random;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public PlayerData PlayerData { get; private set; }
    private bool hasSubscribedEvents;
    
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

    public void SetupNewPlayer()
    {
        PlayerData = new PlayerData
        {
            Id = FirebaseManager.Instance.UserLocalId,
            Username = "Player"+Random.Range(100000,1000000)
        };
    }

    public void CollectData(Action<bool> _callBack)
    {
        FirebaseManager.Instance.GetPlayerData(_response =>
        {
            if (!_response.IsSuccessful)
            {
                _callBack?.Invoke(false);
                return;
            }

            PlayerData = JsonConvert.DeserializeObject<PlayerData>(_response.Json);
            _callBack?.Invoke(true);
        });
    }

    public void SubscribeEvents()
    {
        if (hasSubscribedEvents)
        {
            return;
        }

        hasSubscribedEvents = true;
        
    }

    public void UnSubscribeEvents()
    {
        if (!hasSubscribedEvents)
        {
            return;
        }

        hasSubscribedEvents = false;
    }
}
