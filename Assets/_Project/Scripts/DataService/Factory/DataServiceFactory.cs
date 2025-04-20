namespace DataService
{
    public class DataServiceFactory : IDataServiceFactory
    {
        public IDataService Create(string _playerIdentifier, string _playerToken, IWebRequests _webRequests)
        {
            return new Firebase(_playerIdentifier, _playerToken, _webRequests);
        }
    }
}