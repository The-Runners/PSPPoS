namespace Infrastructure.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> ListAsync(int offset, int limit);

    Task<T?> GetById(Guid? id);

    Task<T> Add(T entity);

    Task<T> Update(T entity);

    Task Delete(Guid? id);

    Task SaveChangesAsync();
}