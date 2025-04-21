using Authentication;
using Configuration;
using DataService;
using UnityEngine;
using UnitySignalR;

public class Inicializator : MonoBehaviour
{
    [SerializeField] private WebRequestHandler webRequestHandler;
    private AuthenticationHandler authentication;
    private DataHandler dataHandler;
    private SignalRHandler signalRHandler;

    private void Start()
    {
        Config _config = new Config
        {
            Database = DatabaseType.Firebase,
            IsTesting = true,
        };
        
        authentication = new AuthenticationHandler(_config,new AuthenticatorFactory(), webRequestHandler);
        dataHandler = new DataHandler(_config,new DataServiceFactory(), authentication, webRequestHandler);
        signalRHandler = new SignalRHandler(_config,new ClientFactory(), new EnvironmentFactory());
    }
}