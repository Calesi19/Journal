namespace JournalApi;

public class UserModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool emailConfirmed { get; set; } = false;
}
