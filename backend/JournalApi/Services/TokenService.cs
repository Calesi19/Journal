using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using JournalApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace JournalApi.Services;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
}

public class TokenService : ITokenService
{
    private readonly SymmetricSecurityKey _key;
    private readonly JwtSecurityTokenHandler _tokenHandler;

    public TokenService(IConfiguration config)
    {
        _key = new(Encoding.UTF8.GetBytes(config["JwtConfig:SecretKey"]));
        _tokenHandler = new();
    }

    public string GenerateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = new(_key, SecurityAlgorithms.HmacSha256Signature),
        };

        var token = _tokenHandler.CreateToken(tokenDescriptor);
        return _tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}