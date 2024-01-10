using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _table;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
        _table = _context.Set<T>();
    }

    public async Task<IEnumerable<T>> ListAsync(int offset, int limit)
    {
        return await _table
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<T?> GetById(Guid id)
    {
        return await _table.FindAsync(id);
    }

    public async Task<T> Add(T entity)
    {
        await _table.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> Update(T entity)
    {
        _table.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task Delete(Guid? id)
    {
        var entity = await _table.FindAsync(id);
        if (entity is null)
        {
            return;
        }

        _table.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _table.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}