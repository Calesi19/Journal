namespace JournalApi.DTOs;

public record CreateAccountResponse
{
    public bool IsSuccess { get; init; } = false;
    public string Email { get; init; } = default!;
}
