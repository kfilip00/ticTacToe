using Authentication;
using Configuration;
using DataService;
using UnityEngine;
using UnitySignalR;

public class ServiceProvider : Singleton<ServiceProvider>
{
    [SerializeField] private WebRequestHandler webRequestHandler;
    public AuthenticationHandler Authentication { get; private set; }
    public DataHandler DataHandler { get; private set; }
    public SignalRHandler SignalRHandler { get; private set; }

    private void Start()
    {
        Config _config = CreateConfig();

        Authentication = new AuthenticationHandler(_config, new AuthenticatorFactory(), webRequestHandler);
        DataHandler = new DataHandler(_config, new DataServiceFactory(), Authentication, webRequestHandler);
        SignalRHandler = new SignalRHandler(_config, new ClientFactory(), new EnvironmentFactory());
        
        SceneManager.Instance.LoadScene(SceneManager.AUTHENTICATE);
    }

    private Config CreateConfig()
    {
        return new Config { Database = DatabaseType.Firebase, IsTesting = true, };
    }
}