namespace JournalApi.Services;

public interface ITokenService
{
    string GenerateAccessToken(UserModel user);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
