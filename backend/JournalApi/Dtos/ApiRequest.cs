namespace JournalApi.DTOs;

public class ApiRequest<T> {
  public T Request { get; set; } = default!;
}
