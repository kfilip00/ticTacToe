namespace UnitySignalR
{
    public interface IEnvironmentFactory
    {
        IEnvironment Create(bool _isTesting);
    }
}