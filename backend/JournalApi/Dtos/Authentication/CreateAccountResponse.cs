namespace JournalApi.DTOs;

public record CreateAccountResponse
{
    public bool IsSuccess { get; init; } = false;
    public string Email { get; init; } = default!;
    public string AccessToken { get; init; } = default!;
    public string RefreshToken { get; init; } = default!;
}
