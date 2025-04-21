namespace DataService
{
    public interface IDataServiceFactory
    {
        IDataService Create(Config _config,AuthenticationHandler _authentication, IWebRequests _webRequests);
    }   
}
