using System;
using Configuration;

namespace Authentication
{
    public class AuthenticatorFactory: IAuthenticatorFactory
    {
        public IAuthentication Create(Config _config,IWebRequests _webRequests)
        {
            switch (_config.Database)
            {
                case DatabaseType.Firebase:
                    return new Firebase.Firebase(_webRequests);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}