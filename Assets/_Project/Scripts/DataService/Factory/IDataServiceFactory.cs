using Authentication;

namespace DataService
{
    public interface IDataServiceFactory
    {
        IDataService Create(Config _config,IAuthentication _authentication, IWebRequests _webRequests);
    }   
}
