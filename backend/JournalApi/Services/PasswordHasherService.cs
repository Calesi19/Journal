using JournalApi.Models;
using Microsoft.AspNetCore.Identity;

namespace JournalApi.Services;

public interface IPasswordHasherService
{
    string HashPassword(string password);
    bool VerifyPasswordMatch(string password, string passwordHash);
}

public class PasswordHasherService : IPasswordHasherService
{
    private readonly IPasswordHasher<User> _passwordHasher;

    public PasswordHasherService(IPasswordHasher<User> passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public string HashPassword(string password)
    {
        return _passwordHasher.HashPassword(null!, password);
    }

    public bool VerifyPasswordMatch(string password, string passwordHash)
    {
        return _passwordHasher.VerifyHashedPassword(null!, passwordHash, password)
            == PasswordVerificationResult.Success;
    }
}

public class BCryptPasswordHasherService : IPasswordHasherService
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPasswordMatch(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}
