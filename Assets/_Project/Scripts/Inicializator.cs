using Authentication;
using UnityEngine;

public class Inicializator : MonoBehaviour
{
    [SerializeField] private WebRequestHandler webRequestHandler;
    private void Start()
    {
        Auth();
    }

    private void Auth()
    {
        AuthenticationHandler _authentication = new AuthenticationHandler(new AuthenticatorFactory(), webRequestHandler);
       
    }
}