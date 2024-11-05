using JournalApi.Models;

namespace JournalApi.DTOs;

public record GetPostsResponse
{
    public List<Post> Posts { get; init; } = new List<Post>();
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
}
