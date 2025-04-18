using Authentication.Firebase;

namespace Authentication
{
    public class AuthenticatorFactory: IAuthenticatorFactory
    {
        public IAuthentication Create(IWebRequests _webRequests)
        {
            return new FirebaseAuthenticator(_webRequests);
        }
    }
}