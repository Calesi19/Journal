namespace JournalApi.DTOs;

public record ActionStatusResponse {
  public bool IsSuccess { get; init; } = false;
}
