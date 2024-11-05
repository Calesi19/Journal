using JournalApi.Models;

namespace JournalApi.DTOs;

public record GetPostsResponse
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int PagesLeft { get; init; }
    public int TotalCount { get; init; }
    public List<Post> Posts { get; init; } = new List<Post>();
}
