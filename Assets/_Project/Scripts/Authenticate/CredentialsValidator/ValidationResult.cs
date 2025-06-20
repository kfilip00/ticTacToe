using System;

namespace Authenticate
{
    [Serializable]
    public class ValidationResult
    {
        public bool IsValid;
        public string ErrorMessage;
    }
}