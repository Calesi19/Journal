namespace JournalApi.DTOs;

public record LoginResponse
{
    public bool IsSuccess { get; init; } = false;
    public string Message { get; init; } = string.Empty;
    public string AccessToken { get; init; } = string.Empty;
    public string RefreshToken { get; init; } = string.Empty;
}
