using UnityEngine.Networking;

public class ForceAcceptAll : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] _certificateData)
    {
        return true;
    }
}