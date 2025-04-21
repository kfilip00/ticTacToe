namespace Authentication
{
    public interface IAuthenticatorFactory
    {
        IAuthentication Create(Config _config,IWebRequests _webRequests);
    }   
}