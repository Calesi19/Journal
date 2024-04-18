using Journal_API.Models;

namespace Journal_API.Data.Repositories;

public interface IJournalRepository
{
    Task<IEnumerable<Journal>> FindAllAsync();
    Task<Journal> FindByIdAsync(Guid id);
    Task AddAsync(Journal newJournal);
    Task UpdateAsync(Journal targetJournal);
    Task DeleteAsync(Guid id);
}