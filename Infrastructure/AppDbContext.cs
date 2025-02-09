using Microsoft.EntityFrameworkCore;
using RegisTrackerSystem;

namespace Infrastructure;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Individual> Individuals { get; set; }
    public DbSet<BatchStatistics> BatchStatistics { get; set; }
}
