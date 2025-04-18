using Authentication;
using UnityEngine;

public class Inicializator : MonoBehaviour
{
    [SerializeField] private WebRequestHandler webRequestHandler;
    private AuthenticationHandler authentication;
    
    private void Start()
    {
        authentication = new AuthenticationHandler(new AuthenticatorFactory(), webRequestHandler);
    }
}