namespace JournalApi.DTOs;

public record GetPostsRequest
{
    public int? page { get; init; } = null;
    public int? LimitBy { get; init; } = null;
}
