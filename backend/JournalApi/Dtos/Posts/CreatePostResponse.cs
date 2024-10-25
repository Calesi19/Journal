namespace JournalApi.DTOs;

public record CreatePostResponse
{
    public bool IsSuccess { get; init; } = false;
    public Guid PostId { get; init; } = default!;
}
