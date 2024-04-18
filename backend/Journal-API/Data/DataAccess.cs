using Journal_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Journal_API.Data;

public class AppDbContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public AppDbContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql(Configuration.GetConnectionString("PostgresDBConnection"));
    }
    
    // Add DbSet properties here
    public DbSet<Journal> Journals { get; set; }
    
}