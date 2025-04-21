namespace UnitySignalR
{
    public interface IServerFunctions
    {
        void SendMessage(string _sender, string _message);
    }
}