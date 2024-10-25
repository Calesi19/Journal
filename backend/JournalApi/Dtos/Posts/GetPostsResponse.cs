using JournalApi.Models;

namespace JournalApi.DTOs;

public record GetPostsResponse
{
    public List<Post> Posts { get; init; } = new List<Post>();
}
