using Domain.Models;

namespace Infrastructure.Repositories;
public class ServiceRepository : GenericRepository<Service>
{
    public ServiceRepository(AppDbContext context) : base(context) { }
}