using Authentication;
using Configuration;
using DataService;
using UnityEngine;
using UnitySignalR;

public class ServiceProvider : Singleton<ServiceProvider>
{
    [SerializeField] private WebRequestHandler webRequestHandler;
    public IAuthentication Authentication { get; private set; }
    public IDataService DataHandler { get; private set; }
    public SignalRHandler SignalRHandler { get; private set; }

    public Config Config { get; private set; }

    private void Start()
    {
        Config = CreateConfig();

        Authentication = new AuthenticatorFactory().Create(Config,webRequestHandler);
        DataHandler = new DataServiceFactory().Create(Config, Authentication, webRequestHandler);
        SignalRHandler = new SignalRHandler(Config, new ClientFactory(), new EnvironmentFactory());
        
        SceneManager.Instance.LoadScene(SceneManager.AUTHENTICATE);
    }

    private Config CreateConfig()
    {
        return new Config { Database = DatabaseType.Firebase, IsTesting = true, };
    }
}