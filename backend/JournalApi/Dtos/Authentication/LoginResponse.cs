namespace JournalApi.DTOs;

public record LoginResponse
{
    public string AccessToken { get; init; } = default!;
    public string RefreshToken { get; init; } = default!;
}
