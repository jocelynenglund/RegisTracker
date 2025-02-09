using Microsoft.EntityFrameworkCore;
using RegisTrackerSystem.Domain;

namespace Infrastructure.SQLAdapter.Migrations;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Individual> Individuals { get; set; }
    public DbSet<BatchStatistics> BatchStatistics { get; set; }
}
