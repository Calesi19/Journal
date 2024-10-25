using System.ComponentModel.DataAnnotations;

namespace JournalApi.DTOs;

public record CreatePostRequest
{
    [Required]
    public string Content { get; init; } = default!;
}
