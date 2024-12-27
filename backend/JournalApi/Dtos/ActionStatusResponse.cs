namespace JournalApi.DTOs;

public record ActionStatusResponse
{
    public bool IsSuccess { get; init; } = false;
    public string Message { get; init; } = string.Empty;
}
