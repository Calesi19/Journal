
namespace Journal_API.Models;

public class Journal
{
    public Guid JournalId { get; init; }
    
    public string EntryContent { get; set; }
    
    public DateTime EntryDate { get; init; }
    
    public string[] Categories { get; set; }
    
    public DateTime UpdatedDate { get; set; }
    
}
