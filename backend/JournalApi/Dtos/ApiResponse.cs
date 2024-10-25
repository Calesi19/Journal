namespace JournalApi.DTOs;

public class ApiResponse<T> {
  public T Response { get; set; } = default!;
}
