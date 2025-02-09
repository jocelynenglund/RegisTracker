using Infrastructure.SQLAdapter.Migrations;
using Microsoft.EntityFrameworkCore;
using RegisTrackerSystem;
using RegisTrackerSystem.Domain;

namespace Infrastructure.SQLAdapter;

public class SQLRepository<T> : IRepository<T> where T : class, AggregateRoot
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;
    public SQLRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }
    public IEnumerable<T> GetAll()
    {
        return _dbSet.ToList();
    }

    public async Task Save(T entity)
    {
        if (!_dbSet.Contains(entity))
            _dbSet.Add(entity);
        await _context.SaveChangesAsync();
    }
}