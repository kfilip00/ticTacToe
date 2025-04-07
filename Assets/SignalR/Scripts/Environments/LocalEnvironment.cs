namespace UnitySignalR
{
    public class LocalEnvironment : IEnvironment
    {
        public string GetHost()
        {
            return "http://localhost:5113";
        }
    }
}