namespace DataService
{
    public interface IDataServiceFactory
    {
        IDataService Create(string _playerIdentifier, string _playerToken, IWebRequests _webRequests);
    }   
}
