namespace JournalApi.DTOs;

// Wrapper class for API requests

public class ApiRequest<T>
{
    public T Request { get; set; } = default!;
}
