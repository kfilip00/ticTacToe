using Authentication;
using DataService;
using UnityEngine;

public class Inicializator : MonoBehaviour
{
    [SerializeField] private WebRequestHandler webRequestHandler;
    private AuthenticationHandler authentication;
    private DataHandler dataHandler;

    private void Start()
    {
        authentication = new AuthenticationHandler(new AuthenticatorFactory(), webRequestHandler);
        dataHandler = new DataHandler(new DataServiceFactory(), authentication.GetPlayerIdentifier(), authentication.GetPlayerToken(), webRequestHandler);
    }
}