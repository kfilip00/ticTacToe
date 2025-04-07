using UnitySignalR;

public class EnvironmentFactory : IEnvironmentFactory
{
    public IEnvironment Create(bool _isTesting)
    {
        return _isTesting ? new LocalEnvironment() : new ProductionEnvironment();
    }
}
