using System;
using SceneManagement;
using UnityEngine;

public class SceneManager : SceneLoader
{
    public const string MAIN_MENU = "MainMenu";
    public const string AUTHENTICATE = "Authenticate";
    public const string GAMEPLAY = "Gameplay";
    
    public static SceneManager Instance;
    
    [SerializeField] private bool useAsync;

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

    public void LoadScene(string _key, LoadingType _type = default)
    {
        DoLoadScene(_key, _type);
    }
    
    public void ReloadScene(LoadingType _type = default)
    {
        DoLoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,_type);
    }

    private void DoLoadScene(string _key,LoadingType _type)
    {
        switch (_type)
        {
            case LoadingType.Default:
                LoadScene(_key, useAsync);
                break;
            case LoadingType.Async:
                LoadScene(_key, true);
                break;
            case LoadingType.NonAsync:
                LoadScene(_key, false);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(_type), _type, null);
        }
    }
}
