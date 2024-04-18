using Journal_API.Data.Repositories;
using Journal_API.Models;

namespace Journal_API.Services;

public class JournalHandlerService
{
    private IJournalRepository _journalRepository;
    
    public JournalHandlerService(IJournalRepository repository)
    {
        _journalRepository = repository;
    }

    public async Task<IEnumerable<Journal>> GetJournals()
    {
        try
        {
            return await _journalRepository.FindAllAsync();
        }
        catch (Exception ex)
        {
            // Log the exception details here (optional)
            Console.WriteLine($"Error getting journals: {ex.Message}");

            // You can return an empty list or a specific error object
            return Enumerable.Empty<Journal>(); // Returns an empty list
            // OR
            // throw a custom exception for caller handling
            // throw new JournalServiceException("An error occurred while retrieving journals.", ex);
        }
    }
    
    public async Task<Journal> GetJournalByIdAsync(Guid id)
    {
        try
        {
            return await _journalRepository.FindByIdAsync(id);
        }
        catch (Exception ex)
        {
            // Log the exception details here (optional)
            Console.WriteLine($"Error getting journal with id {id}: {ex.Message}");

            // You can return null or a specific error object
            return null;  // Returns null
            // OR
            // throw a custom exception for caller handling
            // throw new JournalServiceException("An error occurred while retrieving journal with id " + id, ex);
        }
    }

    public async Task AddAsync(Journal newJournal)
    { 
        try
        {
            await _journalRepository.AddAsync(newJournal);
        }
        catch (Exception ex)
        {
            // Log the exception details here (optional)
            // You can use a logger library for this
            Console.WriteLine($"Error adding journal: {ex.Message}");

            // Re-throw a more specific exception for caller handling (optional)
            throw new Exception("An error occurred while adding the journal.", ex);
        }
    }
    
    
    

}