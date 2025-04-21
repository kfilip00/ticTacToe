using System;

namespace UnitySignalR
{
    [Serializable]
    public enum ConnectionStatus
    {
        Failed = 0,
        Successful = 1,
        AlreadyConnected =2
    }
}