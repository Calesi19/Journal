namespace JournalApi.DTOs;

public class QueryParameters
{
    public string? SearchText { get; set; } = null;
    public int? PageNumber { get; set; } = null;
    public int? PageSize { get; set; } = null;
}
