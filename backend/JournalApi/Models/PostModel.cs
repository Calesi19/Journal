namespace JournalApi.Models;

public class Post
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public Guid UserId { get; set; } = Guid.NewGuid();
}
