namespace JournalApi.Services;

public interface IPasswordStrengthValidatorService
{
    bool IsPasswordStrong(string password);
}

public class PasswordStrengthValidatorService : IPasswordStrengthValidatorService
{
    public bool IsPasswordStrong(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return false;
        }

        if (password.Length < 8)
        {
            return false;
        }

        if (!password.Any(char.IsDigit))
        {
            return false;
        }

        if (!password.Any(char.IsUpper))
        {
            return false;
        }

        if (!password.Any(char.IsLower))
        {
            return false;
        }

        return true;
    }
}
