using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;

        private DbSet<T> _table;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _table = _context.Set<T>();
        }

        public async Task<T> Add(T entity)
        {
            await _table.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(Guid id)
        {
            T? t = await _table.FindAsync(id);
            if (t == null)
            {
                return;
            }
            T existing = t;
            _table.Remove(existing);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _table.ToListAsync();
        }

        public async Task<T?> GetById(Guid id)
        {
            return await _table.FindAsync(id);
        }

        public async Task<T> Update(T entity)
        {
            _table.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
