using System;
using Configuration;

namespace DataService
{
    public class DataServiceFactory : IDataServiceFactory
    {
        public IDataService Create(Config _config,AuthenticationHandler _authentication, IWebRequests _webRequests)
        {
            switch (_config.Database)
            {
                case DatabaseType.Firebase:
                    return new Firebase(_authentication, _webRequests);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}