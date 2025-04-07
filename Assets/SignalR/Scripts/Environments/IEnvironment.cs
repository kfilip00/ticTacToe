namespace UnitySignalR
{
    public interface IEnvironment
    {
        string GetHost();

        public virtual string GetGameHub()
        {
            return GetHost() + "/hubs/game";
        }
    }

}
