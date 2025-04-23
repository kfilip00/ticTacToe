using Authentication;
using Configuration;
using DataService;
using UnitySignalR;
using Zenject;

public class DependencyInstaller : MonoInstaller
{
    private AuthenticationHandler authentication;
    private DataHandler dataHandler;
    private SignalRHandler signalRHandler;

    public override void InstallBindings()
    {
        Config _config = CreateConfig();

        Container.BindInstance(_config).AsSingle();

        Container.Bind<AuthenticatorFactory>().AsSingle();
        Container.Bind<DataServiceFactory>().AsSingle();
        Container.Bind<ClientFactory>().AsSingle();
        Container.Bind<EnvironmentFactory>().AsSingle();

        Container.Bind<WebRequestHandler>().AsSingle();
        Container.Bind<AuthenticationHandler>().AsSingle();
        Container.Bind<DataHandler>().AsSingle();
        Container.Bind<SignalRHandler>().AsSingle();
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