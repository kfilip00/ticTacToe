using System.Text.RegularExpressions;

public static class CredentialsValidator
{
    public static ValidationResult ValidateEmail(string _email)
    {
        if (string.IsNullOrWhiteSpace(_email))
        {
            return new ValidationResult
            {
                IsValid = false,
                ErrorMessage = "Email cannot be empty."
            };
        }

        string _emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        if (!Regex.IsMatch(_email, _emailPattern))
        {
            return new ValidationResult
            {
                IsValid = false,
                ErrorMessage = "Invalid email format. Ensure it follows the pattern: user@domain.com."
            };
        }

        if (_email.Length > 254)
        {
            return new ValidationResult
            {
                IsValid = false,
                ErrorMessage = "Email is too long. Maximum length is 254 characters."
            };
        }

        return new ValidationResult
        {
            IsValid = true,
            ErrorMessage = string.Empty
        };
    }

    public static ValidationResult ValidatePassword(string _password)
    {
        if (string.IsNullOrWhiteSpace(_password))
        {
            return new ValidationResult
            {
                IsValid = false,
                ErrorMessage = "Password cannot be empty."
            };
        }

        if (_password.Length < 6)
        {
            return new ValidationResult
            {
                IsValid = false,
                ErrorMessage = "Password must be at least 6 characters long."
            };
        }

        if (_password.Length > 128)
        {
            return new ValidationResult
            {
                IsValid = false,
                ErrorMessage = "Password is too long. Maximum length is 128 characters."
            };
        }

        return new ValidationResult
        {
            IsValid = true,
            ErrorMessage = string.Empty
        };
    }
}