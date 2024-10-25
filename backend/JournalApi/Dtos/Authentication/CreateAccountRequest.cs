namespace JournalApi.DTOs;

public record CreateAccountRequest
{
    public string Email { get; init; } = default!;
    public string Password { get; init; } = default!;
}
