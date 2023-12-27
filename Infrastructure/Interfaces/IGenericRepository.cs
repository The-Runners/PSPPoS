namespace Infrastructure.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetAll();
        public Task<T?> GetById(Guid id);
        public Task<T> Add(T obj);
        public Task<T> Update(T obj);
        public Task Delete(Guid id);
    }
}
