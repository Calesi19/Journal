using Journal_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Journal_API.Data.Repositories;

public class JournalRepository : IJournalRepository
{
    private readonly AppDbContext _context;
    
    public JournalRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Journal>> FindAllAsync()
    {
        return await _context.Journals.ToListAsync();
    }
    
    public async Task<Journal> FindByIdAsync(Guid id)
    {
        return await _context.Journals.FindAsync(id);
    }

    public async Task AddAsync(Journal newJournal)
    {
        _context.Journals.Add(newJournal);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Journal targetJournal)
    {
        _context.Journals.Update(targetJournal);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var targetJournal = await _context.Journals.FindAsync(id);
        if (targetJournal != null)
        {
            _context.Journals.Remove(targetJournal);
            await _context.SaveChangesAsync();
        }
    }
}

    
    
