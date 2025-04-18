namespace Authentication
{
    public interface IAuthenticatorFactory
    {
        IAuthentication Create(IWebRequests _webRequests);
    }   
}