using UnityEngine;

namespace UnitySignalR
{
    public class ClientFactory : IClientFactory
    {
        public IClient CreateClient()
        {
            return Application.isEditor
                ? new EditorClient()
                : new WebGlClient();
        }
    }
}

