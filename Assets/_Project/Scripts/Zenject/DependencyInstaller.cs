using System;
using Authentication;
using Configuration;
using DataService;
using UnityEngine;
using UnitySignalR;

public class DependencyInstaller : MonoBehaviour
{
    [SerializeField] private WebRequestHandler webRequestHandler;
    private AuthenticationHandler authentication;
    private DataHandler dataHandler;
    private SignalRHandler signalRHandler;

    private void Start()
    {
        Config _config = CreateConfig();

        authentication = new AuthenticationHandler(_config, new AuthenticatorFactory(), webRequestHandler);
        dataHandler = new DataHandler(_config, new DataServiceFactory(), authentication, webRequestHandler);
        signalRHandler = new SignalRHandler(_config, new ClientFactory(), new EnvironmentFactory());
    }

    private Config CreateConfig()
    {
        return new Config
        {
            Database = DatabaseType.Firebase,
            IsTesting = true,
        };
    }
}