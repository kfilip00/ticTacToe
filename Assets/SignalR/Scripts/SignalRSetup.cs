using UnityEngine;

public class SignalRSetup : MonoBehaviour
{
    [SerializeField] private bool useLocalHost;
    [SerializeField] private string localUrl;
    [SerializeField] private string serverUrl;
    
    private string ServerUri => useLocalHost ? localUrl : serverUrl;
    public string GameHub => ServerUri + "/hubs/game";
}
