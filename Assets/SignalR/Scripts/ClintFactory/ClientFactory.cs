namespace UnitySignalR
{
    public class ClientFactory : IClientFactory
    {
        public IClient CreateClient()
        {
            return new StandardClient();
        }
    }
}