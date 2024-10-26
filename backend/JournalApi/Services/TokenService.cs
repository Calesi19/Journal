using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JournalApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace JournalApi.Services;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken(User user);
    bool VerifyRefreshToken(User user, string token);
    Guid GetUserIdFromToken(string token);
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
            new("userId", user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new("isEmailConfirmed", user.IsEmailConfirmed.ToString()),
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(10),
            SigningCredentials = new(_key, SecurityAlgorithms.HmacSha256Signature),
        };

        var token = _tokenHandler.CreateToken(tokenDescriptor);
        return _tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken(User user)
    {
        var claims = new List<Claim> { new("userId", user.Id.ToString()) };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(5),
            SigningCredentials = new(_key, SecurityAlgorithms.HmacSha256Signature),
        };

        var token = _tokenHandler.CreateToken(tokenDescriptor);
        return _tokenHandler.WriteToken(token);
    }

    public Guid GetUserIdFromToken(string token)
    {
        if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            token = token.Substring("Bearer ".Length).Trim();
        }

        try
        {
            var jwtToken = _tokenHandler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "userId");

            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return userId;
            }
        }
        catch (Exception ex)
        {
            // Consider logging the exception or handling it gracefully
            Console.WriteLine($"Token parsing failed: {ex.Message}");
        }

        // Return a default GUID if parsing fails
        return Guid.Empty;
    }

    public bool VerifyRefreshToken(User user, string token)
    {
        try
        {
            var jwtToken = _tokenHandler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "userId");

            if (userIdClaim != null && userIdClaim.Value == user.Id.ToString())
            {
                return true;
            }
        }
        catch
        {
            throw new Exception("Could not verify refresh token.");
        }

        return false;
    }
}
