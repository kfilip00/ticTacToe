namespace UnitySignalR
{
    public class ProductionEnvironment : IEnvironment
    {
        public string GetHost()
        {
            return "https://tictactoe-250326183105.azurewebsites.net";
        }
    }
}