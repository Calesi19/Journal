namespace JournalApi.DTOs;

// Wrapper class for API responses

public class ApiResponse<T>
{
    public T Response { get; set; } = default!;
    public string Message { get; set; } = string.Empty;
}
