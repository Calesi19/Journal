namespace JournalApi.DTOs;

public class QueryParameters
{
    public string SearchText { get; set; } = string.Empty;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 25;
}
